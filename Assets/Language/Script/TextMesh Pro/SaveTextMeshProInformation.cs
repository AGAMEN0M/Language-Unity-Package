using UnityEngine;
using TMPro;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SaveTextMeshProInformation : MonoBehaviour
{
#if UNITY_EDITOR
    // Custom Editor para o SaveLegacyInformation.
    [CustomEditor(typeof(SaveTextMeshProInformation))]
    public class SaveLegacyInformationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SaveTextMeshProInformation script = (SaveTextMeshProInformation)target; // Gets the current SaveLegacyInformation script.

            // Shows a warning to the user.
            GUIStyle style = new(EditorStyles.wordWrappedLabel);
            style.fontSize = 16;
            style.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Notice: This tool only saves the ID and its information in Menu Language Save Editor. It does not serve as an automatic configurator for the current object.", style);

            // Checks if using LText or LInputField.
            if (script.saveType == SaveType.LTextTMP || script.saveType == SaveType.LInputFieldTMP)
            {
                EditorGUILayout.LabelField("I should also point out that depending on the Middle/Center configuration of the 'Rect Transform' component, information may be collected incorrectly.", style);
                EditorGUILayout.LabelField("To ensure that this does not happen, ensure that the object's configuration has the following items: Pos X, Pos Y, Width and Height.", style);
            }

            // Button to save the ID.
            if (GUILayout.Button("Save ID"))
            {
                EditorApplication.ExecuteMenuItem("Window/Language/Language Save Editor"); // Opens the Language Save Editor.
                script.StartSava(); // Calls SaveLegacyInformation's StartSava() method.
                AddLanguageSaveID(); // Adds the saved ID to the LanguageSaveEditorWindow.
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); // Indicate that the scene has been modified.
            }

            // Shows the field to select the save type.
            EditorGUILayout.PropertyField(serializedObject.FindProperty("saveType"));

            // Shows configuration options for the LDropdownTMP type.
            if (script.saveType == SaveType.LDropdownTMP)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Dropdown_TMP Settings", EditorStyles.boldLabel);

                // Shows the field for the Required Items property.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DDisplayText"));

                // Shows the field for the property that will be saved in the Language Save Editor.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("languageSelected"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dTestWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dFontWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dFont"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dAlignmentWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dAlignment"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dReverseWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dReverse"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dReverseValue"));
            }

            // Shows configuration options for the LTextTMP type.
            if (script.saveType == SaveType.LTextTMP)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("UI Text_TMP Settings", EditorStyles.boldLabel);

                // Shows fields for Required Items properties.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FontListTMPro"));

                // Shows fields for properties that will be saved in the Language Save Editor.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TestWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Test"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Size"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosXWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosX"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosYWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosY"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WidthWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HeightWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Height"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FontWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Font"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AlignmentWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Alignment"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ReverseWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reverse"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ReverseValue"));
            }

            // Shows configuration options for the LInputFieldTMP type.
            if (script.saveType == SaveType.LInputFieldTMP)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("UI Input Field_TMP Settings", EditorStyles.boldLabel);

                // Shows fields for Required Items properties.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFtargetText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FontListTMPro"));

                // Shows fields for properties that will be saved in the Language Save Editor.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TestWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Test"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Size"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosXWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosX"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosYWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PosY"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WidthWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HeightWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Height"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FontWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Font"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AlignmentWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Alignment"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ReverseWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reverse"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ReverseValue"));

                // Shows fields for InputField properties that will be saved in the Language Save Editor.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFSizeWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFFontWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFFont"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFAlignmentWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IFAlignment"));
            }

            // Shows configuration options for the LTextMeshTMP type.
            if (script.saveType == SaveType.LTextMeshTMP)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Text Mesh_TMP Settings", EditorStyles.boldLabel);

                // Shows fields for Required Items properties.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FontListTMPro"));

                // Shows fields for properties that will be saved in the Language Save Editor.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMTestWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMTest"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMSizeWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMFontWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMFont"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMReverseWrite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMreverse"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TMReverseValue"));
            }

            serializedObject.ApplyModifiedProperties();
        }

        // Adds the current LanguageSaveID to the LanguageSaveEditorWindow.
        public void AddLanguageSaveID()
        {
            SaveTextMeshProInformation script = (SaveTextMeshProInformation)target;

            // Checks if you are using LDropdownTMP.
            if (script.saveType == SaveType.LDropdownTMP)
            {
                // SaveLegacyLanguageSelected languageSelected.
                var window = EditorWindow.GetWindow<LanguageSaveEditorWindow>();
                var languageSaveID = new LanguageSaveID
                {
                    IDs = script.dID,
                    FontWrite = script.dFontWrite,
                    Font = script.dFont,
                    AlignmentWrite = script.dAlignmentWrite,
                    Alignment = script.dAlignment,
                    ReverseWrite = script.dReverseWrite,
                    Reverse = script.dReverseValue
                };

                window.TextID.Add(languageSaveID);

                foreach (SaveLegacyLanguageSelectedTextMeshPro id in script.languageSelected)
                {
                    var window2 = EditorWindow.GetWindow<LanguageSaveEditorWindow>();
                    var languageSaveID2 = new LanguageSaveID
                    {
                        IDs = id.value,
                        Test = id.SelectedText,
                        TestWrite = script.dTestWrite
                    };

                    window2.TextID.Add(languageSaveID2);
                }
            }

            // Checks if using LTextTMP or LInputFieldTMP.
            if (script.saveType == SaveType.LTextTMP || script.saveType == SaveType.LInputFieldTMP)
            {
                var window = EditorWindow.GetWindow<LanguageSaveEditorWindow>();
                var languageSaveID = new LanguageSaveID
                {
                    IDs = script.ID,
                    TestWrite = script.TestWrite,
                    Test = script.Test,
                    SizeWrite = script.SizeWrite,
                    Size = script.Size,
                    PosXWrite = script.PosXWrite,
                    PosX = script.PosX,
                    PosYWrite = script.PosYWrite,
                    PosY = script.PosY,
                    WidthWrite = script.WidthWrite,
                    Width = script.Width,
                    HeightWrite = script.HeightWrite,
                    Height = script.Height,
                    FontWrite = script.FontWrite,
                    Font = script.Font,
                    AlignmentWrite = script.AlignmentWrite,
                    Alignment = script.Alignment,
                    ReverseWrite = script.ReverseWrite,
                    Reverse = script.ReverseValue
                };

                window.TextID.Add(languageSaveID);
            }

            // Add the InputFieldTMP information.
            if (script.saveType == SaveType.LInputFieldTMP)
            {
                var window2 = EditorWindow.GetWindow<LanguageSaveEditorWindow>();
                var languageSaveID2 = new LanguageSaveID
                {
                    IDs = script.IFID,
                    SizeWrite = script.IFSizeWrite,
                    Size = script.IFSize,
                    FontWrite = script.IFFontWrite,
                    Font = script.IFFont,
                    AlignmentWrite = script.IFAlignmentWrite,
                    Alignment = script.IFAlignment,
                    ReverseWrite = script.ReverseWrite,
                    Reverse = script.ReverseValue
                };

                window2.TextID.Add(languageSaveID2);
            }

            // Checks if using LTextMeshTMP.
            if (script.saveType == SaveType.LTextMeshTMP)
            {
                var window = EditorWindow.GetWindow<LanguageSaveEditorWindow>();
                var languageSaveID = new LanguageSaveID
                {
                    IDs = script.TMID,
                    TestWrite = script.TMTestWrite,
                    Test = script.TMTest,
                    SizeWrite = script.TMSizeWrite,
                    Size = script.TMSize,
                    FontWrite = script.TMFontWrite,
                    Font = script.TMFont,
                    ReverseWrite = script.TMReverseWrite,
                    Reverse = script.TMReverseValue
                };

                window.TextID.Add(languageSaveID);
            }
        }

    }
