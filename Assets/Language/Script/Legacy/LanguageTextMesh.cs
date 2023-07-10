using UnityEngine;
using System.IO;
using System.Globalization;
using System;

[RequireComponent(typeof(TextMesh))]
public class LanguageTextMesh : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMesh targetTextMesh; // Required TextMesh component.
    public float value = 0; // ID of the text to be displayed.
    [Space(10)]
    [Header("Text Settings")]
    [SerializeField] private int FontSize; // Text font size.
    [SerializeField] private int Font; // Which list font is being used.
    [SerializeField] private LanguageFontList fontListObject; // Font list.
    [SerializeField] private bool Reverse = false; // Invert the text.
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
    #pragma warning restore CS0414

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
            ReadFileSize(selectedFile);
            ReadFileConfig(defaultFile);
        }
        else
        {
            // If no previous saved selection, use default file.
            selectedFile = defaultFile;
            ReadFile(defaultFile);
            ReadFileSize(defaultFile);
            ReadFileConfig(defaultFile);
        }

        Size(); // Update the size, position and dimensions of the text.
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

                // If the variable value matches the ID, updates the UI object text.
                if (value == id)
                {
                    if (Reverse)
                    {
                        // Reverse the text if the Reverse variable is true.
                        char[] charArray = text.ToCharArray();
                        Array.Reverse(charArray);
                        text = new string(charArray);
                    }

                    targetTextMesh.text = text;
                    break;
                }
            }
        }
    }

    // Reads the file to find the correct size, position and dimensions of the text to be displayed.
    private void ReadFileSize(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            int idIndex = line.IndexOf("id:");
            if (idIndex >= 0)
            {
                int semicolonIndex = line.IndexOf(";", idIndex);
                int id = int.Parse(line.Substring(idIndex + 3, semicolonIndex - idIndex - 3));

                if (id == value)
                {
                    // Find the font size value from the line in the file.
                    int sIndex = line.IndexOf("S:");
                    if (sIndex >= 0)
                    {
                        int semicolonIndex2 = line.IndexOf(";", sIndex);
                        string fontSizeString = line.Substring(sIndex + 2, semicolonIndex2 - sIndex - 2);
                        FontSize = int.Parse(fontSizeString, CultureInfo.InvariantCulture);
                    }
                }
            }
        }
    }

    // Adjusts the size, position and dimensions of the text based on values read from the file.
    private void Size()
    {
        // Change the font size if a valid value is found in the file.
        if (FontSize != 0)
        {
            targetTextMesh.characterSize = FontSize;
        }
    }

    // Method to read configuration file located in the given file path.
    private void ReadFileConfig(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath); // Read all lines from the file.
        foreach (string line in lines)
        {
            int idIndex = line.IndexOf("id:"); // Find the index of the "id:" keyword in the line.

            // If "id:" is found in the line.
            if (idIndex >= 0)
            {
                int semicolonIndex = line.IndexOf(";", idIndex); // Find the index of the ";" character after the "id:" keyword.
                int id = int.Parse(line.Substring(idIndex + 3, semicolonIndex - idIndex - 3)); // Extract the ID from the line.

                // If the extracted ID matches the target value.
                if (id == value)
                {
                    int FontIndex = line.IndexOf("Font:"); // Find the index of the "Font:" keyword in the line.
                    if (FontIndex >= 0)
                    {
                        int semicolonIndex2 = line.IndexOf(";", FontIndex); // Find the index of the ";" character after the "Font:" keyword.
                        string fontSizeString = line.Substring(FontIndex + 5, semicolonIndex2 - FontIndex - 5); // Extract the font size from the line.
                        Font = int.Parse(fontSizeString, CultureInfo.InvariantCulture); // Convert the font size string to an integer.

                        // If the font list object is not null and the font size is within the range of available fonts.
                        if (fontListObject != null && Font > 0 && Font <= fontListObject.FontList.Count)
                        {
                            targetTextMesh.font = fontListObject.FontList[Font - 1]; // Set the target text's font to the font at the specified index in the font list.
                        }
                    }

                    int RevIndex = line.IndexOf("Rev:"); // Find the index of the "Rev:" keyword in the line.
                    if (RevIndex >= 0)
                    {
                        int value;
                        int semicolonIndex2 = line.IndexOf(";", RevIndex); // Find the index of the ";" character after the "Rev:" keyword.
                        string fontSizeString = line.Substring(RevIndex + 4, semicolonIndex2 - RevIndex - 4); // Extract the reverse value from the line.
                        value = int.Parse(fontSizeString, CultureInfo.InvariantCulture); // Convert the reverse string to an integer.

                        // Set the value of the Reverse property based on the integer value.
                        if (value == 1)
                        {
                            Reverse = false;
                        }

                        if (value == 2)
                        {
                            Reverse = true;
                        }
                    }
                }
            }
        }
    }
}