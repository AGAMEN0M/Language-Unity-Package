/*
 * ---------------------------------------------------------------------------
 * Description: Provides a Project Settings interface for configuring the 
 *              LanguageSettingsData ScriptableObject directly inside Unity.
 *              
 *              This system centralizes localization configuration such as:
 *              - Language folder selection inside StreamingAssets.
 *              - Default language selection using system cultures.
 *              - Font assignment for both Legacy UI and TextMeshPro.
 *              - Error handling UI reference.
 *              
 *              The implementation ensures performance by caching the data asset
 *              and applying changes only when necessary, avoiding continuous
 *              asset reloading and editor refresh loops.
 * 
 * Author: Lucas Gomes Cecchini
 * Pseudonym: AGAMENOM
 * ---------------------------------------------------------------------------
*/

using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using System;

namespace LanguageTools.Editor
{
    #region === Settings Provider ===

    /// <summary>
    /// Provides a Unity Project Settings panel for editing LanguageSettingsData.
    /// This replaces the need to manually locate and edit the ScriptableObject.
    /// </summary>
    public static class LanguageSettingsWindow
    {
        #region === Fields ===

        /// <summary>
        /// Cached reference to the LanguageSettingsData asset.
        /// Prevents unnecessary calls to Resources.Load every GUI repaint.
        /// </summary>
        private static LanguageSettingsData cachedData;

        #endregion

        #region === Settings Provider ===

