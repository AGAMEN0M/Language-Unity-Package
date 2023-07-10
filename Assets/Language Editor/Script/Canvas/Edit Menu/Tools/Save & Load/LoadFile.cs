using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadFile : MonoBehaviour
{
    [Header("File Information")]
    [SerializeField] private string FileName = "ENGLISH"; // Selected file name.
    [SerializeField] private string Linguagem = "ENGLISH"; // Language of the selected file.
    [SerializeField] private string ComputerLanguage = "English (United States)"; // Computer language.
    [Space(10)]
    [SerializeField] private List<FileID> FileIDs; // List of file IDs.
    [SerializeField] private List<FileGroups> fileGroups; // List of file text groups.
    [SerializeField] private List<FileComments> fileComments; // List of file comments.
    [Space(20)]
    [Header("Components")]
    [SerializeField] private Dropdown loadFile; // Dropdown that shows the list of available files.
    [SerializeField] private InputField Name; // InputField that shows the name of the selected file.
    [SerializeField] private GameObject Object; // GameObject that shows text groups and comments.
    [Space(20)]
    [Header("Prefab Components")]
    [SerializeField] private GameObject PrefabLinguagem; // Prefab to create the element that shows the language of the file.
    [SerializeField] private GameObject PrefabIDGroups; // Prefab to create the elements that show the text groups.
    [SerializeField] private GameObject PrefabIDEditor; // Prefab to create the elements that allow you to edit the text groups.
    [SerializeField] private GameObject PrefabComments; // Prefab to create the elements that show the comments.
    [Space(5)]
    [Header("Archives Location")]
    [SerializeField] private List<string> fileNames; // List of available file names.
    #pragma warning disable CS0414
    [SerializeField] private string FileNameInUnity = "/StreamingAssets/Language/"; // File location in Unity Editor.
    [SerializeField] private string FileNameInBuild = "/StreamingAssets/Language/"; // File location in build.
    #pragma warning restore CS0414

    private int previousFileCount; // Variable to store the previous file count.

    private string GetFilePath()
    {
        // Defines the path to the file folder according to the platform.
    #if UNITY_EDITOR
        return Application.dataPath + FileNameInUnity;
    #else
        return Application.dataPath + FileNameInBuild;
    #endif
    }

    private void Start()
    {        
        string filePath = GetFilePath(); // Defines the path to the file folder according to the platform.

        fileNames.Clear(); // Clear the file list.

        // Populates the file name list with the files found in the folder.
        foreach (string file in Directory.GetFiles(filePath, "*.txt"))
        {
            fileNames.Add(Path.GetFileNameWithoutExtension(file));
        }

        // Adds the list of filenames to the dropdown.
        loadFile.ClearOptions();
        loadFile.AddOptions(fileNames); 
    }

    private void Update()
    {        
        string filePath = GetFilePath(); // Defines the path to the file folder according to the platform.

        int currentFileCount = Directory.GetFiles(filePath, "*.txt").Length; // Get the current file count.

        // Check if the file count has changed.
        if (currentFileCount != previousFileCount)
        {
            previousFileCount = currentFileCount; // Update the previous file count.
            Start(); // Call Start() to update the file list.
        }
    }

    // Loads the selected file and creates the corresponding UI elements.
    public void Load()
    {
        // Destroys existing UI elements.
        foreach (Transform child in Object.transform)
        {
            Destroy(child.gameObject);
        }

        // Get the name of the selected file in the Dropdown "loadFile".
        FileName = loadFile.options[loadFile.value].text;
        Name.text = FileName; // Fill the InputField "Name" with the name of the selected file.

        // Construct the file path to be loaded based on the selected file name.
    #if UNITY_EDITOR
        string filePath = Application.dataPath + FileNameInUnity + FileName + ".txt";
    #else
        string filePath = Application.dataPath + FileNameInBuild + FileName + ".txt";
    #endif

        string[] lines = File.ReadAllLines(filePath); // Read all lines from the selected file.

        // Extract the language and computer language from the file.
        Linguagem = lines[0].Substring(lines[0].IndexOf("[") + 1, lines[0].IndexOf("]") - lines[0].IndexOf("[") - 1);
        ComputerLanguage = lines[1];

        GameObject linguagemObj = Instantiate(PrefabLinguagem, Object.transform); // Create a language object using the "LanguagePrefab" prefab.
        OrganizeLinguagem organizeLinguagem = linguagemObj.GetComponent<OrganizeLinguagem>(); // Get the OrganizeLanguage component from the created language object.        
        organizeLinguagem.Linguagem.text = Linguagem; // Define the text of the "Language" field in the OrganizeLinguage component based on the language extracted from the file.
        organizeLinguagem.LanguageComputer.text = ComputerLanguage; // Set the text of the "Language Computer" field in the Organize Language component based on the computer language extracted from the file.

        fileGroups.Clear(); // Clear the list of filegroups.
        fileGroups = new List<FileGroups>(); // Create a new list of filegroups.

        // Loop through all the lines in the file.
        foreach (string line in lines)
        {
            // Check if the line follows the pattern [Text:Sample Number; {Sample Text} ids:Sample Text]
            if (line.StartsWith("[Text:"))
            {
                // Extract the text ID, text and IDs from the file and store them in a filegroup object.
                float textID = float.Parse(line.Substring(line.IndexOf(":") + 1, line.IndexOf(";") - line.IndexOf(":") - 1));
                string test = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1);
                string ids = line.Substring(line.IndexOf("ids:") + 4);

                FileGroups fileGroup = new FileGroups();
                fileGroup.TextID = textID;
                fileGroup.Test = test;
                fileGroup.IDs = ids;
                fileGroups.Add(fileGroup);
            }
        }

        // Create an ID group object for each file group in the file group list.
        foreach (FileGroups fileGroup in fileGroups)
        {
            GameObject idGroupObj = Instantiate(PrefabIDGroups, Object.transform);
            OrganizeIDGroups organizeIDGroups = idGroupObj.GetComponent<OrganizeIDGroups>();
            // Set the text of the "textID" field in the OrganizeIDGroups component to the text ID extracted from the file.
            organizeIDGroups.textID.text = fileGroup.TextID.ToString();
            organizeIDGroups.textIDs.text = fileGroup.IDs;
            organizeIDGroups.texts.text = fileGroup.Test;
        }

        fileComments.Clear(); // Clear the fileComments list.
        fileComments = new List<FileComments>(); // Creates a new instance of the fileComments list.

        // Loop through the lines of the file.
        foreach (string line in lines)
        {
            // Check if the line follows the pattern [Comments:Sample Number; {Sample Text}]
            if (line.StartsWith("[Comments:"))
            {
                float ID = float.Parse(line.Substring(line.IndexOf(":") + 1, line.IndexOf(";") - line.IndexOf(":") - 1)); // Gets the comment ID.
                string test = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1); // Gets the comment text.

                FileComments fileComment = new FileComments(); // Creates a new instance of the FileComments class.
                fileComment.comments = ID; // Assigns the comment ID to the FileComments object.
                fileComment.TestComments = test; // Assigns the comment text to the FileComments object.
                fileComments.Add(fileComment); // Adds the FileComments object to the fileComments list.
            }
        }

        // Loops through the fileComments list.
        foreach (FileComments fileComment in fileComments)
        {
            GameObject CommentObj = Instantiate(PrefabComments, Object.transform); // Creates a new instance of the PrefabComments object.
            OrganizeCommentsIDS organizeCommentsIDS = CommentObj.GetComponent<OrganizeCommentsIDS>(); // Gets the OrganizeCommentsIDS component of the CommentObj object.
            organizeCommentsIDS.textID.text = fileComment.comments.ToString(); // Defines the text of the Text component of the OrganizeCommentIDS instance.
            organizeCommentsIDS.Comment.text = fileComment.TestComments; // Defines the text of the Comment component of the OrganizeCommentIDS instance.
        }

        FileIDs.Clear(); // Clears the FileIDs list.
        FileIDs = new List<FileID>(); // Creates a new instance of the FileIDs list.

        // Loop through the lines of the file.
        foreach (string line in lines)
        {
            // Check if the line follows the pattern id:0;{Test} S:0; X:0; Y:0; Width:0; Height:0; Font:0; Ali:0; Rev:0;
            if (line.StartsWith("id:"))
            {
                FileID fileIDs = new FileID(); // Creates a new instance of the FileID class.

                float ID = float.Parse(line.Substring(line.IndexOf(":") + 1, line.IndexOf(";") - line.IndexOf(":") - 1)); // Gets the file ID.
                // Gets the text from the file.
                if (line.Contains("{"))
                {
                    string test = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1);
                    fileIDs.Test = test; // Assigns the text of the file to the FileID object.
                    fileIDs.TestWrite = true; // Sets TestWrite to true.
                }

                // Checks if the line contains information about the S position.
                if (line.Contains("S:"))
                {
                    int startIndex = line.IndexOf("S:") + 2;
                    int endIndex = line.IndexOf(";", startIndex);

                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        float size = float.Parse(line.Substring(startIndex, endIndex - startIndex)); // Gets the font size.
                        fileIDs.Size = size; // Assigns the font size to the FileID object.
                        fileIDs.SizeWrite = true; // Sets SizeWrite to true.
                    }
                }

                // Checks if the line contains information about the X position.
                if (line.Contains("X:"))
                {
                    // Gets the index of the start of the X position and the end of the X position value in the row.
                    int startIndex = line.IndexOf("X:") + 2;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If the indexes are valid.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        // Extracts the value from the line's X position and stores it in the corresponding FileID object.
                        float posX = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                        fileIDs.PosX = posX;
                        fileIDs.PosXWrite = true;
                    }
                }

                // Checks if the line contains information about the Y position.
                if (line.Contains("Y:"))
                {
                    // Gets the index of the start of the Y position and the end of the Y position value in the line.
                    int startIndex = line.IndexOf("Y:") + 2;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If the indexes are valid.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        // Extracts the Y position value from the line and stores it in the corresponding FileID object.
                        float posY = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                        fileIDs.PosY = posY;
                        fileIDs.PosYWrite = true;
                    }
                }

                // Checks whether the line contains width information.
                if (line.Contains("Width:"))
                {
                    // Gets the index of the beginning of the width and the end of the width value in the line.
                    int startIndex = line.IndexOf("Width:") + 6;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If the indexes are valid.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        // Extracts the line width value and stores it in the corresponding FileID object.
                        float width = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                        fileIDs.Width = width;
                        fileIDs.WidthWrite = true;
                    }
                }

                // Checks whether the row contains height information.
                if (line.Contains("Height:"))
                {
                    // Gets the index of the start height and end height value in the row.
                    int startIndex = line.IndexOf("Height:") + 7;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If the indexes are valid.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        // Extracts the row height value and stores it in the corresponding FileID object.
                        float height = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                        fileIDs.Height = height;
                        fileIDs.HeightWrite = true;
                    }
                }

                // Checks whether the line contains font information.
                if (line.Contains("Font:"))
                {
                    // Gets the index of the beginning of the font and the end of the font value in the row.
                    int startIndex = line.IndexOf("Font:") + 5;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If the indexes are valid.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        // Extracts the source value from the row and stores it in the corresponding FileID object.
                        float font = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                        fileIDs.Font = font;
                        fileIDs.FontWrite = true;
                    }
                }

                // Extracts line alignment information, if any.
                if (line.Contains("Ali:"))
                {
                    int startIndex = line.IndexOf("Ali:") + 4;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If start and end index are valid, extract alignment information.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        float font = float.Parse(line.Substring(startIndex, endIndex - startIndex));

                        // Stores the alignment information in the corresponding FileID object.
                        fileIDs.Alignment = font;
                        fileIDs.AlignmentWrite = true;
                    }
                }

                // Extracts the reversal information from the row, if any.
                if (line.Contains("Rev:"))
                {
                    int startIndex = line.IndexOf("Rev:") + 4;
                    int endIndex = line.IndexOf(";", startIndex);

                    // If the start and end index are valid, extract the reversal information.
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        float Rev = float.Parse(line.Substring(startIndex, endIndex - startIndex));

                        // Stores the rollback information in the corresponding FileID object.
                        fileIDs.Reverse = Rev;
                        fileIDs.ReverseWrite = true;
                    }
                }

                // Extracts the row ID and stores it in the corresponding FileID object.
                fileIDs.id = ID;                
                FileIDs.Add(fileIDs);
            }
        }

        // Loop through the FileID objects created from the file and set the corresponding GameObject object.
        foreach (FileID fileComment in FileIDs)
        {
            GameObject CommentObj = Instantiate(PrefabIDEditor, Object.transform);
            OrganizeIDS organizeIDS = CommentObj.GetComponent<OrganizeIDS>();
            organizeIDS.TextID = fileComment.id;

            // Sets the comment text, if any.
            organizeIDS.InteractableText = fileComment.TestWrite;
            if (fileComment.TestWrite == true)
            {
                organizeIDS.text.text = fileComment.Test;
            }

            // Sets the font size of the comment, if any.
            organizeIDS.InteractableFontSize = fileComment.SizeWrite;
            if (fileComment.SizeWrite == true)
            {
                organizeIDS.FontSize.text = fileComment.Size.ToString();
            }

            // Sets the X position of the comment, if any.
            organizeIDS.InteractablePosX = fileComment.PosXWrite;
            if (fileComment.PosXWrite == true)
            {
                organizeIDS.PosX.text = fileComment.PosX.ToString();
            }

            // Sets the Y position of the comment, if any.
            organizeIDS.InteractablePosY = fileComment.PosYWrite;
            if (fileComment.PosYWrite == true)
            {
                organizeIDS.PosY.text = fileComment.PosY.ToString();
            }

            // Sets the width of the comment, if any.
            organizeIDS.InteractableWidth = fileComment.WidthWrite;
            if (fileComment.WidthWrite == true)
            {
                organizeIDS.Width.text = fileComment.Width.ToString();
            }

            // Sets the height of the comment, if any.
            organizeIDS.InteractableHeight = fileComment.HeightWrite;
            if (fileComment.HeightWrite == true)
            {
                organizeIDS.Height.text = fileComment.Height.ToString();
            }

            // Sets the comment font, if any.
            organizeIDS.InteractableFont = fileComment.FontWrite;
            if (fileComment.FontWrite == true)
            {
                organizeIDS.Font.text = fileComment.Font.ToString();
            }

            // Sets the interactive alignment capability according to the option selected in the configuration file.
            organizeIDS.InteractableAlignment = fileComment.AlignmentWrite;
            if (fileComment.AlignmentWrite == true)
            {
                organizeIDS.AlignmentValue = Convert.ToInt32(fileComment.Alignment);
            }

            // Sets the interactive inversion capability according to the option selected in the configuration file.
            organizeIDS.InteractableReverse = fileComment.ReverseWrite;
            if (fileComment.ReverseWrite == true)
            {
                organizeIDS.ReverseValue = Convert.ToInt32(fileComment.Reverse);
            }
        }
    }
}

