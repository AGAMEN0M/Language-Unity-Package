/*
 * ---------------------------------------------------------------------------
 * Description: Unity editor test script for TSV tables. Provides buttons in
 *              the Inspector to load, save, read, and modify TSV table content.
 *              
 * Author: Lucas Gomes Cecchini
 * Pseudonym: AGAMENOM
 * ---------------------------------------------------------------------------
*/

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

using static TSVTools.TabTableUtility;
#endif

namespace TSVTools
{
    [AddComponentMenu("Tools/TSV Tools/Test Table")]
    public class TestTable : MonoBehaviour
    {
    #if UNITY_EDITOR

        #region === Serialized Fields ===

        [Header("Table Data")]
        [SerializeField, Tooltip("Array holding the loaded table data.")]
        private VerticalTable[] table;

        [SerializeField, Tooltip("Path to the TSV file.")]
        private string filePath;

        [Header("Cell Operations")]
        [SerializeField, TextArea, Tooltip("Text input for setting a specific cell's content.")]
        private string text;

        [SerializeField, Tooltip("Row index for the selected cell.")]
        private int columnVertical;

        [SerializeField, Tooltip("Column index for the selected cell.")]
        private int columnHorizontal;

        [Header("Add/Remove Operations")]
        [SerializeField, Tooltip("Direction for adding or removing rows/columns.")]
        private LineDirection direction;

        [SerializeField, Tooltip("Position for adding or removing a row/column.")]
        private int addColumn;

        #endregion

        #region === Properties ===

        /// <summary>
        /// Gets or sets the table data array.
        /// </summary>
        public VerticalTable[] Table
        {
            get => table;
            set => table = value;
        }

        /// <summary>
        /// Gets or sets the path of the TSV file currently loaded.
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        /// <summary>
        /// Gets or sets the text content for cell operations.
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Gets or sets the row index of the selected cell.
        /// </summary>
        public int ColumnVertical
        {
            get => columnVertical;
            set => columnVertical = value;
        }

        /// <summary>
        /// Gets or sets the column index of the selected cell.
        /// </summary>
        public int ColumnHorizontal
        {
            get => columnHorizontal;
            set => columnHorizontal = value;
        }

        /// <summary>
        /// Gets or sets the direction used for adding or removing lines (row/column).
        /// </summary>
        public LineDirection Direction
        {
            get => direction;
            set => direction = value;
        }

        /// <summary>
        /// Gets or sets the index used for adding or removing a row/column.
        /// </summary>
        public int AddColumn
        {
            get => addColumn;
            set => addColumn = value;
        }

        #endregion

        #region === Public Methods ===

        /// <summary>
        /// Opens a file panel to select a TSV file and loads its content into the table.
        /// </summary>
        public void GetFile()
        {
            // Open file panel to select a TSV file.
            filePath = EditorUtility.OpenFilePanel("Select TSV File", Application.dataPath, "tsv");

            if (!string.IsNullOrEmpty(filePath))
            {
                LoadTableFile(filePath, ref table); // Load the table content.
                Debug.Log("File loaded successfully.", this);
            }
            else
            {
                Debug.LogWarning("No file selected.", this);
            }
        }

        /// <summary>
        /// Checks if the table is loaded and valid.
        /// Returns true if the table is invalid (null or empty).
        /// </summary>
        private bool Checking()
        {
            if (table == null || table.Length == 0)
            {
                Debug.LogError("Table is empty. Load a file first.", this);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Opens a save file panel and saves the current table to a TSV file.
        /// </summary>
        public void SaveFile()
        {
            if (Checking()) return;

            // Open save file dialog.
            string savePath = EditorUtility.SaveFilePanel("Save TSV File", Application.dataPath, "table", "tsv");

            if (!string.IsNullOrEmpty(savePath))
            {
                SaveTableFile(savePath, table); // Save table.
                Debug.Log($"File saved successfully at: {savePath}", this);
            }
            else
            {
                Debug.LogWarning("Save operation cancelled.", this);
            }
        }

        /// <summary>
        /// Retrieves text from a specific cell in the table and logs it to the console.
        /// </summary>
        public void GetText()
        {
            if (Checking()) return;

            // Get the text from the specified cell.
            string cellText = TabTableUtility.GetText(table, columnVertical, columnHorizontal);

            if (cellText != null)
            {
                text = cellText;
                Debug.Log($"Text at ({columnVertical}, {columnHorizontal}): {cellText}", this);
            }
        }

        /// <summary>
        /// Sets text for a specific cell in the table.
        /// </summary>
        public void SetText()
        {
            if (Checking()) return;

            // Set the text in the specified cell.
            TabTableUtility.SetText(ref table, columnVertical, columnHorizontal, text);
            Debug.Log($"Text set at ({columnVertical}, {columnHorizontal}): {text}", this);
        }

        /// <summary>
        /// Adds a row or column in the table at the specified position.
        /// </summary>
        public void AddColumnInTable()
        {
            if (Checking()) return;

            // Add a line in the given direction.
            AddLine(ref table, addColumn, direction);
        }

        /// <summary>
        /// Removes a row or column from the table at the specified position.
        /// </summary>
        public void RemoveColumnInTable()
        {
            if (Checking()) return;

            // Remove a line in the given direction.
            RemoveLine(ref table, addColumn, direction);
        }

        #endregion

    #endif
    }

#if UNITY_EDITOR

    #region === Inspector ===

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TestTable))]
    public class TestTableInspector : Editor
    {
        /// <summary>
        /// Draws custom Inspector GUI with buttons for all TestTable context menu methods.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var script = (TestTable)target;

            // Row 1: File operations.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(
                "Get File",
                "Opens a file dialog to select a TSV file and loads its content into the table."
                ), GUILayout.Width(200))) script.GetFile();
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent(
                "Save File",
                "Opens a save dialog and writes the current table data to a TSV file."
                ), GUILayout.Width(200))) script.SaveFile();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Row 2: Cell operations.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(
                "Get Text",
                "Reads the value from the selected cell (row and column) and stores it in the text field."
                ), GUILayout.Width(200))) script.GetText();
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent(
                "Set Text",
                "Writes the current text value into the selected cell (row and column)."
                ), GUILayout.Width(200))) script.SetText();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Row 3: Add/Remove operations.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(
                "Add Line",
                "Adds a new row or column based on the selected direction at the specified index."
                ), GUILayout.Width(200))) script.AddColumnInTable();
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent(
                "Remove Line",
                "Removes a row or column based on the selected direction at the specified index."
                ), GUILayout.Width(200))) script.RemoveColumnInTable();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            DrawDefaultInspector(); // Draw the default inspector below the custom buttons.
        }
    }

    #endregion

#endif
}