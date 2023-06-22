#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class LanguageSaveEditorWindow : EditorWindow
{
    // Defines public variables used in the editor window.
    public string SaveFile = "/StreamingAssets/Language/"; // File destination path.
    public string FileName = "ENGLISH - Base File"; // File name.
    public string LanguageName = "ENGLISH"; // Language name.
    public string ComputerLanguage = "English (United States)"; // Name of the language on the user's computer.

    // Defines lists of IDs, groups and comments.
    public List<LanguageSaveID> TextID = new();
    private int selectedIndex = -1;
    private Color currentColor = Color.white;

    public List<LanguageSaveGroups> GroupsID = new();
    private int selectedGroupsIndex = -1;
    private Color currentGroupsColor = Color.white;

    public List<LanguageSaveComments> CommentsID = new();
    private int selectedCommentsIndex = -1;
    private Color currentCommentsColor = Color.white;

    public Vector2 scrollPosition = Vector2.zero; // Defines the scroll position in the window.
    public bool firstTime = false; // Defines whether this is the first time the window is displayed.

    // Defines the menu that will be displayed in the editor window.
    [MenuItem("Window/Language/Language Save Editor")]
    public static void ShowWindow()
    {
        GetWindow<LanguageSaveEditorWindow>("Language Save Editor");
    }

    // Adds an item to a list.
    private void AddItem<T>(List<T> lista)
    {
        // Checks if the list is empty.
        if (lista.Count == 0)
        {
            // If empty, create a new item and add it to the list.
            if (typeof(T) == typeof(LanguageSaveID))
            {
                TextID.Add(new LanguageSaveID());
            }
            else if (typeof(T) == typeof(LanguageSaveGroups))
            {
                GroupsID.Add(new LanguageSaveGroups());
            }
            else if (typeof(T) == typeof(LanguageSaveComments))
            {
                CommentsID.Add(new LanguageSaveComments());
            }
        }
        else
        {
            // If not empty, checks what type of object the list contains.
            if (lista[0] is LanguageSaveID)
            {
                // Add a new item to the LanguageSaveID list.
                TextID.Add(new LanguageSaveID());
            }
            else if (lista[0] is LanguageSaveGroups)
            {
                // Add a new item to the LanguageSaveGroups list.
                GroupsID.Add(new LanguageSaveGroups());
            }
            else if (lista[0] is LanguageSaveComments)
            {
                // Add a new item to the LanguageSaveComments list.
                CommentsID.Add(new LanguageSaveComments());
            }
        }
    }

    // Removes an item from a list.
    private void RemoveItem<T>(List<T> lista,int index)
    {
        if (lista is List<LanguageSaveID>)
        {
            TextID.RemoveAt(index);
            selectedIndex = -1;
        }
        else if (lista is List<LanguageSaveGroups>)
        {
            GroupsID.RemoveAt(index);
            selectedGroupsIndex = -1;
        }
        else if (lista is List<LanguageSaveComments>)
        {
            CommentsID.RemoveAt(index);
            selectedCommentsIndex = -1;
        }
    }

    // Displays the editor window.
    private void OnGUI()
    {
        // Set the default settings the first time the method runs and load the saved file if it exits.
        if (firstTime == false)
        {
            DefaultSettings();
            LoadDataFromFile();
            firstTime = true;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition); // The scrolling area begins.

        // Configuration section of the file to be saved.
        GUILayout.Label("Save File");
        SaveFile = GUILayout.TextField(SaveFile);
        GUILayout.Label("File Name");
        FileName = GUILayout.TextField(FileName);
        GUILayout.Label("Language Name");
        LanguageName = GUILayout.TextField(LanguageName);
        GUILayout.Label("Computer Language");
        ComputerLanguage = GUILayout.TextField(ComputerLanguage);

        // Save and open folder buttons.
        if (GUILayout.Button("Save"))
        {
            SaveTXTFile();
            SaveDataToFile();
        }

        if (GUILayout.Button("Open Folder"))
        {
            OpenFolder();
        }

        // Button to organize the lists.
        if (GUILayout.Button("Organize All Items"))
        {
            SortByID();
        }

        // ----------------------------------------------------------------------

        // ID section.
        GUILayout.Label("IDs (" + TextID.Count + ")", EditorStyles.boldLabel);

        GUILayout.BeginVertical(EditorStyles.helpBox);

        // Fixed-width label style.
        GUIStyle labelStyle = new (EditorStyles.label);
        labelStyle.fixedWidth = 50f;

        // Loop through IDs.
        for (int j = 0; j < TextID.Count; j++)
        {
            LanguageSaveID id = TextID[j];
            GUILayout.BeginHorizontal();

            // Button to remove the current ID.
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
            {
                selectedIndex = j;
            }

            GUILayout.Label("ID", labelStyle); // Styled "ID" label.

            // Checks for ID conflicts with others.
            bool conflict = false;
            for (int k = 0; k < TextID.Count; k++)
            {
                if (k != j && TextID[k].IDs == id.IDs)
                {
                    conflict = true;
                    break;
                }
            }

            // Defines the color of the current ID according to the existence of a conflict.
            if (conflict)
            {
                currentColor = Color.red;
            }
            else
            {
                currentColor = Color.white;
            }

            GUI.color = currentColor; // Applies the color to the current ID.

            id.IDs = EditorGUILayout.FloatField(id.IDs); // Input field for ID value.

            // Styled label "Test" and input field for test value.
            GUILayout.Label("Test", labelStyle);
            id.Test = EditorGUILayout.TextField(id.Test, GUILayout.Width(150));

            // Stylish labels and input fields for other values.
            GUILayout.Label("Size", labelStyle);
            id.Size = EditorGUILayout.FloatField(id.Size);

            GUILayout.Label("PosX", labelStyle);
            id.PosX = EditorGUILayout.FloatField(id.PosX);

            GUILayout.Label("PosY", labelStyle);
            id.PosY = EditorGUILayout.FloatField(id.PosY);

            GUILayout.Label("Width", labelStyle);
            id.Width = EditorGUILayout.FloatField(id.Width);

            GUILayout.Label("Height", labelStyle);
            id.Height = EditorGUILayout.FloatField(id.Height);

            GUILayout.Label("Font", labelStyle);
            id.Font = EditorGUILayout.FloatField(id.Font);

            GUILayout.Label("Alignment", labelStyle);
            id.Alignment = EditorGUILayout.FloatField(id.Alignment);

            GUILayout.Label("Reverse", labelStyle);
            id.Reverse = EditorGUILayout.FloatField(id.Reverse);

            GUI.color = Color.white; // Returns to default color.

            TextID[j] = id; // Updates the modified ID in the list.

            GUILayout.EndHorizontal(); // Closes the horizontal group.

            // Creates a new line for the label for the bools.
            GUILayout.BeginHorizontal();
            GUILayout.Space(180);

            GUI.color = currentColor; // Sets the current color for the GUI.

            // Labels toggled to current layout position.
            GUILayout.Label("Test Write", labelStyle);
            id.TestWrite = EditorGUILayout.Toggle(id.TestWrite);
            GUILayout.Space(50);

            GUILayout.Label("Size Write", labelStyle);
            id.SizeWrite = EditorGUILayout.Toggle(id.SizeWrite);

            GUILayout.Label("PosX Write", labelStyle);
            id.PosXWrite = EditorGUILayout.Toggle(id.PosXWrite);

            GUILayout.Label("PosY Write", labelStyle);
            id.PosYWrite = EditorGUILayout.Toggle(id.PosYWrite);

            GUILayout.Label("Width Write", labelStyle);
            id.WidthWrite = EditorGUILayout.Toggle(id.WidthWrite);

            GUILayout.Label("Height Write", labelStyle);
            id.HeightWrite = EditorGUILayout.Toggle(id.HeightWrite);

            GUILayout.Label("Font Write", labelStyle);
            id.FontWrite = EditorGUILayout.Toggle(id.FontWrite);

            GUILayout.Label("Alignment Write", labelStyle);
            id.AlignmentWrite = EditorGUILayout.Toggle(id.AlignmentWrite);

            GUILayout.Label("Reverse Write", labelStyle);
            id.ReverseWrite = EditorGUILayout.Toggle(id.ReverseWrite);

            GUI.color = Color.white; // Restores the GUI color to the default color (white).

            GUILayout.EndHorizontal(); // Finalizes the horizontal layout.
        }

        GUILayout.Space(10); // Adds a 10-pixel empty space to the layout.

        GUILayout.BeginHorizontal(); // Starts a horizontal layout, aligning elements horizontally.

        // Button to add an item to the TextID list.
        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(20)))
        {
            AddItem(TextID);
        }

        // Button to remove the current item from the list.
        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(20)))
        {
            if (selectedIndex >= 0 && selectedIndex < TextID.Count)
            {
                RemoveItem(TextID, selectedIndex);
            }
            else
            {
                RemoveItem(TextID, TextID.Count - 1);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        // ----------------------------------------------------------------------

        // ID Groups section.
        GUILayout.Label("ID Groups (" + GroupsID.Count + ")", EditorStyles.boldLabel);

        GUILayout.BeginVertical(EditorStyles.helpBox);

        // Fixed-width label style.
        GUIStyle labelStyleGroups = new (EditorStyles.label);
        labelStyleGroups.fixedWidth = 50f;

        // Loop through the ID Groups.
        for (int j = 0; j < GroupsID.Count; j++)
        {
            LanguageSaveGroups id = GroupsID[j];
            GUILayout.BeginHorizontal();

            // Button to remove the current Text ID.
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
            {
                selectedGroupsIndex = j;
            }

            GUILayout.Label("Text ID", labelStyleGroups); // Styled "Text ID" label.

            // Checks for ID conflicts with others.
            bool conflict = false;
            for (int k = 0; k < GroupsID.Count; k++)
            {
                if (k != j && GroupsID[k].TextID == id.TextID)
                {
                    conflict = true;
                    break;
                }
            }

            // Defines the color of the current ID according to the existence of a conflict.
            if (conflict)
            {
                currentGroupsColor = Color.red;
            }
            else
            {
                currentGroupsColor = Color.white;
            }

            GUI.color = currentGroupsColor; // Applies the color to the current ID.

            id.TextID = EditorGUILayout.FloatField(id.TextID, GUILayout.Width(50)); // Input field for the TextID value.

            // Label "IDs" with style and input field for test value.
            GUILayout.Label("IDs", labelStyleGroups);
            id.IDs = EditorGUILayout.TextField(id.IDs, GUILayout.Width(300));

            // Styled label "Test" and input field for test value.
            GUILayout.Label("Test", labelStyleGroups);
            id.Test = EditorGUILayout.TextField(id.Test, GUILayout.Width(300));

            GUI.color = Color.white; // Returns to default color.

            GroupsID[j] = id; // Updates the modified ID in the list.

            GUILayout.EndHorizontal(); // Closes the horizontal group.
        }

        GUILayout.Space(10); // Adds a 10-pixel empty space to the layout.

        GUILayout.BeginHorizontal(); // Starts a horizontal layout, aligning elements horizontally.

        // Button to add an item to the GroupsID list.
        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(20)))
        {
            AddItem(GroupsID);
        }

        // Button to remove the current item from the list.
        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(20)))
        {
            if (selectedGroupsIndex >= 0 && selectedGroupsIndex < GroupsID.Count)
            {
                RemoveItem(GroupsID, selectedGroupsIndex);
            }
            else
            {
                RemoveItem(GroupsID, GroupsID.Count - 1);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        // ----------------------------------------------------------------------

        // CommentsID section.
        GUILayout.Label("Comments (" + CommentsID.Count + ")", EditorStyles.boldLabel);

        GUILayout.BeginVertical(EditorStyles.helpBox);

        // Fixed-width label style.
        GUIStyle labelStyleComments = new (EditorStyles.label);
        labelStyleComments.fixedWidth = 50f;

        // Loop through the ID Groups.
        for (int j = 0; j < CommentsID.Count; j++)
        {
            LanguageSaveComments id = CommentsID[j];
            GUILayout.BeginHorizontal();

            // Button to remove the current Text ID.
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
            {
                selectedCommentsIndex = j;
            }

            GUILayout.Label("ID", labelStyleComments); // Styled "ID" label.

            // Checks for ID conflicts with others.
            bool conflict = false;
            for (int k = 0; k < CommentsID.Count; k++)
            {
                if (k != j && CommentsID[k].comments == id.comments)
                {
                    conflict = true;
                    break;
                }
            }

            // Defines the color of the current ID according to the existence of a conflict.
            if (conflict)
            {
                currentCommentsColor = Color.red;
            }
            else
            {
                currentCommentsColor = Color.white;
            }

            GUI.color = currentCommentsColor; // Applies the color to the current ID.

            id.comments = EditorGUILayout.FloatField(id.comments, GUILayout.Width(50)); // Input field for the comments value.

            // Label "Test Comments" with style and input field for test value.
            GUILayout.Label("Test Comments", labelStyleComments);
            id.TestComments = EditorGUILayout.TextField(id.TestComments, GUILayout.Width(650));

            GUI.color = Color.white; // Returns to default color.

            CommentsID[j] = id; // Updates the modified ID in the list.

            GUILayout.EndHorizontal(); // Closes the horizontal group.
        }

        GUILayout.Space(10); // Adds a 10-pixel empty space to the layout.

        GUILayout.BeginHorizontal(); // Starts a horizontal layout, aligning elements horizontally.

        // Button to add an item to the CommentsID list.
        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(20)))
        {
            AddItem(CommentsID);
        }

        // Button to remove the current item from the list.
        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(20)))
        {
            if (selectedCommentsIndex >= 0 && selectedCommentsIndex < CommentsID.Count)
            {
                RemoveItem(CommentsID, selectedCommentsIndex);
            }
            else
            {
                RemoveItem(CommentsID, CommentsID.Count - 1);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.EndScrollView();
    }

    // Method to save the language data to a text file.
    private void SaveTXTFile()
    {
        SortByID(); // Organize all items.

        // Get the file path to save the data.
        string Path = Application.dataPath + SaveFile;
        string filePath = Path + FileName + ".txt";

        StringBuilder sb = new(); // Create a StringBuilder to store the data.

        // Append the language name and computer language to the StringBuilder.
        sb.AppendLine("Linguagem - [" + LanguageName + "]");
        sb.AppendLine(ComputerLanguage);

        // Loop through each LanguageSaveID object and append its data to the StringBuilder.
        foreach (LanguageSaveID id in TextID)
        {
            sb.Append("id:" + id.IDs + "; ");

            if (id.TestWrite)
            {
                sb.Append("{" + id.Test + "} ");
            }

            // Check if size data should be included and append it if necessary.
            if (id.SizeWrite)
            {
                sb.Append("S:" + id.Size.ToString("F0") + "; ");
            }

            // Check if position data should be included and append it if necessary.
            if (id.PosXWrite)
            {
                sb.Append("X:" + id.PosX + "; ");
            }

            if (id.PosYWrite)
            {
                sb.Append("Y:" + id.PosY + "; ");
            }

            // Check if width and height data should be included and append it if necessary.
            if (id.WidthWrite)
            {
                sb.Append("Width:" + id.Width + "; ");
            }

            if (id.HeightWrite)
            {
                sb.Append("Height:" + id.Height + "; ");
            }

            // Check if font data should be included and append it if necessary.
            if (id.FontWrite)
            {
                sb.Append("Font:" + id.Font + "; ");
            }

            // Check if alignment data should be included and append it if necessary.
            if (id.AlignmentWrite)
            {
                sb.Append("Ali:" + id.Alignment + "; ");
            }

            // Check if reverse data should be included and append it if necessary.
            if (id.ReverseWrite)
            {
                sb.Append("Rev:" + id.Reverse + ";");
            }

            // Add a new line to separate each LanguageSaveID object's data.
            sb.AppendLine();
        }

        // Loop through each LanguageSaveGroups object and append its data to the StringBuilder.
        foreach (LanguageSaveGroups groups in GroupsID)
        {
            sb.AppendLine("[Text:" + groups.TextID + "; {" + groups.Test + "} ids:" + groups.IDs + "]");
        }

        // Loop through each LanguageSaveComments object and append its data to the StringBuilder.
        foreach (LanguageSaveComments comment in CommentsID)
        {
            sb.AppendLine("[Comments:" + comment.comments + "; {" + comment.TestComments + "}]");
        }

        File.WriteAllText(filePath, sb.ToString()); // Write the StringBuilder's data to the file.
        Debug.Log("Saved File: " + filePath); // Log a message to indicate that the file has been saved.
    }

    // This method opens the folder where the save file is located.
    private void OpenFolder()
    {
        // Get the full path of the save file by appending it to the data path of the application.
        string Path = Application.dataPath + SaveFile;

        Application.OpenURL(Path); // Open the folder where the save file is located.
    }

    // This method sets default values for various properties of the LanguageSaveID objects.
    private void DefaultSettings()
    {
        // Create a new LanguageSaveID object and set its properties.
        LanguageSaveID item1 = new();
        item1.IDs = -11f;
        item1.TestWrite = true;
        item1.Test = "Toggle";
        item1.SizeWrite = true;
        item1.Size = 0f;
        item1.PosXWrite = true;
        item1.PosX = 0f;
        item1.PosYWrite = true;
        item1.PosY = 0f;
        item1.WidthWrite = true;
        item1.Width = 0f;
        item1.HeightWrite = true;
        item1.Height = 0f;
        item1.FontWrite = true;
        item1.Font = 0f;
        item1.AlignmentWrite = true;
        item1.Alignment = 0f;
        item1.ReverseWrite = true;
        item1.Reverse = 0f;
        TextID.Add(item1); // Add the LanguageSaveID object to the TextID list.

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item2 = new();
        item2.IDs = -10f;
        item2.TestWrite = true;
        item2.Test = "Test Language";
        TextID.Add(item2);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item3 = new();
        item3.IDs = -9f;
        item3.TestWrite = true;
        item3.Test = "Hello World";
        item3.SizeWrite = true;
        item3.Size = 0f;
        item3.FontWrite = true;
        item3.Font = 0f;
        item3.ReverseWrite = true;
        item3.Reverse = 0f;
        TextID.Add(item3);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item4 = new();
        item4.IDs = -8f;
        item4.TestWrite = true;
        item4.Test = "New Text";
        item4.SizeWrite = true;
        item4.Size = 0f;
        item4.PosXWrite = true;
        item4.PosX = 0f;
        item4.PosYWrite = true;
        item4.PosY = 0f;
        item4.WidthWrite = true;
        item4.Width = 0f;
        item4.HeightWrite = true;
        item4.Height = 0f;
        item4.FontWrite = true;
        item4.Font = 0f;
        item4.AlignmentWrite = true;
        item4.Alignment = 0f;
        item4.ReverseWrite = true;
        item4.Reverse = 0f;
        TextID.Add(item4);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item5 = new();
        item5.IDs = -7f;
        item5.SizeWrite = true;
        item5.Size = 0f;
        item5.FontWrite = true;
        item5.Font = 0f;
        item5.AlignmentWrite = true;
        item5.Alignment = 0f;
        item5.ReverseWrite = true;
        item5.Reverse = 0f;
        TextID.Add(item5);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item6 = new();
        item6.IDs = -6f;
        item6.TestWrite = true;
        item6.Test = "Enter text...";
        item6.SizeWrite = true;
        item6.Size = 0f;
        item6.PosXWrite = true;
        item6.PosX = 0f;
        item6.PosYWrite = true;
        item6.PosY = 0f;
        item6.WidthWrite = true;
        item6.Width = 0f;
        item6.HeightWrite = true;
        item6.Height = 0f;
        item6.FontWrite = true;
        item6.Font = 0f;
        item6.AlignmentWrite = true;
        item6.Alignment = 0f;
        item6.ReverseWrite = true;
        item6.Reverse = 0f;
        TextID.Add(item6);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item7 = new();
        item7.IDs = -5f;
        item7.FontWrite = true;
        item7.Font = 0f;
        item7.AlignmentWrite = true;
        item7.Alignment = 0f;
        item7.ReverseWrite = true;
        item7.Reverse = 0f;
        TextID.Add(item7);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item8 = new();
        item8.IDs = -4f;
        item8.TestWrite = true;
        item8.Test = "Option C";
        TextID.Add(item8);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item9 = new();
        item9.IDs = -3f;
        item9.TestWrite = true;
        item9.Test = "Option B";
        TextID.Add(item9);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item10 = new();
        item10.IDs = -2f;
        item10.TestWrite = true;
        item10.Test = "Option A";
        TextID.Add(item10);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item11 = new();
        item11.IDs = -1f;
        item11.TestWrite = true;
        item11.Test = "Button";
        item11.SizeWrite = true;
        item11.Size = 0f;
        item11.PosXWrite = true;
        item11.PosX = 0f;
        item11.PosYWrite = true;
        item11.PosY = 0f;
        item11.WidthWrite = true;
        item11.Width = 0f;
        item11.HeightWrite = true;
        item11.Height = 0f;
        item11.FontWrite = true;
        item11.Font = 0f;
        item11.AlignmentWrite = true;
        item11.Alignment = 0f;
        item11.ReverseWrite = true;
        item11.Reverse = 0f;
        TextID.Add(item11);

        // Create more LanguageSaveID objects and set their properties in a similar way.
        LanguageSaveID item12 = new();
        item12.IDs = 0f;
        item12.TestWrite = true;
        item12.Test = "No language ID";
        item12.SizeWrite = true;
        item12.Size = 0f;
        item12.PosXWrite = true;
        item12.PosX = 0f;
        item12.PosYWrite = true;
        item12.PosY = 0f;
        item12.WidthWrite = true;
        item12.Width = 0f;
        item12.HeightWrite = true;
        item12.Height = 0f;
        item12.FontWrite = true;
        item12.Font = 0f;
        item12.AlignmentWrite = true;
        item12.Alignment = 0f;
        item12.ReverseWrite = true;
        item12.Reverse = 0f;
        TextID.Add(item12);

        // Create a new LanguageSaveGroups object with default values and add it to the GroupsID list.
        LanguageSaveGroups Groups1 = new();
        Groups1.TextID = 0f;
        Groups1.Test = "Unity default items";
        Groups1.IDs = "0;-1;-2;-3;-4;-5;-6;-7;-8;-9;-10;-11;";
        GroupsID.Add(Groups1);

        // Create a new LanguageSaveComments object with default values and add it to the CommentsID list.
        LanguageSaveComments Comments1 = new();
        Comments1.comments = 0f;
        Comments1.TestComments = "If an id has not been defined, it will show this message.";
        CommentsID.Add(Comments1);
    }

    // Method that sorts the list items.
    private void SortByID()
    {
        // Sorts the TextID list by the IDs of the LanguageSaveID objects using a lambda function that compares the IDs of two objects and returns a value that indicates which object should come first in the ordering.
        TextID.Sort((a, b) => a.IDs.CompareTo(b.IDs));

        // Sorts the GroupsID list by the IDs of LanguageSaveGroups objects using a lambda function that compares the IDs of two objects and returns a value that indicates which object should come first in the ordering.
        GroupsID.Sort((a, b) => a.TextID.CompareTo(b.TextID));

        // Sorts the CommentsID list by the IDs of the LanguageSaveComments objects using a lambda function that compares the IDs of two objects and returns a value that indicates which object should come first in the ordering.
        CommentsID.Sort((a, b) => a.comments.CompareTo(b.comments));
    }

    // Method called when window is closed.
    private void OnDestroy()
    {
        SaveDataToFile(); // save the information.
    }

    // Method to save window settings.
    private void SaveDataToFile()
    {
        SortByID(); // Organize all items.

        // Creates or opens the file for writing.
        using (StreamWriter writer = new ("ProjectSettings/LanguageData.txt"))
        {
            // Writes the information to the file.
            writer.WriteLine(SaveFile);
            writer.WriteLine(FileName);
            writer.WriteLine(LanguageName);
            writer.WriteLine(ComputerLanguage);
            writer.WriteLine(firstTime);

            // Writes the ID information to the file.
            foreach (LanguageSaveID id in TextID)
            {
                writer.WriteLine("id:" + id.IDs + "; " + "{" + id.Test + "} " + "S:" + id.Size + "; " + "X:" + id.PosX + "; " + "Y:" + id.PosY + "; " + "Width:" + id.Width + "; " + "Height:" + id.Height + "; " + "Font:" + id.Font + "; " + "Ali:" + id.Alignment + "; " + "Rev:" + id.Reverse + "; " + "TW:" + id.TestWrite + "; " + "SW:" + id.SizeWrite + "; " + "XW:" + id.PosXWrite + "; " + "YW:" + id.PosYWrite + "; " + "WW:" + id.WidthWrite + "; " + "HW:" + id.HeightWrite + "; " + "FW:" + id.FontWrite + "; " + "AW:" + id.AlignmentWrite + "; " + "RW:" + id.ReverseWrite + ";");                
            }

            // Writes the Groups information to the file.
            foreach (LanguageSaveGroups groups in GroupsID)
            {
                writer.WriteLine("[Text:" + groups.TextID + "; {" + groups.Test + "} ids:" + groups.IDs);
            }

            // Writes the Comments information to the file.
            foreach (LanguageSaveComments comment in CommentsID)
            {
                writer.WriteLine("[Comments:" + comment.comments + "; {" + comment.TestComments + "}]");
            }
        }
    }

    // Method to load window settings.
    private void LoadDataFromFile()
    {
        // If the file exists.
        if (File.Exists("ProjectSettings/LanguageData.txt"))
        {
            // Opens the file for reading.
            using (StreamReader reader = new ("ProjectSettings/LanguageData.txt"))
            {
                // Read information from the file.
                SaveFile = reader.ReadLine();
                FileName = reader.ReadLine();
                LanguageName = reader.ReadLine();
                ComputerLanguage = reader.ReadLine();
                firstTime = bool.Parse(reader.ReadLine());

                // Iterates over the data structure to create the required objects.
                TextID.Clear();
                GroupsID.Clear();
                CommentsID.Clear();

                string line;

                // Gets the information for the TextID, GroupsID and CommentsID.
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("id:"))
                    {
                        LanguageSaveID fileIDs = new();

                        float ID = float.Parse(line.Substring(line.IndexOf(":") + 1, line.IndexOf(";") - line.IndexOf(":") - 1));

                        if (line.Contains("{"))
                        {
                            string test = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1);
                            fileIDs.Test = test;
                        }

                        if (line.Contains("S:"))
                        {
                            int startIndex = line.IndexOf("S:") + 2;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float size = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.Size = size;
                            }
                        }

                        if (line.Contains("X:"))
                        {
                            int startIndex = line.IndexOf("X:") + 2;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float posX = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.PosX = posX;
                            }
                        }

                        if (line.Contains("Y:"))
                        {
                            int startIndex = line.IndexOf("Y:") + 2;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float posY = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.PosY = posY;
                            }
                        }

                        if (line.Contains("Width:"))
                        {
                            int startIndex = line.IndexOf("Width:") + 6;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float width = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.Width = width;
                            }
                        }

                        if (line.Contains("Height:"))
                        {
                            int startIndex = line.IndexOf("Height:") + 7;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float height = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.Height = height;
                            }
                        }

                        if (line.Contains("Font:"))
                        {
                            int startIndex = line.IndexOf("Font:") + 5;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float font = float.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.Font = font;
                            }
                        }

                        if (line.Contains("Ali:"))
                        {
                            int startIndex = line.IndexOf("Ali:") + 4;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float font = float.Parse(line.Substring(startIndex, endIndex - startIndex));

                                fileIDs.Alignment = font;
                            }
                        }

                        if (line.Contains("Rev:"))
                        {
                            int startIndex = line.IndexOf("Rev:") + 4;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                float Rev = float.Parse(line.Substring(startIndex, endIndex - startIndex));

                                fileIDs.Reverse = Rev;
                            }
                        }

                        if (line.Contains("TW:"))
                        {
                            int startIndex = line.IndexOf("TW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool TW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.TestWrite = TW;
                            }
                        }

                        if (line.Contains("SW:"))
                        {
                            int startIndex = line.IndexOf("SW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool SW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.SizeWrite = SW;
                            }
                        }

                        if (line.Contains("XW:"))
                        {
                            int startIndex = line.IndexOf("XW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool XW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.PosXWrite = XW;
                            }
                        }

                        if (line.Contains("YW:"))
                        {
                            int startIndex = line.IndexOf("YW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool YW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.PosYWrite = YW;
                            }
                        }

                        if (line.Contains("WW:"))
                        {
                            int startIndex = line.IndexOf("WW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool WW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.WidthWrite = WW;
                            }
                        }

                        if (line.Contains("HW:"))
                        {
                            int startIndex = line.IndexOf("HW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool HW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.HeightWrite = HW;
                            }
                        }

                        if (line.Contains("FW:"))
                        {
                            int startIndex = line.IndexOf("FW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool FW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.FontWrite = FW;
                            }
                        }

                        if (line.Contains("AW:"))
                        {
                            int startIndex = line.IndexOf("AW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool AW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.AlignmentWrite = AW;
                            }
                        }

                        if (line.Contains("RW:"))
                        {
                            int startIndex = line.IndexOf("RW:") + 3;
                            int endIndex = line.IndexOf(";", startIndex);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                bool RW = bool.Parse(line.Substring(startIndex, endIndex - startIndex));
                                fileIDs.ReverseWrite = RW;
                            }
                        }

                        fileIDs.IDs = ID;
                        TextID.Add(fileIDs);
                    } else if (line.StartsWith("[Text:"))
                    {
                        float textID = float.Parse(line.Substring(line.IndexOf(":") + 1, line.IndexOf(";") - line.IndexOf(":") - 1));
                        string test = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1);
                        string ids = line.Substring(line.IndexOf("ids:") + 4);

                        LanguageSaveGroups fileGroup = new();
                        fileGroup.TextID = textID;
                        fileGroup.Test = test;
                        fileGroup.IDs = ids;
                        GroupsID.Add(fileGroup);
                    } else if (line.StartsWith("[Comments:"))
                    {
                        float ID = float.Parse(line.Substring(line.IndexOf(":") + 1, line.IndexOf(";") - line.IndexOf(":") - 1));
                        string test = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1);

                        LanguageSaveComments fileComment = new();
                        fileComment.comments = ID;
                        fileComment.TestComments = test;
                        CommentsID.Add(fileComment);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class LanguageSaveID
{
    public float IDs; // The unique identifier for the text.
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
public class LanguageSaveGroups
{
    public float TextID; // ID of the text that makes up the group.
    public string IDs; // IDs of the texts that are part of the group (separated by semicolons).
    public string Test; // Group text.
}

[System.Serializable]
public class LanguageSaveComments
{
    public float comments; // Stores the Value of the comment.
    public string TestComments; // Stores the comment text.
}
#endif