#endif
    [Header("Settings")]
    [SerializeField] private SaveType saveType = SaveType.LTextTMP; // Save type to use.
    [Space(10)]
    [Header("Required Items")]
    [SerializeField] private GameObject targetText; // Text that will be saved.
    [SerializeField] private GameObject IFtargetText; // InputField text that will be saved.
    [SerializeField] private GameObject DDisplayText; // Dropdown text component.
    [SerializeField] private LanguageFontListTextMeshPro FontListTMPro; // List of used fonts.
    [Space(10)]
    [Header("LanguageDropdownOptions Settings")]
    [SerializeField] private List<SaveLegacyLanguageSelectedTextMeshPro> languageSelected; // List of all information.
    [SerializeField] private float dID; // Text ID.
    [SerializeField] private bool dTestWrite; // Recording test.
    [SerializeField] private bool dFontWrite; // Font Write Test.
    [SerializeField] private float dFont;  // Index of the font used.
    [SerializeField] private bool dAlignmentWrite; // Alignment engraving test.
    [SerializeField] private float dAlignment; // Text alignment.
    [SerializeField] private bool dReverseWrite; // Reverse write test.
    [SerializeField] private Reverse dReverse = Reverse.Default; // Reversal type to use.
    [SerializeField] private float dReverseValue; // Reversal value.
    [Space(10)]
    [Header("LanguageText Settings")]
    [SerializeField] private float ID; // Text ID.
    [SerializeField] private bool TestWrite; // Recording test.
    [SerializeField] private string Test; // Test text.
    [SerializeField] private bool SizeWrite; // Text size recording test.
    [SerializeField] private float Size; // Text size.
    [SerializeField] private bool PosXWrite; // X position recording test.
    [SerializeField] private float PosX; // Text X position.
    [SerializeField] private bool PosYWrite; // Y position recording test.
    [SerializeField] private float PosY; // Y position of the text.
    [SerializeField] private bool WidthWrite; // Width engraving test.
    [SerializeField] private float Width; // Text width.
    [SerializeField] private bool HeightWrite; // Height recording test.
    [SerializeField] private float Height; // Text height.
    [SerializeField] private bool FontWrite; // Font Write Test.
    [SerializeField] private float Font;  // Index of the font used.
    [SerializeField] private bool AlignmentWrite; // Alignment engraving test.
    [SerializeField] private float Alignment; // Text alignment.
    [SerializeField] private bool ReverseWrite; // Reverse write test.
    [SerializeField] private Reverse reverse = Reverse.Default; // Reversal type to use.
    [SerializeField] private float ReverseValue; // Reversal value.
    [Space(10)]
    [Header("LanguageTextInputField Settings")]
    [SerializeField] private float IFID; // InputField's text ID.
    [SerializeField] private bool IFSizeWrite; // Text size recording test.
    [SerializeField] private float IFSize; // Text size.
    [SerializeField] private bool IFFontWrite; // Font Write Test.
    [SerializeField] private float IFFont;  // Index of the font used.
    [SerializeField] private bool IFAlignmentWrite; // Alignment engraving test.
    [SerializeField] private float IFAlignment; // Text alignment.
    [Space(10)]
    [Header("LanguageTextMesh Settings")]
    [SerializeField] private float TMID; // TextMesh ID.
    [SerializeField] private bool TMTestWrite; // Recording test.
    [SerializeField] private string TMTest; // Test text.
    [SerializeField] private bool TMSizeWrite; // TextMesh size recording test.
    [SerializeField] private float TMSize; // Text size.
    [SerializeField] private bool TMFontWrite; // Font Write Test.
    [SerializeField] private float TMFont;  // Index of the font used.
    [SerializeField] private bool TMReverseWrite; // Reverse write test.
    [SerializeField] private Reverse TMreverse = Reverse.Default; // Reversal type to use.
    [SerializeField] private float TMReverseValue; // Reversal value.

    // Enums for different types of save and revert.
    public enum SaveType
    {
        LDropdownTMP,
        LTextTMP,
        LInputFieldTMP,
        LTextMeshTMP
    }

    public enum Reverse
    {
        Default,
        False,
        True
    }

    // Method to start saving.
    private void StartSava()
    {
        if (saveType == SaveType.LDropdownTMP)
        {
            SavaLDropdown();
        }

        if (saveType == SaveType.LTextTMP)
        {
            SavaLText();
        }

        if (saveType == SaveType.LInputFieldTMP)
        {
            SavaLText();
            SavaLInputField();
        }

        if (saveType == SaveType.LTextMeshTMP)
        {
            SavaLTextMesh();
        }
    }

    // Method to save Dropdown IDs.
    private void SavaLDropdown()
    {
        LanguageDropdownOptionsTextMeshPro languageDropdownOptions = targetText.GetComponent<LanguageDropdownOptionsTextMeshPro>(); // Gets the LanguageDropdownOptionsTextMeshPro component of the target object.

        dID = languageDropdownOptions.valueID; // Gets the ID of the LanguageDropdownOptions.

        languageSelected.Clear(); // Clear the list of languageSelected.

        // It collects all the information and puts it on the list.
        foreach (LanguageSelectedTextMeshPro id in languageDropdownOptions.languageSelected)
        {
            var saveLegacyLanguage = new SaveLegacyLanguageSelectedTextMeshPro
            {
                SelectedText = id.SelectedText,
                value = id.value
            };

            languageSelected.Add(saveLegacyLanguage);
        }

        dTestWrite = true; // Sets the flag to indicate that the text must be saved.

        // Gets character order reversal information.
        if (dReverse == Reverse.Default)
        {
            dReverseValue = 0f;
        }
        else if (dReverse == Reverse.False)
        {
            dReverseValue = 1f;
        }
        else if (dReverse == Reverse.True)
        {
            dReverseValue = 2f;
        }
        dReverseWrite = true; // Sets the flag to indicate that inversion information should be saved.

        TMP_Text text = DDisplayText.GetComponent<TMP_Text>(); // Gets the Text component of the target object.

        // Gets alignment information from the Text component of the target object.
        if (text.alignment == TextAlignmentOptions.Left || text.alignment == TextAlignmentOptions.TopLeft || text.alignment == TextAlignmentOptions.BottomLeft)
        {
            dAlignment = 1;
        }
        else if (text.alignment == TextAlignmentOptions.Center || text.alignment == TextAlignmentOptions.Top || text.alignment == TextAlignmentOptions.Bottom)
        {
            dAlignment = 2;
        }
        else if (text.alignment == TextAlignmentOptions.Right || text.alignment == TextAlignmentOptions.TopRight || text.alignment == TextAlignmentOptions.BottomRight)
        {
            dAlignment = 3;
        }
        dAlignmentWrite = true; // Sets the flag to indicate that alignment information should be saved.

        TMP_FontAsset font = text.font; // Gets the font used by the Text component of the target object.

        int index = FontListTMPro.FontListTextMeshPro.IndexOf(font) + 1; // Gets the index of the font in the font list of the fontListObject object.

        // If the font is present in the list, set the flag to indicate that the font information should be saved.
        if (index > 0)
        {
            dFont = index;
            dFontWrite = true;
        }
    }

    // Method for saving text information.
    private void SavaLText()
    {
        LanguageTextTextMeshPro languageText = targetText.GetComponent<LanguageTextTextMeshPro>(); // Gets the LanguageTextTextMeshPro component of the target object.

        ID = languageText.value; // Gets the ID of the LanguageText.

        RectTransform rectTransform = targetText.GetComponent<RectTransform>(); // Gets the RectTransform component of the target object.

        // Gets the position and size information of the target object's RectTransform.
        PosX = rectTransform.anchoredPosition.x;
        PosY = rectTransform.anchoredPosition.y;
        Width = rectTransform.rect.width;
        Height = rectTransform.rect.height;

        // Sets flags to indicate that position and size information should be saved.
        PosXWrite = true;
        PosYWrite = true;
        WidthWrite = true;
        HeightWrite = true;

        TMP_Text text = targetText.GetComponent<TMP_Text>(); // Gets the Text component of the target object.

        Test = text.text; // Gets the text from the Text component of the target object.
        Size = text.fontSize; // Gets the font size of the Text component of the target object.

        // Sets flags to indicate that text and font size information should be saved.
        TestWrite = true;
        SizeWrite = true;

        // Gets character order reversal information.
        if (reverse == Reverse.Default)
        {
            ReverseValue = 0f;
        }
        else if (reverse == Reverse.False)
        {
            ReverseValue = 1f;
        }
        else if (reverse == Reverse.True)
        {
            ReverseValue = 2f;
        }
        ReverseWrite = true; // Sets the flag to indicate that inversion information should be saved.
        
        // Gets alignment information from the Text component of the target object.
        if (text.alignment == TextAlignmentOptions.Left || text.alignment == TextAlignmentOptions.TopLeft || text.alignment == TextAlignmentOptions.BottomLeft)
        {
            Alignment = 1;
        }
        else if (text.alignment == TextAlignmentOptions.Center || text.alignment == TextAlignmentOptions.Top || text.alignment == TextAlignmentOptions.Bottom)
        {
            Alignment = 2;
        }
        else if (text.alignment == TextAlignmentOptions.Right || text.alignment == TextAlignmentOptions.TopRight || text.alignment == TextAlignmentOptions.BottomRight)
        {
            Alignment = 3;
        }
        AlignmentWrite = true; // Sets the flag to indicate that alignment information should be saved.

        TMP_FontAsset font = text.font; // Gets the font used by the Text component of the target object.

        int index = FontListTMPro.FontListTextMeshPro.IndexOf(font) + 1; // Gets the index of the font in the font list of the fontListObject object.

        // If the font is present in the list, set the flag to indicate that the font information should be saved.
        if (index > 0)
        {
            Font = index;
            FontWrite = true;
        }
    }

    // Method to save the InputField information.
    private void SavaLInputField()
    {
        LanguageTextInputFieldTextMeshPro languageTextInputField = IFtargetText.GetComponent<LanguageTextInputFieldTextMeshPro>(); // Gets the LanguageTextInputFieldTextMeshPro component of the target object.

        IFID = languageTextInputField.value; // Gets the ID of the LanguageTextInputField.

        TMP_Text text = IFtargetText.GetComponent<TMP_Text>(); // Gets the Text component of the target object.

        IFSize = text.fontSize; // Gets the font size of the Text component of the target object.
        IFSizeWrite = true; // Indicate that font size information is to be saved.

        // Gets alignment information from the Text component of the target object.
        if (text.alignment == TextAlignmentOptions.Left || text.alignment == TextAlignmentOptions.TopLeft || text.alignment == TextAlignmentOptions.BottomLeft)
        {
            IFAlignment = 1;
        }
        else if (text.alignment == TextAlignmentOptions.Center || text.alignment == TextAlignmentOptions.Top || text.alignment == TextAlignmentOptions.Bottom)
        {
            IFAlignment = 2;
        }
        else if (text.alignment == TextAlignmentOptions.Right || text.alignment == TextAlignmentOptions.TopRight || text.alignment == TextAlignmentOptions.BottomRight)
        {
            IFAlignment = 3;
        }
        IFAlignmentWrite = true; // Sets the flag to indicate that alignment information should be saved.

        TMP_FontAsset font = text.font; // Gets the font used by the Text component of the target object.

        int index = FontListTMPro.FontListTextMeshPro.IndexOf(font) + 1; // Gets the index of the font in the font list of the fontListObject object.

        // If the font is present in the list, set the flag to indicate that the font information should be saved.
        if (index > 0)
        {
            IFFont = index;
            IFFontWrite = true;
        }
    }

    // Método para salvar as informações do TextMesh.
    private void SavaLTextMesh()
    {
        LanguageTextMeshTextMeshPro languageTextMesh = targetText.GetComponent<LanguageTextMeshTextMeshPro>(); // Gets the LanguageTextMeshTextMeshPro component of the target object.

        TMID = languageTextMesh.value; // Gets the ID of the LanguageTextMesh.

        TMP_Text textMesh = targetText.GetComponent<TMP_Text>(); // Gets the TextMesh component of the target object.

        TMTest = textMesh.text; // Gets the text from the TextMesh component of the target object.
        TMSize = textMesh.fontSize; // Gets the font size of the TextMesh component of the target object.

        // Sets flags to indicate that text and font size information should be saved.
        TMTestWrite = true;
        TMSizeWrite = true;

        // Gets character order reversal information.
        if (TMreverse == Reverse.Default)
        {
            TMReverseValue = 0f;
        }
        else if (TMreverse == Reverse.False)
        {
            TMReverseValue = 1f;
        }
        else if (TMreverse == Reverse.True)
        {
            TMReverseValue = 2f;
        }
        TMReverseWrite = true; // Sets the flag to indicate that inversion information should be saved.

        TMP_FontAsset font = textMesh.font; // Gets the font used by the TextMesh component of the target object.

        int index = FontListTMPro.FontListTextMeshPro.IndexOf(font) + 1; // Gets the index of the font in the font list of the fontListObject object.

        // If the font is present in the list, set the flag to indicate that the font information should be saved.
        if (index > 0)
        {
            TMFont = index;
            TMFontWrite = true;
        }
    }
}

[System.Serializable]
public class SaveLegacyLanguageTextMeshPro
{
    public string SelectedText; // Selected text.
    public float value = 0; // ID of the text to be displayed.
}

[System.Serializable]
public class SaveLegacyLanguageSelectedTextMeshPro
{
    public string SelectedText; // Text to be placed.
    public float value = 0; // ID of the text to be displayed.
}
