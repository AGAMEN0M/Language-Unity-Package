using UnityEngine;
using UnityEngine.UI;


public class DropdownActivatObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private GameObject[] objectsToActivate;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        // We deactivate all objects in the list before activating the corresponding object.
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }

        // We activate the object corresponding to the index of the selected option.
        if (index >= 0 && index < objectsToActivate.Length)
        {
            objectsToActivate[index].SetActive(true);
        }
    }
}