using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MeuEditorAddComponent : EditorWindow
{
    // Adds the "SaveLegacyInformation" component to selected objects.
    [MenuItem("Component/Language/Legacy/Save Legacy Information")]
    public static void AddMeusScriptsComponent1()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<SaveLegacyInformation>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Add the "Language Create File" component to the selected objects.
    [MenuItem("Component/Language/Language Create File")]
    public static void AddMeusScriptsComponent2()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageCreateFile>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Add the "Language Script" component to the selected objects.
    [MenuItem("Component/Language/Language Script")]
    public static void AddMeusScriptsComponent3()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent< LanguageScript> ();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Add the "Language Text Mesh" component to the selected objects.
    [MenuItem("Component/Language/Legacy/Language Text Mesh")]
    public static void AddMeusScriptsComponent4()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageTextMesh>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Add the "Language Text Input Field" component to the selected objects.
    [MenuItem("Component/Language/Legacy/Language Text Input Field")]
    public static void AddMeusScriptsComponent5()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageTextInputField>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Add the "Language Text" component to the selected objects.
    [MenuItem("Component/Language/Legacy/Language Text")]
    public static void AddMeusScriptsComponent6()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageText>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Adds the "LanguageFontList" component to selected objects.
    [MenuItem("Component/Language/Legacy/Language Font List")]
    public static void AddMeusScriptsComponent7()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageFontList>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Adds the "LanguageDropdownOptions" component to selected objects.
    [MenuItem("Component/Language/Legacy/Language Dropdown Options")]
    public static void AddMeusScriptsComponent8()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageDropdownOptions>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // Add "Language Dropdown" component to selected objects.
    [MenuItem("Component/Language/Legacy/Language Dropdown")]
    public static void AddMeusScriptsComponent9()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageDropdownOptions>();
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}