[System.Serializable]
public class FileID
{
    [Header("Information")]
    public float id; // Text ID.
    [Space(5)]
    public bool TestWrite; // if there is.
    public string Test; // Text that the ID represents.
    public bool SizeWrite; // if there is.
    public float Size; // Text that the ID represents.
    public bool PosXWrite; // if there is.
    public float PosX;  // X position of the text box.
    public bool PosYWrite; // if there is.
    public float PosY; // Y position of the text box.
    public bool WidthWrite; // if there is.
    public float Width; // Text box width.
    public bool HeightWrite; // if there is.
    public float Height; // Height of the text box.
    public bool FontWrite; // if there is.
    public float Font; // Index of the font used.
    public bool AlignmentWrite; // if there is.
    public float Alignment; // Text alignment.
    public bool ReverseWrite; // if there is.
    public float Reverse; // Whether the text should be inverted.
}

[System.Serializable]
public class FileGroups
{
    [Header("Information")]
    public float TextID; // Unique ID for each text group.
    [Space(5)]
    public string IDs; // Sample text for this text group.
    public string Test; // Semicolon-separated IDs associated with this text group.
}

[System.Serializable]
public class FileComments
{
    [Header("Information")]
    public float comments; // Comment ID.
    [Space(5)]
    public string TestComments; // Comment text.
}