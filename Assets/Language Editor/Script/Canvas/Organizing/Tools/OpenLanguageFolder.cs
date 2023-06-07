using UnityEngine;

public class OpenLanguageFolder : MonoBehaviour
{
    [Header("Archives Location")]
    [SerializeField] private string FolderNameInUnity = "/StreamingAssets/Language/";
    [SerializeField] private string FolderNameInBuild = "/StreamingAssets/Language/";

    public void openFolder()
    {
    #if UNITY_EDITOR
        string Path = Application.dataPath + FolderNameInUnity;
    #else
        string Path = Application.dataPath + FolderNameInBuild;
    #endif

        Application.OpenURL(Path);
    }
}