using UnityEngine;
using UnityEditor;

public class MeuEditorAddComponentTextMeshPro : MonoBehaviour
{
    // Menu item to add LanguageDropdownOptionsTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Language Dropdown Options TextMesh Pro")]
    public static void AddMeusScriptsComponent1()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            // Loop through selected game objects and add LanguageDropdownOptionsTextMeshPro component to each one.
            obj.AddComponent<LanguageDropdownOptionsTextMeshPro>();
        }
    }

    // Menu item to add LanguageDropdownTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Language Dropdown TextMesh Pro")]
    public static void AddMeusScriptsComponent2()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through selected game objects and add LanguageDropdownTextMeshPro component to each one.
        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageDropdownTextMeshPro>();
        }
    }

    // Menu item to add LanguageFontListTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Language Font List TextMesh Pro")]
    public static void AddMeusScriptsComponent3()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through selected game objects and add LanguageFontListTextMeshPro component to each one.
        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageFontListTextMeshPro>();
        }
    }

    // Menu item to add LanguageTextInputFieldTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Language Text Input Field TextMesh Pro")]
    public static void AddMeusScriptsComponent4()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through selected game objects and add LanguageTextInputFieldTextMeshPro component to each one.
        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageTextInputFieldTextMeshPro>();
        }
    }

    // Menu item to add LanguageTextMeshTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Language Text Mesh TextMesh Pro")]
    public static void AddMeusScriptsComponent5()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through selected game objects and add LanguageTextMeshTextMeshPro component to each one.
        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageTextMeshTextMeshPro>();
        }
    }

    // Menu item to add LanguageTextTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Language Text TextMesh Pro")]
    public static void AddMeusScriptsComponent6()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through selected game objects and add LanguageTextTextMeshPro component to each one.
        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<LanguageTextTextMeshPro>();
        }
    }

    // Menu item to add SaveLegacyInformationTextMeshPro component to selected game objects.
    [MenuItem("Component/Language/TextMesh Pro/Save TextMesh Pro Information")]
    public static void AddMeusScriptsComponent7()
    {
        // Get array of selected game objects.
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through selected game objects and add SaveLegacyInformationTextMeshPro component to each one.
        foreach (GameObject obj in selectedObjects)
        {
            obj.AddComponent<SaveTextMeshProInformation>();
        }
    }
}