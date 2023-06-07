using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Globalization;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Dropdown))]
public class LanguageDropdownOptions : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Dropdown targetDropdown; // Required Dropdown component.
    [SerializeField] private Text childTexts; // Dropdown's base text component.
    [SerializeField] private Text displayText; // Dropdown Display Text Component.
    public List<LanguageSelected> languageSelected; // List of language options.
    public float valueID = 0; // ID of the text to be displayed.
    [Space(10)]
    [Header("Text Settings")]
    [SerializeField] private int Font; // Which list font is being used.
    [SerializeField] private LanguageFontList fontListObject; // Font list.
    [SerializeField] private int Alignment; //What alignment should be used in the text.
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
    [SerializeField] private string jsonNameInUnity = "/Language/Editor/LanguageFileSave.json"; // JSON file name in Unity.
    [SerializeField] private string FolderNameInUnity = "/StreamingAssets/Language/"; // Folder name in Unity.
    [Space(10)]
    [SerializeField] private string jsonNameInBuild = "/LanguageFileSave.json"; // JSON file name in build.
    [SerializeField] private string FolderNameInBuild = "/StreamingAssets/Language/"; // Folder name in build.

    private int previousSelectedIndex = -1; // initialized with -1 to indicate that there is no previous selection.
    
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
            ReadFileConfig(selectedFile);
            ReadFile(selectedFile);

            // Selects the previously selected option.
            targetDropdown.value = previousSelectedIndex;
            targetDropdown.RefreshShownValue();
        }
        else
        {
            // If no previous saved selection, use default file.
            selectedFile = defaultFile;
            ReadFileConfig(defaultFile);
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

                // Update all items in the list that correspond to the ID.
                foreach (LanguageSelected p in languageSelected)
                {
                    if (p.value == id)
                    {
                        if (Reverse)
                        {
                            // Reverse the text if the Reverse variable is true.
                            char[] charArray = text.ToCharArray();
                            Array.Reverse(charArray);
                            text = new string(charArray);
                        }

                        p.SelectedText = text;
                    }
                }
            }
        }

        previousSelectedIndex = targetDropdown.value; // stores the index of the currently selected option.
        targetDropdown.ClearOptions(); // Clear dropdown options.

        // Adds an option for each language to the list of options.
        foreach (LanguageSelected languageOption in languageSelected)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData(); // Create an OptionData object for the new option.
            optionData.text = languageOption.SelectedText; // Sets the option text to the selected language option text.
            optionData.image = languageOption.selectedSprite; // Sets the option sprite to the selected language option sprite.
            targetDropdown.options.Add(optionData); // Adds the new option to the dropdown.
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
                if (id == valueID)
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
                            childTexts.font = fontListObject.FontList[Font - 1]; // Set the target text's font to the font at the specified index in the font list.
                            displayText.font = fontListObject.FontList[Font - 1]; // Set the target text's font to the font at the specified index in the font list.
                        }
                    }

                    int AliIndex = line.IndexOf("Ali:"); // Find the index of the "Ali:" keyword in the line.
                    if (AliIndex >= 0)
                    {
                        int semicolonIndex2 = line.IndexOf(";", AliIndex); // Find the index of the ";" character after the "Ali:" keyword.
                        string fontSizeString = line.Substring(AliIndex + 4, semicolonIndex2 - AliIndex - 4); // Extract the alignment value from the line.
                        Alignment = int.Parse(fontSizeString, CultureInfo.InvariantCulture); // Convert the alignment string to an integer.

                        // Set the target text's alignment based on the extracted alignment value.
                        if (Alignment == 1)
                        {
                            childTexts.alignment = TextAnchor.MiddleLeft;
                            displayText.alignment = TextAnchor.MiddleLeft;
                        }
                        else if (Alignment == 2)
                        {
                            childTexts.alignment = TextAnchor.MiddleCenter;
                            displayText.alignment = TextAnchor.MiddleCenter;
                        }
                        else if (Alignment == 3)
                        {
                            childTexts.alignment = TextAnchor.MiddleRight;
                            displayText.alignment = TextAnchor.MiddleRight;
                        }
                    }

                    int RevIndex = line.IndexOf("Rev:"); // Find the index of the "Rev:" keyword in the line.
                    if (AliIndex >= 0)
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

[System.Serializable]
public class LanguageSelected
{
    public string SelectedText; // Text to be placed.
    public Sprite selectedSprite; // Sprite selected for language option.
    public float value = 0; // ID of the text to be displayed.
}