        /// <summary>
        /// Creates and registers the SettingsProvider used in Unity Project Settings.
        /// </summary>
        /// <returns>The configured SettingsProvider instance.</returns>
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new SettingsProvider("Project/Language Tool", SettingsScope.Project)
            {
                label = "Language Tool",

                guiHandler = (searchContext) =>
                {
                    // Load the asset only once and cache it.
                    if (cachedData == null) cachedData = LanguageFileManager.LoadLanguageSettings();

                    var data = cachedData;

                    EditorGUILayout.Space(5);

                    EditorGUILayout.HelpBox("Configure localization settings such as language, fonts, and archive paths.", MessageType.Info);

                    EditorGUILayout.Space(5);

                    #region === Missing Asset ===

                    // If the asset is not found, provide a creation button.
                    if (data == null)
                    {
                        EditorGUILayout.HelpBox("LanguageSettingsData asset not found.", MessageType.Warning);

                        if (GUILayout.Button(new GUIContent("Create Settings Asset"), GUILayout.Height(30)))
                        {
                            // Create or retrieve the asset automatically.
                            cachedData = LanguageDataAutoCreator.CreateOrGetData(false);

                            if (cachedData != null)
                            {
                                // Highlight and select the asset in Project view.
                                EditorGUIUtility.PingObject(cachedData);
                                Selection.activeObject = cachedData;
                            }

                            GUIUtility.ExitGUI(); // Exit GUI to avoid layout issues after creation.
                        }

                        return;
                    }

                    #endregion

                    SerializedObject serializedData = new(data); // Create a serialized representation of the asset.
                    serializedData.Update(); // Sync serialized data with current object state.

                    #region === Data Asset ===

                    EditorGUILayout.LabelField("Data Asset", EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(data, typeof(LanguageSettingsData), false);
                    EditorGUI.EndDisabledGroup();

                    // Button to highlight the asset.
                    if (GUILayout.Button(new GUIContent("Ping", "Highlight asset in Project window."), GUILayout.MaxWidth(50)))
                    {
                        EditorGUIUtility.PingObject(data);
                        Selection.activeObject = data;
                    }

                    // Button to open the asset in inspector.
                    if (GUILayout.Button(new GUIContent("Open", "Open asset in Inspector."), GUILayout.MaxWidth(50)))
                    {
                        EditorUtility.OpenPropertyEditor(data);
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(10);

                    #endregion

                    #region === Folder Selection ===

                    EditorGUILayout.LabelField("Archives Location", EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();

                    // Label aligned manually to match Unity style.
                    EditorGUILayout.LabelField(new GUIContent("Folder Name", "Folder inside StreamingAssets used for language files."), GUILayout.Width(EditorGUIUtility.labelWidth - 4));

                    EditorGUI.BeginChangeCheck(); // Begin change check for folder name field.

                    // Editable text field for folder name.
                    var folderProp = serializedData.FindProperty("folderName");
                    folderProp.stringValue = EditorGUILayout.TextField(folderProp.stringValue);

                    // Button to open folder picker.
                    if (GUILayout.Button(new GUIContent("Select", "Select a folder inside StreamingAssets"), GUILayout.Width(70)))
                    {
                        string basePath = Application.streamingAssetsPath;

                        // Open OS folder selection dialog.
                        string selectedPath = EditorUtility.OpenFolderPanel("Select Language Folder", basePath, "");

                        if (!string.IsNullOrEmpty(selectedPath))
                        {
                            // Ensure selected folder is inside StreamingAssets.
                            if (selectedPath.StartsWith(basePath))
                            {
                                Undo.RecordObject(data, "Change Language Folder");

                                // Convert absolute path to relative path.
                                string relative = selectedPath[basePath.Length..].TrimStart('/', '\\');
                                folderProp.stringValue = relative;

                                EditorUtility.SetDirty(data);
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Invalid Folder", "Selected folder must be inside StreamingAssets.", "OK");
                            }
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    // Register undo if text field changed.
                    if (EditorGUI.EndChangeCheck()) Undo.RecordObject(data, "Edit Folder Name");

                    EditorGUILayout.Space(10);

                    #endregion

                    #region === Default Language ===

                    var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures); // Retrieve all available system cultures.
                    string[] cultureNames = new string[cultures.Length]; // Prepare display names for dropdown.

                    for (int i = 0; i < cultures.Length; i++) cultureNames[i] = cultures[i].NativeName;

                    int index = Array.FindIndex(cultures, c => c.Name == data.defaultLanguage); // Find current selected index.
                    index = Mathf.Clamp(index, 0, cultures.Length - 1); // Clamp index to valid range.

                    EditorGUILayout.LabelField("Default Language", EditorStyles.boldLabel);

                    // Draw dropdown.
                    int newIndex = EditorGUILayout.Popup(new GUIContent("Language", "Select the default language for the project."), index, cultureNames);

                    // Apply change if selection changed.
                    if (newIndex != index)
                    {
                        Undo.RecordObject(data, "Change Default Language");
                        serializedData.FindProperty("defaultLanguage").stringValue = cultures[newIndex].Name;
                        EditorUtility.SetDirty(data);
                    }

                    EditorGUILayout.Space(10);

                    #endregion

                    #region === Font List Data ===

                    EditorGUI.BeginChangeCheck();

                    // Draw ScriptableObject reference.
                    var fontProp = serializedData.FindProperty("fontListData");
                    EditorGUILayout.PropertyField(fontProp);

                    // Apply change if modified.
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(data, "Change Font List Data");
                        EditorUtility.SetDirty(data);
                    }

                    // Draw nested font list if assigned.
                    if (data.fontListData != null)
                    {
                        SerializedObject fontSO = new(data.fontListData);
                        fontSO.Update();
                        EditorGUILayout.PropertyField(fontSO.FindProperty("fontList"), true);
                        fontSO.ApplyModifiedProperties();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Font List Data is not assigned. Legacy UI text may not render correctly.", MessageType.Error);
                    }

                    EditorGUILayout.Space(10);

                    #endregion

                    #region === TMP Font List ===

                    var tmpProp = serializedData.FindProperty("fontListDataTMP");

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(tmpProp);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(data, "Change TMP Font List Data");
                        EditorUtility.SetDirty(data);
                    }

                    if (data.fontListDataTMP != null)
                    {
                        SerializedObject tmpSO = new(data.fontListDataTMP);
                        tmpSO.Update();
                        EditorGUILayout.PropertyField(tmpSO.FindProperty("TMPFontList"), true);
                        tmpSO.ApplyModifiedProperties();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("TMP Font List Data is not assigned. TextMeshPro text may not render correctly.", MessageType.Error);
                    }

                    EditorGUILayout.Space(10);

                    #endregion

                    #region === Error Canvas ===

                    EditorGUI.BeginChangeCheck();

                    // Draw reference to error UI prefab.
                    var errorProp = serializedData.FindProperty("errorLanguageTool");
                    EditorGUILayout.PropertyField(errorProp);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(data, "Change Error Canvas");
                        EditorUtility.SetDirty(data);
                    }

                    EditorGUILayout.Space(20);

                    #endregion

                    serializedData.ApplyModifiedProperties(); // Apply all serialized property changes to the actual object.
                },

                keywords = new HashSet<string>
                {
                    "Language",
                    "Localization",
                    "Fonts",
                    "TMP",
                    "Translation",
                    "Culture",
                    "StreamingAssets"
                }
            };
        }

        #endregion
    }

    #endregion
}