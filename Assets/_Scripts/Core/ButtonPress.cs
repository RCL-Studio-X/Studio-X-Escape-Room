using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles physical button press interactions using trigger collisions.
    /// Moves the button visually when pressed and invokes press/release events.
    /// </summary>
    public class ButtonPress : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Button Events")]

        [Tooltip("Event invoked when the button is pressed.")]
        public UnityEvent onPress;

        [Tooltip("Event invoked when the button is released.")]
        public UnityEvent onRelease;

        [Header("Button Settings")]

        [Tooltip("How far the button moves downward when pressed (in meters).")]
        public float pressDepth = 0.02f;

        #endregion

        #region Private Variable Declarations

        /// <summary> Reference to the object currently pressing the button. </summary>
        private GameObject _presser;

        /// <summary> Whether the button is currently pressed. </summary>
        private bool _isPressed = false;

        /// <summary> The starting local position of the button. </summary>
        private Vector3 _startLocalPosition;

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Unity Start method. Caches the initial local position of the button.
        /// </summary>
        private void Start()
        {
            _startLocalPosition = transform.localPosition;
        }

        /// <summary>
        /// Called when another collider enters this trigger.
        /// </summary>
        /// <param name="other">The collider entering the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (_isPressed)
            {
                return;
            }

            transform.localPosition = new Vector3(
                _startLocalPosition.x,
                _startLocalPosition.y - pressDepth,
                _startLocalPosition.z);

            _presser = other.gameObject;
            onPress?.Invoke();
            _isPressed = true;
        }

        /// <summary>
        /// Called when another collider exits this trigger.
        /// </summary>
        /// <param name="other">The collider exiting the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != _presser)
            {
                return;
            }

            StartCoroutine(TriggerExit());
        }

        /// <summary>
        /// Handles delayed release behavior, restoring position and invoking release event.
        /// </summary>
        /// <returns>IEnumerator for coroutine execution.</returns>
        private IEnumerator TriggerExit()
        {
            transform.localPosition = _startLocalPosition;

            onRelease?.Invoke();

            yield return new WaitForSeconds(0.5f);

            _isPressed = false;
        }

        #endregion
    }
}