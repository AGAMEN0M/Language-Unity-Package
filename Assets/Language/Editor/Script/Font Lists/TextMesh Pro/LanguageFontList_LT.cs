using UnityEngine;
using UnityEditor;

public static class LanguageFontList_LT
{
    [MenuItem("GameObject/Language/Font Lists/LanguageFontList (LT)")]
    public static void Create(MenuCommand menuCommand)
    {
        // Manually added prefab path.
        string prefabPath = "Assets/Language/Prefab/TextMesh Pro/LanguageFontList (LT).prefab";

        // Load prefab from specified path.
        GameObject originalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (originalPrefab == null)
        {
            Debug.LogError("Could not find the original prefab.");
            return;
        }

        // Get the currently selected game object.
        GameObject selectedGameObject = Selection.activeGameObject;

        // Create the new game object as a child of the selected game object, if there is one.
        GameObject newGameObject;
        if (selectedGameObject != null)
        {
            newGameObject = PrefabUtility.InstantiatePrefab(originalPrefab, selectedGameObject.transform) as GameObject;
        }
        else
        {
            newGameObject = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
        }

        // Rename the new game object.
        newGameObject.name = "LanguageFontList (LT)";
    }
}