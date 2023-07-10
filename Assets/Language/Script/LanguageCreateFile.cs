using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LanguageCreateFile : MonoBehaviour
{
    [Header("Settings")]
    public List<Language> language;
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
    #pragma warning disable CS0414
    [SerializeField] private string jsonNameInUnity = "/Language/Editor/LanguageFileSave.json"; // JSON file name in Unity.
    [SerializeField] private string FolderNameInUnity = "/StreamingAssets/Language/"; // Folder name in Unity.
    [Space(10)]
    [SerializeField] private string jsonNameInBuild = "/LanguageFileSave.json"; // JSON file name in build.
    [SerializeField] private string FolderNameInBuild = "/StreamingAssets/Language/"; // Folder name in build.
    [Space(15)]
    // Variables to define folder and file name, extension and content.
    [Header("File Creator Settings")]
    [SerializeField] private string folderNameInUnity = "Editor/Files";
    [SerializeField] private string folderNameInBuild = "Files";
    #pragma warning restore CS0414
    [Space(10)]
    [SerializeField] private string FileName = "Test File";
    [SerializeField] private string FileExtension = ".txt";
    [SerializeField] private List<string> FileLines;

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

            // Read selected file and update SelectedText variable.
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
        // Reads the contents of the selected file.
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            // Checks if the line starts with "id:"
            if (line.StartsWith("id:"))
            {
                // Separates ID value and text
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

                // If the variable value matches the ID, updates SelectedText variable.
                for (int i = 0; i < language.Count; i++)
                {
                    Language p = language[i];
                    if (p.value == id)
                    {
                        p.SelectedText = text;
                        language[i] = p;
                        break;
                    }
                }
            }
        }
        // Create the file.
        CreateFile();
    }

    private void CreateFile()
    {
        // Sets the folder path depending on whether running in the Editor or the final build.
    #if UNITY_EDITOR
        string folderPath = Path.Combine(Application.dataPath, folderNameInUnity);
    #else
        string folderPath = Path.Combine(Application.dataPath, folderNameInBuild);
    #endif

        // Creates the folder and full file path.
        Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, FileName + FileExtension);

        // Replace the selected line in FileLines with the corresponding SelectedText value.
        foreach (Language lang in language)
        {
            int index = Mathf.RoundToInt(lang.Line);
            if (index >= 0 && index < FileLines.Count)
            {
                FileLines[index] = lang.SelectedText;
            }
        }

        // Write the contents of the file.
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (string line in FileLines)
            {
                writer.WriteLine(line);
            }
        }
    }
}

[System.Serializable]
public class Language
{
    public string SelectedText; // Selected text.
    public float value = 0; // ID of the text to be displayed.
    public float Line; // Line to be replaced.
}