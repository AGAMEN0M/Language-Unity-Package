using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.SceneManagement;

public class Canvas_LT_Legacy : MonoBehaviour
{
    [MenuItem("GameObject/Language/UI/Legacy/Language Dropdown (LT Legacy)")]
    public static void Create1(MenuCommand menuCommand)
    {
        // Looks for a Canvas object in the scene.
        Canvas canvasObject = Object.FindObjectOfType<Canvas>();

        if (canvasObject == null)
        {
            // If not found, it creates a new Canvas object in the scene.
            GameObject newCanvasObject = new GameObject("Canvas");
            canvasObject = newCanvasObject.AddComponent<Canvas>();
            newCanvasObject.AddComponent<CanvasScaler>();
            newCanvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.sortingOrder = 0;
            canvasObject.targetDisplay = 0;

            // Defines the layer of the Canvas object.
            canvasObject.gameObject.layer = LayerMask.NameToLayer("UI");

            // Creates an EventSystem object to handle user interaction.
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/Legacy/UI/Language Dropdown (LT Legacy).prefab";

        // Loads the prefab from the specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Check if an object is selected inside the Canvas.
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(canvasObject.transform))
        {
            // Creates a new prefab from the existing prefab as a child of the selected object.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, selectedObject.transform) as GameObject;
            newPrefab.name = "Language Dropdown (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        else
        {
            // Creates a new prefab from the existing prefab as a child of the currently selected Canvas.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, canvasObject.transform) as GameObject;
            newPrefab.name = "Language Dropdown (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/Language/UI/Legacy/Text (LT Legacy)")]
    public static void Create2(MenuCommand menuCommand)
    {
        // Looks for a Canvas object in the scene.
        Canvas canvasObject = Object.FindObjectOfType<Canvas>();

        if (canvasObject == null)
        {
            // If not found, it creates a new Canvas object in the scene.
            GameObject newCanvasObject = new GameObject("Canvas");
            canvasObject = newCanvasObject.AddComponent<Canvas>();
            newCanvasObject.AddComponent<CanvasScaler>();
            newCanvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.sortingOrder = 0;
            canvasObject.targetDisplay = 0;

            // Defines the layer of the Canvas object.
            canvasObject.gameObject.layer = LayerMask.NameToLayer("UI");

            // Creates an EventSystem object to handle user interaction.
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/Legacy/UI/Text (LT Legacy).prefab";

        // Loads the prefab from the specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Check if an object is selected inside the Canvas.
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(canvasObject.transform))
        {
            // Creates a new prefab from the existing prefab as a child of the selected object.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, selectedObject.transform) as GameObject;
            newPrefab.name = "Text (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        else
        {
            // Creates a new prefab from the existing prefab as a child of the currently selected Canvas.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, canvasObject.transform) as GameObject;
            newPrefab.name = "Text (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/Language/UI/Legacy/Toggle (LT Legacy)")]
    public static void Create3(MenuCommand menuCommand)
    {
        // Looks for a Canvas object in the scene.
        Canvas canvasObject = Object.FindObjectOfType<Canvas>();

        if (canvasObject == null)
        {
            // If not found, it creates a new Canvas object in the scene.
            GameObject newCanvasObject = new GameObject("Canvas");
            canvasObject = newCanvasObject.AddComponent<Canvas>();
            newCanvasObject.AddComponent<CanvasScaler>();
            newCanvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.sortingOrder = 0;
            canvasObject.targetDisplay = 0;

            // Defines the layer of the Canvas object.
            canvasObject.gameObject.layer = LayerMask.NameToLayer("UI");

            // Creates an EventSystem object to handle user interaction.
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/Legacy/UI/Toggle (LT Legacy).prefab";

        // Loads the prefab from the specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Check if an object is selected inside the Canvas.
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(canvasObject.transform))
        {
            // Creates a new prefab from the existing prefab as a child of the selected object.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, selectedObject.transform) as GameObject;
            newPrefab.name = "Toggle (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        else
        {
            // Creates a new prefab from the existing prefab as a child of the currently selected Canvas.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, canvasObject.transform) as GameObject;
            newPrefab.name = "Toggle (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/Language/UI/Legacy/Button (LT Legacy)")]
    public static void Create4(MenuCommand menuCommand)
    {
        // Looks for a Canvas object in the scene.
        Canvas canvasObject = Object.FindObjectOfType<Canvas>();

        if (canvasObject == null)
        {
            // If not found, it creates a new Canvas object in the scene.
            GameObject newCanvasObject = new GameObject("Canvas");
            canvasObject = newCanvasObject.AddComponent<Canvas>();
            newCanvasObject.AddComponent<CanvasScaler>();
            newCanvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.sortingOrder = 0;
            canvasObject.targetDisplay = 0;

            // Defines the layer of the Canvas object.
            canvasObject.gameObject.layer = LayerMask.NameToLayer("UI");

            // Creates an EventSystem object to handle user interaction.
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/Legacy/UI/Button (LT Legacy).prefab";

        // Loads the prefab from the specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Check if an object is selected inside the Canvas.
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(canvasObject.transform))
        {
            // Creates a new prefab from the existing prefab as a child of the selected object.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, selectedObject.transform) as GameObject;
            newPrefab.name = "Button (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        else
        {
            // Creates a new prefab from the existing prefab as a child of the currently selected Canvas.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, canvasObject.transform) as GameObject;
            newPrefab.name = "Button (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/Language/UI/Legacy/Dropdown (LT Legacy)")]
    public static void Create5(MenuCommand menuCommand)
    {
        // Looks for a Canvas object in the scene.
        Canvas canvasObject = Object.FindObjectOfType<Canvas>();

        if (canvasObject == null)
        {
            // If not found, it creates a new Canvas object in the scene.
            GameObject newCanvasObject = new GameObject("Canvas");
            canvasObject = newCanvasObject.AddComponent<Canvas>();
            newCanvasObject.AddComponent<CanvasScaler>();
            newCanvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.sortingOrder = 0;
            canvasObject.targetDisplay = 0;

            // Defines the layer of the Canvas object.
            canvasObject.gameObject.layer = LayerMask.NameToLayer("UI");

            // Creates an EventSystem object to handle user interaction.
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/Legacy/UI/Dropdown (LT Legacy).prefab";

        // Loads the prefab from the specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Check if an object is selected inside the Canvas.
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(canvasObject.transform))
        {
            // Creates a new prefab from the existing prefab as a child of the selected object.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, selectedObject.transform) as GameObject;
            newPrefab.name = "Dropdown (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        else
        {
            // Creates a new prefab from the existing prefab as a child of the currently selected Canvas.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, canvasObject.transform) as GameObject;
            newPrefab.name = "Dropdown (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/Language/UI/Legacy/InputField (LT Legacy)")]
    public static void Create6(MenuCommand menuCommand)
    {
        // Looks for a Canvas object in the scene.
        Canvas canvasObject = Object.FindObjectOfType<Canvas>();

        if (canvasObject == null)
        {
            // If not found, it creates a new Canvas object in the scene.
            GameObject newCanvasObject = new GameObject("Canvas");
            canvasObject = newCanvasObject.AddComponent<Canvas>();
            newCanvasObject.AddComponent<CanvasScaler>();
            newCanvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.sortingOrder = 0;
            canvasObject.targetDisplay = 0;

            // Defines the layer of the Canvas object.
            canvasObject.gameObject.layer = LayerMask.NameToLayer("UI");

            // Creates an EventSystem object to handle user interaction.
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/Legacy/UI/InputField (LT Legacy).prefab";

        // Loads the prefab from the specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Check if an object is selected inside the Canvas.
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(canvasObject.transform))
        {
            // Creates a new prefab from the existing prefab as a child of the selected object.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, selectedObject.transform) as GameObject;
            newPrefab.name = "InputField (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        else
        {
            // Creates a new prefab from the existing prefab as a child of the currently selected Canvas.
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(originalPrefab, canvasObject.transform) as GameObject;
            newPrefab.name = "InputField (LT Legacy)";

            // Unpacks the created prefab.
            PrefabUtility.UnpackPrefabInstance(newPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // Indicate that the scene has been modified.
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }    
}