using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace StudioXRCL.EscapeRoom.XR
{
    /// <summary>
    /// Applies a positional camera shake effect suitable for VR,
    /// optionally triggering controller haptics and audio playback.
    /// </summary>
    public class CameraShaker : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Shake Settings")]
        [Tooltip("Duration of the shake effect in seconds.")]
        [SerializeField] private float duration = 3.0f;

        [Tooltip("Intensity of the shake. Keep low for VR comfort (recommended: 0.01–0.1).")]
        [SerializeField] private float magnitude = 0.015f;

        [Header("Haptics")]
        [Tooltip("Intensity of controller vibration.")]
        [SerializeField] private float hapticIntensity = 0.8f;

        [Header("Audio")]
        [Tooltip("Audio source played during the shake effect.")]
        [SerializeField] private AudioSource earthquakeAudio;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Cached original local position of the camera.
        /// </summary>
        private Vector3 _originalLocalPosition;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Automatically triggers the shake effect when the object spawns.
        /// </summary>
        private void Start()
        {
            TriggerEarthquake();
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Triggers the camera shake, audio playback, and controller haptics.
        /// </summary>
        public void TriggerEarthquake()
        {
            _originalLocalPosition = transform.localPosition;
            StartCoroutine(ShakeRoutine());

            if (earthquakeAudio != null)
            {
                earthquakeAudio.Play();
            }

            TriggerHaptics(XRNode.LeftHand);
            TriggerHaptics(XRNode.RightHand);
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Coroutine that applies positional camera shake over time.
        /// Rotation is intentionally not modified to avoid VR discomfort.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator ShakeRoutine()
        {
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                float offsetY = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(
                    _originalLocalPosition.x + offsetX,
                    _originalLocalPosition.y + offsetY,
                    _originalLocalPosition.z
                );

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _originalLocalPosition;
        }

        /// <summary>
        /// Triggers haptic feedback on the specified XR controller.
        /// </summary>
        /// <param name="node">The XRNode (hand) to trigger haptics on.</param>
        private void TriggerHaptics(XRNode node)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);

            if (!device.isValid)
                return;

            device.SendHapticImpulse(0, hapticIntensity, duration);
        }

        #endregion
    }
}
