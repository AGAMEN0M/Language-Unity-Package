using UnityEngine;
using System.IO;
using System.Reflection;

public class LanguageScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject Object; // Required Object component.
    [SerializeField] private string ScriptName; // Script name.
    [SerializeField] private string VariableName; // Script public variable name.
    public float value = 0; // ID of the text to be displayed.
    [Space(10)]
    [Header("Default language if there is no save")]
    [SerializeField] private string StandardFile = "Linguagem - [ENGLISH]"; // Default file for the language.
    [Space(10)]
    [Header("Automatic Information")]
    [SerializeField] private string selectedFile; // File selected for the language.
    [SerializeField] private string savePath; // Path to save the file.
    [SerializeField] private string defaultFile; // Default file for language if no selection.
    [Space(10)]
    [Header("Archives Location")]
    [SerializeField] private string jsonNameInUnity = "/Language/Editor/LanguageFileSave.json"; // JSON file name in Unity.
    [SerializeField] private string FolderNameInUnity = "/StreamingAssets/Language/"; // Folder name in Unity.
    [Space(10)]
    [SerializeField] private string jsonNameInBuild = "/LanguageFileSave.json"; // JSON file name in build.
    [SerializeField] private string FolderNameInBuild = "/StreamingAssets/Language/"; // Folder name in build.

    private void Start()
    {
        LanguageUpdate(); // Call the LanguageUpdate method when the script is started.
    }

    private void OnEnable()
    {
        LanguageUpdate(); // Call the LanguageUpdate method when the script is enabled.
    }

    public void LanguageUpdate()
    {
        // Path to save language file.
    #if UNITY_EDITOR
        savePath = Application.dataPath + jsonNameInUnity;
    #else
        savePath = Application.dataPath + jsonNameInBuild;
    #endif

        // Find default file.
    #if UNITY_EDITOR
        string path = Application.dataPath + FolderNameInUnity;
    #else
        string path = Application.dataPath + FolderNameInBuild;
    #endif
        string[] files = Directory.GetFiles(path, "*.txt");
        for (int i = 0; i < files.Length; i++)
        {
            string[] lines = File.ReadAllLines(files[i]);
            foreach (string line in lines)
            {
                if (line.StartsWith(StandardFile))
                {
                    defaultFile = files[i];
                    break;
                }
            }
        }

        // Load saved selection.
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            FileSave fileSaveData = JsonUtility.FromJson<FileSave>(json);
            selectedFile = fileSaveData.selectedFile;

            // Read selected file and update UI object text.
            ReadFile(selectedFile);
        }
        else
        {
            // If no previous saved selection, use default file.
            selectedFile = defaultFile;
            ReadFile(defaultFile);
        }
    }

    private void ReadFile(string filePath)
    {
        // Reads the contents of the selected file
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            // Checks if the line starts with "id:"
            if (line.StartsWith("id:"))
            {
                // Separates ID value and text.
                string[] parts = line.Split(';');
                float id = float.Parse(parts[0].Replace("id:", ""));
                string text = parts[1];

                // Get the text between the curly braces.
                int startIndex = text.IndexOf("{");
                int endIndex = text.IndexOf("}");
                if (startIndex >= 0 && endIndex >= 0)
                {
                    text = text.Substring(startIndex + 1, endIndex - startIndex - 1);
                }

                // If the variable value matches the ID, updates the specified variable in the specified script.
                if (value == id)
                {                    
                    Component myScript = Object.GetComponent(ScriptName); // Encontre o componente ScriptName no objeto.                    
                    FieldInfo field = myScript.GetType().GetField(VariableName); // Use a reflexão para atualizar o valor da variável VariableName.
                    field.SetValue(myScript, text);

                    break;
                }
            }
        }
    }
}