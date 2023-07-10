using UnityEngine;

public class OpenLanguageFolder : MonoBehaviour
{
    [Header("Archives Location")]
    #pragma warning disable CS0414
    [SerializeField] private string FolderNameInUnity = "/StreamingAssets/Language/";
    [SerializeField] private string FolderNameInBuild = "/StreamingAssets/Language/";
    #pragma warning restore CS0414

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