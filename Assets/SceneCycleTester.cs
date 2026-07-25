using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Automatically creates itself when the game starts and allows cycling
/// through all scenes in the Build Settings by pressing the R key.
/// </summary>
public class SceneCycleTester : MonoBehaviour
{
    /// <summary>
    /// Creates the tester automatically before the first scene loads.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var obj = new GameObject(nameof(SceneCycleTester));
        DontDestroyOnLoad(obj);

        obj.AddComponent<SceneCycleTester>();
    }

    /// <summary>
    /// Checks for the R key every frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) LoadNextScene();
    }

    /// <summary>
    /// Loads the next scene from the Build Settings.
    /// Wraps back to the first scene after the last one.
    /// </summary>
    private void LoadNextScene()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        if (sceneCount <= 1)
        {
            Debug.LogWarning("There are not enough scenes in the Build Settings.");
            return;
        }

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene >= sceneCount) nextScene = 0;

        Debug.Log($"Loading scene {nextScene}...");
        SceneManager.LoadScene(nextScene);
    }
}