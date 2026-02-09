using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Provides a simple interface for changing Unity scenes,
    /// with optional fallback to a default scene.
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {
        #region Private Variable Declarations

        [Header("Scene Settings")]
        [Tooltip("Name of the scene to load when no scene name is provided.")]
        [SerializeField] private string defaultSceneName;

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Changes the active scene to the provided scene name.
        /// If the provided name is invalid, the default scene is loaded instead.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void ChangeScene(string sceneName)
        {
            if (!string.IsNullOrWhiteSpace(sceneName))
            {
                SceneManager.LoadScene(sceneName);
                return;
            }

            if (!string.IsNullOrWhiteSpace(defaultSceneName))
            {
                SceneManager.LoadScene(defaultSceneName);
            }
            else
            {
                Debug.LogWarning(
                    $"{nameof(SceneChanger)}: No valid scene name provided and no default scene configured."
                );
            }
        }

        #endregion
    }
}
