#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class LanguageSettingsWindow : EditorWindow
{
    public Vector2 scrollPosition = Vector2.zero; // Defines the scroll position in the window.

    // Defines the menu that will be displayed in the editor window.
    [MenuItem("Window/Language/Language Settings")]
    public static void ShowWindow()
    {
        GetWindow<LanguageSettingsWindow>("Language Settings"); // Opens the "Language Settings" window in Unity Editor.
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition); // Creates a scrollable area for the window's GUI.

        GUILayout.Label("Reset all Language settings"); // Label to show the text "Reset all Language settings".

        GUILayout.Space(5); // Vertical spacing.

        // Button.
        if (GUILayout.Button("Reset"))
        {
            // Displays a confirmation dialog with the custom message.
            bool resetConfirmed = EditorUtility.DisplayDialog("Reset Confirmation", "Are you sure you want to reset?\nThis will undo any changes you have made to the Language system.", "Yes", "No");
            if (resetConfirmed)
            {
                // Gets a reference to the "LanguageSaveEditorWindow" window and closes it.
                LanguageSaveEditorWindow editorWindow = EditorWindow.GetWindow<LanguageSaveEditorWindow>("Language Save Editor");
                editorWindow.Close();

                DeleteDataFile(); // Calls the method to delete the language data file.
            }
        }

        GUILayout.EndScrollView(); // Ends the scroll area.
    }

    private void DeleteDataFile()
    {
        string filePath = "ProjectSettings/LanguageData.txt";

        if (File.Exists(filePath))
        {
            File.Delete(filePath); // Deletes the language data file.
            Debug.Log("Data File Deleted Successfully.");
        }
    }
}
#endif