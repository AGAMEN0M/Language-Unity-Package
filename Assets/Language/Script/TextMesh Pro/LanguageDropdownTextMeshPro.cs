using UnityEngine;
using TMPro;
using System.IO;
using System.Globalization;
using System.Linq;

[RequireComponent(typeof(TMP_Dropdown))]
public class LanguageDropdownTextMeshPro : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TMP_Dropdown languageDropdown; // Required Dropdown component.
    [Space(10)]
    [Header("Default language if there is no save")]
    [SerializeField] private string LanguageName = "ENGLISH"; // Default file for the language.
    [Space(10)]
    [Header("Automatic Information")]
    [SerializeField] private string selectedLanguage; // File selected for the language.
    [SerializeField] private string systemLanguage; // operating system language.
    [SerializeField] private string savePath; // Path to save the file.
    [SerializeField] private string selectedFile; // Default file for language if no selection.
    [SerializeField] private string fileSavePath; // Selected file for the language of the other scripts.
    [Space(10)]
    [Header("Archives Location")]
    [SerializeField] private string jsonNameInUnity = "/Language/Editor/LanguageFileSave.json"; // JSON file name in Unity.
    [SerializeField] private string jsonSaveNameInUnity = "/Language/Editor/LanguageSave.json"; // JSON file name in Unity.
    [SerializeField] private string FolderNameInUnity = "/StreamingAssets/Language/"; // Folder name in Unity.
    [Space(10)]
    [SerializeField] private string jsonNameInBuild = "/LanguageFileSave.json"; // JSON file name in build.
    [SerializeField] private string jsonSaveNameInBuild = "/LanguageSave.json"; // JSON file name in build.
    [SerializeField] private string FolderNameInBuild = "/StreamingAssets/Language/"; // Folder name in build.

    private void Start()
    {
        // Get the system language using CultureInfo.
        CultureInfo cultureInfo = CultureInfo.InstalledUICulture;
        systemLanguage = cultureInfo.DisplayName;

        languageDropdown.ClearOptions(); // Clear Dropdown Options.

        // Path to save language file.
    #if UNITY_EDITOR
        savePath = Application.dataPath + jsonSaveNameInUnity;
        fileSavePath = Application.dataPath + jsonNameInUnity;
        string path = Application.dataPath + FolderNameInUnity;
    #else
        savePath = Application.dataPath + jsonSaveNameInBuild;
        fileSavePath = Application.dataPath + jsonNameInBuild;
        string path = Application.dataPath + FolderNameInBuild;
    #endif

        string[] files = Directory.GetFiles(path, "*.txt"); // Get all .txt files in the language folder and iterate over them.

        // If there is no saved language file or default language file, iterates over each file to determine the language.
        if (!File.Exists(savePath) && !File.Exists(fileSavePath))
        {
            foreach (string buildFilePath in files)
            {
                foreach (string unityFilePath in files)
                {
                    // Get the language name from the first line of the file and the system language from the second line.
                    string[] unityFileLines = File.ReadAllLines(unityFilePath);
                    string unityFileLine2 = unityFileLines.Length >= 2 ? unityFileLines[1] : "";
                    string firstLine = File.ReadLines(unityFilePath).First();
                    string contentInBrackets = firstLine.Replace("Linguagem - [", "").Replace("]", "");

                    // If the system language matches the language in the file, save the language selection and update the dropdown.
                    if (systemLanguage == unityFileLine2)
                    {
                        selectedLanguage = contentInBrackets;
                        selectedFile = unityFilePath;

                        LanguageSave saveData = new LanguageSave();
                        saveData.selectedLanguage = selectedLanguage;
                        string json = JsonUtility.ToJson(saveData);
                        File.WriteAllText(savePath, json);

                        FileSave fileSaveData = new FileSave();
                        fileSaveData.selectedFile = selectedFile;
                        json = JsonUtility.ToJson(fileSaveData);
                        File.WriteAllText(fileSavePath, json);

                        LanguageUpdate();
                    }
                }
            }
        }

        // Iterate over the language files again to get the language names for the dropdown options.
        int defaultLanguageIndex = -1;
        for (int i = 0; i < files.Length; i++)
        {
            string[] lines = File.ReadAllLines(files[i]);
            foreach (string line in lines)
            {
                if (line.StartsWith("Linguagem - ["))
                {
                    string language = line.Replace("Linguagem - [", "").Replace("]", "");
                    languageDropdown.options.Add(new TMP_Dropdown.OptionData(language));

                    //Save English language index.
                    if (language == LanguageName)
                    {
                        defaultLanguageIndex = i;
                    }
                }
            }
        }

        // Load saved selection.
        if (File.Exists(savePath) && File.Exists(fileSavePath))
        {
            string json = File.ReadAllText(savePath);
            LanguageSaveTextMeshPro saveData = JsonUtility.FromJson<LanguageSaveTextMeshPro>(json);
            selectedLanguage = saveData.selectedLanguage;

            json = File.ReadAllText(fileSavePath);
            FileSaveTextMeshPro fileSaveData = JsonUtility.FromJson<FileSaveTextMeshPro>(json);
            selectedFile = fileSaveData.selectedFile;

            for (int i = 0; i < languageDropdown.options.Count; i++)
            {
                if (languageDropdown.options[i].text == selectedLanguage)
                {
                    languageDropdown.value = i;
                    languageDropdown.RefreshShownValue();
                    break;
                }
            }
        }
        else
        {
            // If no previous saved selection, use English language as default.
            if (defaultLanguageIndex >= 0)
            {
                languageDropdown.value = defaultLanguageIndex;
                languageDropdown.RefreshShownValue();
                selectedLanguage = LanguageName;
                selectedFile = files[defaultLanguageIndex];
                OnLanguageChanged();
                OnFileChanged();
            }
        }
        languageDropdown.onValueChanged.AddListener(delegate {OnLanguageChanged(); } );
    }

    public void OnLanguageChanged()
    {
        // Save the selected language to a JSON file.
        selectedLanguage = languageDropdown.options[languageDropdown.value].text;
        LanguageSave saveData = new LanguageSave();
        saveData.selectedLanguage = selectedLanguage;
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);

        // Update selected file.
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
                if (line.StartsWith("Linguagem - ["))
                {
                    string language = line.Replace("Linguagem - [", "").Replace("]", "");
                    if (language == selectedLanguage)
                    {
                        selectedFile = files[i];
                        break;
                    }
                }
            }
        }
        OnFileChanged();
        LanguageUpdate();
    }

    private void LanguageUpdate()
    {
        // Finds all objects in the scene that have a Language component.
        LanguageScript[] languageScript = FindObjectsOfType<LanguageScript>();
        LanguageCreateFile[] languageCreateFile = FindObjectsOfType<LanguageCreateFile>();
        LanguageTextMeshTextMeshPro[] languageTextMeshTextMeshPro = FindObjectsOfType<LanguageTextMeshTextMeshPro>();
        LanguageTextInputFieldTextMeshPro[] languageTextInputFieldTextMeshPro = FindObjectsOfType<LanguageTextInputFieldTextMeshPro>();
        LanguageTextTextMeshPro[] languageTextTextMeshPro = FindObjectsOfType<LanguageTextTextMeshPro>();
        LanguageDropdownOptionsTextMeshPro[] languageDropdownOptionsTextMeshPro = FindObjectsOfType<LanguageDropdownOptionsTextMeshPro>();
        /**/
        // If using the old text system.
        LanguageText[] languageTexts = FindObjectsOfType<LanguageText>();
        LanguageTextInputField[] languageTextInputField = FindObjectsOfType<LanguageTextInputField>();
        LanguageTextMesh[] languageTextMesh = FindObjectsOfType<LanguageTextMesh>();
        LanguageDropdownOptions[] languageDropdownOptions = FindObjectsOfType<LanguageDropdownOptions>();
        /**/

        // For each object found, it calls the LanguageUpdate() method.
        foreach (LanguageScript language in languageScript)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageCreateFile language in languageCreateFile)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageTextMeshTextMeshPro language in languageTextMeshTextMeshPro)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageTextInputFieldTextMeshPro language in languageTextInputFieldTextMeshPro)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageTextTextMeshPro language in languageTextTextMeshPro)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageDropdownOptionsTextMeshPro language in languageDropdownOptionsTextMeshPro)
        {
            language.LanguageUpdate();
        }
        /**/
        // If using the old text system.
        foreach (LanguageText language in languageTexts)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageTextInputField language in languageTextInputField)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageTextMesh language in languageTextMesh)
        {
            language.LanguageUpdate();
        }

        foreach (LanguageDropdownOptions language in languageDropdownOptions)
        {
            language.LanguageUpdate();
        }
        /**/
    }

    public void OnFileChanged()
    {
        // Save the selected file to a JSON file.
        FileSaveTextMeshPro fileSaveData = new FileSaveTextMeshPro();
        fileSaveData.selectedFile = selectedFile;
        string json = JsonUtility.ToJson(fileSaveData);
        File.WriteAllText(fileSavePath, json);
    }
}

public class LanguageSaveTextMeshPro
{
    public string selectedLanguage;
}

public class FileSaveTextMeshPro
{
    public string selectedFile;
}