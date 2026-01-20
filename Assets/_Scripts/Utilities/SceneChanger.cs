using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load when ChangeScene is called without parameters.")]
    [SerializeField] private string defaultSceneName;

    /// <summary>
    /// Changes the active scene to the provided scene name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void ChangeScene(string sceneName)
    {
        if (sceneName is { Length: > 0 })
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        if (defaultSceneName is { Length: > 0 })
        {
            SceneManager.LoadScene(defaultSceneName);
        }
    }
}
