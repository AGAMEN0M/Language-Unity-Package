using System.Collections.Generic;
using UnityEngine;

public class ButtonsMiddleCenter : MonoBehaviour
{
    [Header("Middle/Center Settings")]
    [SerializeField] private List<RectTransform> objectsToAlign; // A list of RectTransform objects to be aligned.

    // Functions to align the objects to different positions.
    public void ButtonLeftTop()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
        }
    }

    public void ButtonCenterTop()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
        }
    }

    public void ButtonRightTop()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
        }
    }

    public void ButtonLeftMiddle()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
        }
    }

    public void ButtonCenterMiddle()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }

    public void ButtonRightMiddle()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(1, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
        }
    }

    public void ButtonLeftBottom()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
        }
    }

    public void ButtonCenterBottom()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
        }
    }

    public void ButtonRightBottom()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
        }
    }

    // Functions to align the objects to stretch along one axis.
    public void ButtonTopStretch1()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
        }
    }

    public void ButtonMiddleStretch1()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
        }
    }

    public void ButtonBottomStretch1()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
        }
    }

    public void ButtonStretch2Left()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 1);
        }
    }

    public void ButtonStretch2Center()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
        }
    }

    public void ButtonStretch2Right()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }
    }

    public void ButtonStretch2Stretch1()
    {
        foreach (RectTransform rectTransform in objectsToAlign)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }
    }
}