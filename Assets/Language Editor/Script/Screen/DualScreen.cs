using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DualScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Button> Button; // List of buttons to activate if only one monitor is detected.
    [Space(5)]
    [SerializeField] private Canvas canvas1; // Canvas 1 to be displayed on monitor 1.
    [SerializeField] private Canvas canvas2; // Canvas 2 to be displayed on monitor 2.

    private int targetDisplay1 = 0; // Target Monitor ID for Canvas 1.
    private int targetDisplay2 = 1; // Target Monitor ID for Canvas 2.

    void Start()
    {
        // Check if there is more than one monitor.
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate(); // Activate monitor 2.

            // Sets monitor resolution to screen resolution and disables full screen mode.
            for (int i = 0; i < Display.displays.Length; i++)
            {
                Display.displays[i].SetRenderingResolution(Screen.width, Screen.height);
                Screen.fullScreen = false;
            }
        }
        else
        {
            // Activates all buttons in the Button list.
            foreach (Button button in Button)
            {
                button.gameObject.SetActive(true);
            }

            Debug.LogError("Could not find a second monitor. Alternate options enabled."); // Displays an error message stating that only one monitor was found.
        }
    }

    public void ChangeCanvas()
    {
        int temp = targetDisplay1; // Stores the Canvas 1's current target monitor ID.
        targetDisplay1 = targetDisplay2; // Sets the Canvas 1 target monitor ID to the Canvas 2 target monitor ID.
        targetDisplay2 = temp; // Sets the Canvas 2 target monitor ID to the previously stored ID.

        canvas1.targetDisplay = targetDisplay1; // Sets the target monitor to Canvas 1.
        canvas2.targetDisplay = targetDisplay2; // Sets the target monitor for Canvas 2.
    }
}