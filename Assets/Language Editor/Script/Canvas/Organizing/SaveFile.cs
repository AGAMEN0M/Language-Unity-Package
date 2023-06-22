using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveFile : MonoBehaviour
{
    [Header("Save Information")]
    [SerializeField] private string FileName = "ENGLISH"; // Save file name.
    [SerializeField] private string Linguagem = "Linguagem - [ENGLISH]"; // Saved language.
    [SerializeField] private string ComputerLanguage = "English (United States)"; // Computer language.
    [Space(10)]
    [SerializeField] private List<ID> IDs; // ID list.
    [SerializeField] private List<Groups> groups; // List of groups.
    [SerializeField] private List<Comments> comments; // Comments list.
    [Space(20)]
    [Header("Components")]
    [SerializeField] private InputField Name; // Name input field.
    [Space(5)]
    [Header("Archives Location")]
    [SerializeField] private string DateTime = "00-00-0000 00-00-00"; // Save date and time.
    [SerializeField] private string FileNameInUnity = "/StreamingAssets/Language/Saves/"; // Save path in Unity.
    [SerializeField] private string FileNameInBuild = "/StreamingAssets/Language/Saves/"; // Save path in build.

    // Updates the save date and time.
    private void FixedUpdate()
    {
        DateTime = System.DateTime.Now.ToString();
        DateTime = DateTime.Replace("/", "-").Replace(":", "-");
    }

    // Save the file data.
    public void save()
    {
        FileName = Name.text;

        OrganizeLinguagem organizeScript = FindObjectOfType<OrganizeLinguagem>();
        Linguagem = string.Format("Linguagem - [" + organizeScript.Linguagem.text + "]");
        ComputerLanguage = organizeScript.LanguageComputer.text;

        CreateIDsFromAllOrganize();
        CreateGroupsFromAllOrganize();
        CreateCommentsFromAllOrganize();

        saveFile();
    }

    // Save the file.
    private void saveFile()
    {
        SortByID(); // Organize all items.

        // Checks if the application is running in the editor or in a final build and sets the file path accordingly.
#if UNITY_EDITOR
        string Path = Application.dataPath + FileNameInUnity;
    #else
        string Path = Application.dataPath + FileNameInBuild;
    #endif

        // Creates the fully qualified file name based on the name entered in InputField 'Name' and the current date and time.
        string filePath = Path + FileName + "=" + DateTime + ".txt";

        // Creates a StringBuilder to build the string that will be written to the file.
        StringBuilder sb = new StringBuilder();

        // Writes the language of the file and the language of the computer.
        sb.AppendLine(Linguagem);
        sb.AppendLine(ComputerLanguage);

        // Writes the data for each ID to the file.
        foreach (ID id in IDs)
        {
            sb.Append("id:" + id.id + "; ");

            if (id.TestWrite)
            {
                sb.Append("{" + id.Test + "} ");
            }

            if (id.SizeWrite)
            {
                sb.Append("S:" + id.Size.ToString("F0") + "; ");
            }

            if (id.PosXWrite)
            {
                sb.Append("X:" + id.PosX + "; ");
            }

            if (id.PosYWrite)
            {
                sb.Append("Y:" + id.PosY + "; ");
            }

            if (id.WidthWrite)
            {
                sb.Append("Width:" + id.Width + "; ");
            }

            if (id.HeightWrite)
            {
                sb.Append("Height:" + id.Height + "; ");
            }

            if (id.FontWrite)
            {
                sb.Append("Font:" + id.Font + "; ");
            }

            if (id.AlignmentWrite)
            {
                sb.Append("Ali:" + id.Alignment + "; ");
            }

            if (id.ReverseWrite)
            {
                sb.Append("Rev:" + id.Reverse + ";");
            }

            sb.AppendLine(); // Adds a new line after writing the data for each ID.
            string resultado = sb.ToString(); // Converts the contents of the StringBuilder to a string.

        }

        // Writes the data for each group to the file.
        foreach (Groups groups in groups)
        {
            sb.AppendLine("[Text:" + groups.TextID + "; {" + groups.Test + "} ids:" + groups.IDs + "]");
        }

        // Writes the data for each comment to the file.
        foreach (Comments comment in comments)
        {
            sb.AppendLine("[Comments:" + comment.comments + "; {" + comment.TestComments + "}]");
        }

        File.WriteAllText(filePath, sb.ToString()); // Writes the final string to the file.
        Debug.Log("Saved File: " + filePath); // Prints on the console the message indicating the path and name of the saved file.
    }

    private void CreateIDsFromAllOrganize()
    {
        IDs.Clear(); // Clears the list of IDs so as not to duplicate existing values.

        OrganizeIDS[] organizeScripts = FindObjectsOfType<OrganizeIDS>(); // Gets all OrganizeIDS components present in the scene.

        foreach (OrganizeIDS organize in organizeScripts) // Iterates over each OrganizeIDS found in the scene.
        {
            ID id = MapOrganizeToID(organize); // Maps the OrganizeIDS component to an ID object.
            IDs.Add(id); // Adds the ID object to the list of IDs.
        }
    }

    private ID MapOrganizeToID(OrganizeIDS organize)
    {
        ID id = new ID(); // Creates a new ID object.

        // Maps the OrganizeIDS object information to the ID object.
        id.id = organize.TextID;
        id.TestWrite = organize.InteractableText;
        id.Test = organize.text.text;
        id.SizeWrite = organize.InteractableFontSize;
        float.TryParse(organize.FontSize.text, out id.Size);
        id.PosXWrite = organize.InteractablePosX;
        float.TryParse(organize.PosX.text, out id.PosX);
        id.PosYWrite = organize.InteractablePosY;
        float.TryParse(organize.PosY.text, out id.PosY);
        id.WidthWrite = organize.InteractableWidth;
        float.TryParse(organize.Width.text, out id.Width);
        id.HeightWrite = organize.InteractableHeight;
        float.TryParse(organize.Height.text, out id.Height);
        id.FontWrite = organize.InteractableFont;
        float.TryParse(organize.Font.text, out id.Font);
        id.AlignmentWrite = organize.InteractableAlignment;
        id.Alignment = organize.AlignmentValue;
        id.ReverseWrite = organize.InteractableReverse;
        id.Reverse = organize.ReverseValue;

        return id; // Returns the mapped ID object.
    }

    private void CreateGroupsFromAllOrganize()
    {
        groups.Clear(); // Clears the list of groups.

        // Finds all OrganizeIDGroups scripts in the scene.
        OrganizeIDGroups[] organizeScripts = FindObjectsOfType<OrganizeIDGroups>();

        // For each script found, it maps to a Groups object and adds it to the list of groups.
        foreach (OrganizeIDGroups organize in organizeScripts)
        {
            Groups group = MapOrganizeToGroups(organize);
            groups.Add(group);
        }
    }

    private Groups MapOrganizeToGroups(OrganizeIDGroups organize)
    {
        Groups group = new Groups();

        float.TryParse(organize.textID.text, out group.TextID); // Converts the value of the textID field to a float and assigns it to the TextID field of the group variable.
        group.IDs = organize.textIDs.text; // Assigns the value of the textIDs field of the organize variable to the IDs field of the group variable.
        group.Test = organize.texts.text; // Assigns the value of the test field of the organize variable to the Test field of the group variable.

        return group;
    }

    private void CreateCommentsFromAllOrganize()
    {
        comments.Clear(); // Clears the comments list.

        OrganizeCommentsIDS[] organizeScripts = FindObjectsOfType<OrganizeCommentsIDS>(); // Finds all OrganizeComments objects in the scene.

        // For each OrganizeComments found, it maps its values to a Comments object and adds it to the list of comments.
        foreach (OrganizeCommentsIDS organize in organizeScripts)
        {
            Comments Comment = MapOrganizeToComments(organize);
            comments.Add(Comment);
        }
    }

    private Comments MapOrganizeToComments(OrganizeCommentsIDS organize)
    {
        Comments Comment = new Comments();

        float.TryParse(organize.textID.text, out Comment.comments); // Sets the comment text from the OrganizeComments text field.
        Comment.TestComments = organize.Comment.text; // Sets the comment test text from the OrganizeComments text field.

        return Comment;
    }

    // Method that sorts the list items.
    private void SortByID()
    {
        // Sorts the TextID list by the IDs of the LanguageSaveID objects using a lambda function that compares the IDs of two objects and returns a value that indicates which object should come first in the ordering.
        IDs.Sort((a, b) => a.id.CompareTo(b.id));

        // Sorts the GroupsID list by the IDs of LanguageSaveGroups objects using a lambda function that compares the IDs of two objects and returns a value that indicates which object should come first in the ordering.
        groups.Sort((a, b) => a.TextID.CompareTo(b.TextID));

        // Sorts the CommentsID list by the IDs of the LanguageSaveComments objects using a lambda function that compares the IDs of two objects and returns a value that indicates which object should come first in the ordering.
        comments.Sort((a, b) => a.comments.CompareTo(b.comments));
    }
}

