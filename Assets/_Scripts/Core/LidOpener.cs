using System.Collections;
using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles smoothly opening a lid by rotating it over time to a target Z rotation.
    /// </summary>
    public class LidOpener : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Lid Settings")]

        [Tooltip("Duration in seconds for the lid to fully open.")]
        public float openDuration = 1f;

        [Tooltip("Target Z-axis rotation (in degrees) when the lid is fully open.")]
        public float endZ = -110f;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Reference to the currently running lid-opening coroutine.
        /// </summary>
        private Coroutine _openRoutine;

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Starts opening the lid. If an open routine is already running, it is restarted.
        /// </summary>
        public void Open()
        {
            if (_openRoutine != null)
            {
                StopCoroutine(_openRoutine);
            }

            _openRoutine = StartCoroutine(OpenLidRoutine());
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Coroutine that smoothly rotates the lid from its current rotation
        /// to the target open rotation over the configured duration.
        /// </summary>
        /// <returns>IEnumerator used by Unity's coroutine system.</returns>
        private IEnumerator OpenLidRoutine()
        {
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(
                startRotation.eulerAngles.x,
                startRotation.eulerAngles.y,
                endZ
            );

            float elapsed = 0f;

            while (elapsed < openDuration)
            {
                elapsed += Time.deltaTime;

                float t = openDuration > 0f ? elapsed / openDuration : 1f;
                t = Mathf.Clamp01(t);

                transform.rotation = Quaternion.Slerp(
                    startRotation,
                    endRotation,
                    t
                );

                yield return null;
            }

            _openRoutine = null;
        }

        #endregion
    }
}
