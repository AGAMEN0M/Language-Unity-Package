using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganizeIDS : MonoBehaviour
{
    [Header("Settings")]
    public float ID = 0; // The order number of this object.
    public bool IncludeInOrder = true; // Whether this object should be included in the organization.
    [Space(10)]
    [SerializeField] private OrganizeIDS[] ObjectsToOrganize; // Private variable that stores all objects to be sorted.
    [Space(10)]
    [Header("Components")]
    public Text NumberID; // Reference to the text component to display the order number.
    public InputField text; // Reference to the text field component to edit the text.
    public InputField FontSize; // Reference to Text Field component to edit font size.
    public InputField PosX; // Reference to text field component to edit X position.
    public InputField PosY; // Reference to the text field component to edit the Y position.
    public InputField Width; // Reference to the Text Field component to edit the width.
    public InputField Height; // Reference to the text field component to edit the height.
    public InputField Font; // Reference to the text field component to edit the font.
    public Dropdown Alignment; // Reference to dropdown component for editing alignment.
    public Dropdown Reverse; // Reference to dropdown menu component to reverse sort order.
    [Space(10)]
    [Header("Information")]
    public float TextID = 0f; // Order number of this object in float form.
    public bool InteractableText = true; // Whether the text component is interactive.
    public bool InteractableFontSize = true; // Whether the font size component is interactive.
    public bool InteractablePosX = true; // Whether the X position component is interactive.
    public bool InteractablePosY = true; // Whether the Y position component is interactive.
    public bool InteractableWidth = true; // Whether the width component is interactive.
    public bool InteractableHeight = true; // Whether the height component is interactive.
    public bool InteractableFont = true; // Whether the font component is interactive.
    public bool InteractableAlignment = true; // Whether the alignment component is interactive.
    public int AlignmentValue = 0; // Value selected in alignment dropdown component.
    public bool InteractableReverse = true; // Whether the order reversal component is interactive.
    public int ReverseValue = 0; // Value selected in reverse order drop-down component.

    void Start()
    {
        ID = TextID; // Assigns the sort number in float form to the sort number variable.
        information(); // Calls the function to display information about the object.
        ID_Organize(); // Calling the function to arrange the objects.
    }

    // Function for sorting objects based on the ID variable.
    public void ID_Organize()
    {
        Array.Clear(ObjectsToOrganize, 0, ObjectsToOrganize.Length); // Clears the array before populating it again.
        ObjectsToOrganize = FindObjectsOfType<OrganizeIDS>(); // Finds all objects of type OrganizeIDS in the scene.

        // Creates a list from the array of found objects and sorts them based on the order number.
        List<OrganizeIDS> orderedObjects = new List<OrganizeIDS>(ObjectsToOrganize);
        orderedObjects.Sort((obj1, obj2) => obj1.ID.CompareTo(obj2.ID));

        // Loops through all sorted objects and updates the hierarchy based on order.
        foreach (OrganizeIDS obj in orderedObjects)
        {
            if (obj.IncludeInOrder) // Whether this object should be included in the organization.
            {
                obj.transform.SetSiblingIndex(orderedObjects.IndexOf(obj)); // Sets the object's position in the hierarchy based on order.
            }
        }
    }

    // Method for updating info panel fields with information for the current instance.
    void information()
    {
        NumberID.text = TextID.ToString() + ";"; // Sets the text of the NumberID Variable Text component to the current ID.
        text.interactable = InteractableText; // Defines whether the InputField component of the Text variable should be interactive or not.
        FontSize.interactable = InteractableFontSize; // Defines whether the InputField component of the FontSize variable should be interactive or not.
        PosX.interactable = InteractablePosX; // Defines whether the InputField component of the PosX variable should be interactive or not.
        PosY.interactable = InteractablePosY; // Defines whether the InputField component of the PosY variable should be interactive or not.
        Width.interactable = InteractableWidth; // Defines whether the InputField component of the Width variable should be interactive or not.
        Height.interactable = InteractableHeight; // Defines whether the InputField component of the Height variable should be interactive or not.
        Font.interactable = InteractableFont; // Defines whether the InputField component of the Font variable should be interactive or not.
        Alignment.interactable = InteractableAlignment; // Defines whether the Dropdown component of the Alignment variable should be interactive or not.
        Alignment.value = AlignmentValue; // Sets the selected value of the Dropdown component of the Alignment variable.
        Reverse.interactable = InteractableReverse; // Defines whether the Dropdown component of the Reverse variable should be interactive or not.
        Reverse.value = ReverseValue; // Sets the selected value of the Dropdown component of the Reverse variable.
    }

    // Method called when the Dropdown Alignment value is changed by the user.
    public void OnDropdownValueChangedAlignment()
    {
        AlignmentValue = Alignment.value; // Stores the current Dropdown Alignment value in the AlignmentValue variable.
    }

    // Method called when the Dropdown Reverse value changes.
    public void OnDropdownValueChangedReverse()
    {
        ReverseValue = Reverse.value; // Updates the ReverseValue variable with the value selected by the user in the Reverse Dropdown.
    }

    public void interacting()
    {
        // Finds all objects with the ViewInformation component.
        ViewInformation[] componentesViewInformation = FindObjectsOfType<ViewInformation>();

        // Iterates over each object found.
        foreach (ViewInformation componente in componentesViewInformation)
        {
            componente.Text = text.text; // Assigns the desired text to the ViewInformation component of the current object.

            // Converts input box text to int or float and assigns it to the ViewInformation component of the current object.
            int.TryParse(FontSize.text, out componente.FontSize);
            float.TryParse(PosX.text, out componente.X);
            float.TryParse(PosY.text, out componente.Y);
            float.TryParse(Width.text, out componente.Width);
            float.TryParse(Height.text, out componente.Height);
            int.TryParse(Font.text, out componente.Font);
            componente.Alignment = AlignmentValue;
            componente.ReverseIndex = ReverseValue;
        }

        // Finds all objects with the ViewInformationTMPro component.
        ViewInformationTMPro[] componentesViewInformationTMPro = FindObjectsOfType<ViewInformationTMPro>();

        // Iterates over each object found.
        foreach (ViewInformationTMPro componente in componentesViewInformationTMPro)
        {
            componente.Text = text.text; // Assigns the desired text to the ViewInformationTMPro component of the current object.

            // Converts input box text to int or float and assigns it to the ViewInformationTMPro component of the current object.
            int.TryParse(FontSize.text, out componente.FontSize);
            float.TryParse(PosX.text, out componente.X);
            float.TryParse(PosY.text, out componente.Y);
            float.TryParse(Width.text, out componente.Width);
            float.TryParse(Height.text, out componente.Height);
            int.TryParse(Font.text, out componente.Font);
            componente.Alignment = AlignmentValue;
            componente.ReverseIndex = ReverseValue;
        }
    }
}