[System.Serializable]
public class ID
{
    [Header("Information")]
    public float id; // The unique identifier for the text.
    [Space(5)]
    public bool TestWrite; // Indicates whether text can be written to the save file.
    public string Test; // The text content.
    public bool SizeWrite; // Indicates whether the text size can be written to the save file.
    public float Size; // The text size.
    public bool PosXWrite; // Indicates whether the X position of the text can be written to the save file.
    public float PosX; // The X position of the text on the screen.
    public bool PosYWrite; // Indicates whether the Y position of the text can be written to the save file.
    public float PosY; // The Y position of the text on the screen.
    public bool WidthWrite; // Indicates whether the text width can be written to the save file.
    public float Width; // The width of the text on the screen.
    public bool HeightWrite; // Indicates whether text height can be written to the save file.
    public float Height; // The height of the text on the screen.
    public bool FontWrite; // Indicates whether the text font can be written in the save file.
    public float Font; // What is the font of the text.
    public bool AlignmentWrite; // Indicates whether text alignment can be written to the save file.
    public float Alignment; // The text alignment value.
    public bool ReverseWrite; // Indicates whether inverted text can be written to the save file.
    public float Reverse; // Indicates whether the text is inverted.
}

[System.Serializable]
public class Groups
{
    [Header("Information")]
    public float TextID; // ID of the text that makes up the group.
    [Space(5)]
    public string IDs; // IDs of the texts that are part of the group (separated by semicolons).
    public string Test; // Group text.
}

[System.Serializable]
public class Comments
{
    [Header("Information")]
    public float comments; // Stores the Value of the comment.
    [Space(5)]
    public string TestComments; // Stores the comment text.
}