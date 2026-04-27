/*
 * ---------------------------------------------------------------------------
 * Description: Provides an automated and manual system for creating and maintaining 
 *              the LanguageSettingsData asset within a Unity project. The system 
 *              dynamically searches for the "Language Tool" folder regardless of its 
 *              location in the project hierarchy and ensures that a "Resources" 
 *              subfolder exists inside it. The asset is then created or retrieved 
 *              from this location.
 *              
 *              Includes a Unity menu option for manual creation with optional 
 *              overwrite confirmation, as well as an automatic initializer that 
 *              runs on editor startup to guarantee the asset always exists. If the 
 *              "Language Tool" folder cannot be found, the system safely falls back 
 *              to using the default "Assets/Resources" directory.
 *              
 *              This approach ensures flexibility in project structure while keeping 
 *              localization data consistently accessible and properly organized.
 * 
 * Author: Lucas Gomes Cecchini
 * Pseudonym: AGAMENOM
 * ---------------------------------------------------------------------------
*/

using UnityEditor;
using UnityEngine;
using System.IO;

namespace LanguageTools.Editor
{
    #region === Language Data Asset System ===

    /// <summary>
    /// Responsible for creating or retrieving the LanguageSettingsData asset.
    /// Handles folder discovery, creation, and overwrite logic.
    /// </summary>
    public static class LanguageDataAutoCreator
    {
        #region === Public API ===

        /// <summary>
        /// Creates or ensures the LanguageSettingsData asset exists inside the "Language Tool/Resources" folder.
        /// </summary>
        /// <param name="allowOverwritePrompt">If true, prompts before replacing an existing asset.</param>
        /// <returns>The created or existing LanguageSettingsData asset.</returns>
        public static LanguageSettingsData CreateOrGetData(bool allowOverwritePrompt)
        {
            // Try to find the base folder "Language Tool".
            string baseFolderPath = FindLanguageToolFolder();

            // Fallback if not found.
            if (string.IsNullOrEmpty(baseFolderPath))
            {
                Debug.LogWarning("Folder 'Language Tool' not found. Using Assets/Resources instead.");
                baseFolderPath = "Assets";
            }

            // Ensure Resources folder exists inside the base folder.
            string resourcesPath = $"{baseFolderPath}/Resources";

            if (!AssetDatabase.IsValidFolder(resourcesPath))
            {
                AssetDatabase.CreateFolder(baseFolderPath, "Resources");
            }

            // Final asset path.
            string assetPath = $"{resourcesPath}/Language Data.asset";

            // Try load existing asset.
            var existingAsset = AssetDatabase.LoadAssetAtPath<LanguageSettingsData>(assetPath);

            if (existingAsset != null)
            {
                if (!allowOverwritePrompt) return existingAsset;

                // Ask before overwrite.
                if (!EditorUtility.DisplayDialog(
                    "Replace File",
                    "There is already a 'Language Data'. Do you want to replace it?",
                    "Yes",
                    "No"))
                {
                    return existingAsset;
                }

                AssetDatabase.DeleteAsset(assetPath);
            }

            // Create new asset.
            var asset = ScriptableObject.CreateInstance<LanguageSettingsData>();

            AssetDatabase.CreateAsset(asset, assetPath);
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }

        /// <summary>
        /// Creates the LanguageSettingsData asset via Unity menu.
        /// </summary>
        [MenuItem("Assets/Create/Tools/Language Tool/Language Data")]
        public static void CreateLanguageDataAsset()
        {
            var asset = CreateOrGetData(true);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        #endregion

        #region === Internal Utilities ===

        /// <summary>
        /// Searches the project for the "Language Tool" folder.
        /// </summary>
        /// <returns>Folder path if found; otherwise null.</returns>
        private static string FindLanguageToolFolder()
        {
            var guids = AssetDatabase.FindAssets("Language Tool t:Folder");

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                // Ensure exact match.
                if (Path.GetFileName(path) == "Language Tool")
                {
                    return path;
                }
            }

            return null;
        }

        #endregion
    }

    #endregion

    #region === Automatic Initialization ===

    /// <summary>
    /// Ensures the LanguageSettingsData asset exists when the Unity Editor starts.
    /// </summary>
    [InitializeOnLoad]
    public static class LanguageDataStartup
    {
        /// <summary>
        /// Static constructor executed on editor load.
        /// </summary>
        static LanguageDataStartup()
        {
            EditorApplication.delayCall += () =>
            {
                LanguageDataAutoCreator.CreateOrGetData(false);
            };
        }
    }

    #endregion
}