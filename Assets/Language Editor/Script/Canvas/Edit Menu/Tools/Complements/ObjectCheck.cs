using UnityEngine;
using UnityEngine.UI;

public class ObjectCheck : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject parentObject;
    [Space(5)]
    [SerializeField] private bool hasChildObject = false;
    [SerializeField] private Button[] buttons;

    private void Update()
    {
        hasChildObject = parentObject.transform.childCount > 0; // Checks whether the parent object has a child object.

        // Enables or disables button interaction based on the bool.
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = hasChildObject;
        }
    }
}