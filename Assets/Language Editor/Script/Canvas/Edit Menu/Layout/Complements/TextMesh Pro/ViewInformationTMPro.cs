using System;
using UnityEngine;
using TMPro;

public class ViewInformationTMPro : MonoBehaviour
{
    // Declaration of public and private variables.
    [Header("Settings")]
    [SerializeField] private TMP_Text targetText;
    [Space(10)]
    [Header("Text Settings")]
    public string Text;
    public int FontSize;
    public float X;
    public float Y;
    public float Width;
    public float Height;
    public int Font;
    [SerializeField] private LanguageFontListTextMeshPro FontListTextMeshProObject;
    public int Alignment;
    public int ReverseIndex;
    [SerializeField] private bool Reverse = false;
    [Space(10)]
    [Header("Old Text Settings")]
    [SerializeField] private string oldText;
    [SerializeField] private float oldFontSize;
    [SerializeField] private float oldX;
    [SerializeField] private float oldY;
    [SerializeField] private float oldWidth;
    [SerializeField] private float oldHeight;
    [SerializeField] private int oldAlignment;

    // Method executed at the beginning of script execution.
    private void Awake()
    {
        // Saves the original values of the Target Text (targetText).
        oldText = targetText.text;
        oldFontSize = targetText.fontSize;
        oldX = targetText.rectTransform.anchoredPosition.x;
        oldY = targetText.rectTransform.anchoredPosition.y;
        oldWidth = targetText.rectTransform.sizeDelta.x;
        oldHeight = targetText.rectTransform.sizeDelta.y;

        // Saves the original text alignment setting.
        if (targetText.alignment == TextAlignmentOptions.Left)
        {
            oldAlignment = 1;
        }
        else if (targetText.alignment == TextAlignmentOptions.Center)
        {
            oldAlignment = 2;
        }
        else if (targetText.alignment == TextAlignmentOptions.Right)
        {
            oldAlignment = 3;
        }
    }

    // Method executed every frame (frame) of the game execution.
    public void Update()
    {
        Size(); // Calls the Size() method to update the target Text.
    }

    // Method responsible for updating the properties of the Target Text (targetText).
    private void Size()
    {
        // Checks if the Text property has been set.
        if (Text != "")
        {
            // If Reverse is true, reverses the character order in the Text property.
            if (Reverse == true)
            {
                char[] chars = Text.ToCharArray();
                Array.Reverse(chars);
                targetText.text = new string(chars);
            }
            else // Otherwise, sets the Text directly.
            {
                targetText.text = Text;
            }
        }
        else // If the Text property has not been set, use the original value saved in oldText.
        {
            // If Reverse is true, reverses the order of characters in oldText.
            if (Reverse == true)
            {
                char[] chars = oldText.ToCharArray();
                Array.Reverse(chars);
                targetText.text = new string(chars);
            }
            else // Otherwise, sets the Original Text.
            {
                targetText.text = oldText;
            }
        }

        // Checks if the FontSize property has been set.
        if (FontSize != 0)
        {
            targetText.fontSize = FontSize; // Sets the font size.
        }
        else // Otherwise, keep the previous font size.
        {
            targetText.fontSize = oldFontSize;
        }

        // Checks that the X and Y properties have been set.
        if (X != 0 || Y != 0)
        {
            // Sets the position of the anchored rectangle to which the text is attached.
            RectTransform rectTransform = targetText.rectTransform;
            rectTransform.anchoredPosition = new Vector2(X, Y);
        }
        else // Otherwise, it maintains the previous position.
        {
            RectTransform rectTransform = targetText.rectTransform;
            rectTransform.anchoredPosition = new Vector2(oldX, oldY);
        }

        // Checks if the Width and Height properties have been set.
        if (Width != 0 || Height != 0)
        {
            // Sets the width and height of the anchored rectangle to which the text is attached.
            RectTransform rectTransform = targetText.rectTransform;
            Vector2 newSize = new Vector2(Width, Height);
            rectTransform.sizeDelta = newSize;
        }
        else // Otherwise, it keeps the previous width and height.
        {
            RectTransform rectTransform = targetText.rectTransform;
            Vector2 newSize = new Vector2(oldWidth, oldHeight);
            rectTransform.sizeDelta = newSize;
        }

        // Checks if the Font property has been set.
        if (Font != 0)
        {
            // Checks that the list of fonts is present and that the Font property is a valid value.
            if (FontListTextMeshProObject != null && Font > 0 && Font <= FontListTextMeshProObject.FontListTextMeshPro.Count)
            {
                targetText.font = FontListTextMeshProObject.FontListTextMeshPro[Font - 1]; // Sets the text font based on the font list.
            }
        }
        else // Otherwise, keep the previous source.
        {
            targetText.font = FontListTextMeshProObject.FontListTextMeshPro[0];
        }

        // Checks if the Alignment property has been set.
        if (Alignment != 0)
        {
            // Sets text alignment based on the value of the Alignment property.
            if (Alignment == 1)
            {
                targetText.alignment = TextAlignmentOptions.Left;
            }
            else if (Alignment == 2)
            {
                targetText.alignment = TextAlignmentOptions.Center;
            }
            else if (Alignment == 3)
            {
                targetText.alignment = TextAlignmentOptions.Right;
            }
        }
        else // Otherwise, keep the previous alignment.
        {
            if (oldAlignment == 1)
            {
                targetText.alignment = TextAlignmentOptions.Left;
            }
            else if (oldAlignment == 2)
            {
                targetText.alignment = TextAlignmentOptions.Center;
            }
            else if (oldAlignment == 3)
            {
                targetText.alignment = TextAlignmentOptions.Right;
            }
        }

        // Checks if the ReverseIndex property has been set.
        if (ReverseIndex != 0)
        {
            // Sets the Reverse property value based on the ReverseIndex property value.
            if (ReverseIndex == 1)
            {
                Reverse = false;
            }

            if (ReverseIndex == 2)
            {
                Reverse = true;
            }
        }
        else // Otherwise, it keeps the previous value of the Reverse property.
        {
            Reverse = false;
        }
    }
}