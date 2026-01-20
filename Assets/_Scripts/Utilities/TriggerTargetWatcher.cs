using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Watches for a specific target GameObject entering this trigger
    /// and invokes an event when it does.
    /// </summary>
    public class TriggerTargetWatcher : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Trigger Settings")]
        [Tooltip("The specific GameObject that must enter this trigger to invoke the event.")]
        public GameObject targetGameObject;

        [Header("Events")]
        [Tooltip("Invoked when the target GameObject enters this trigger.")]
        public UnityEvent onTriggerEntered;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Called when another collider enters this trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{other.gameObject.name} entered the trigger.");

            // Ensure the target is assigned
            if (targetGameObject == null)
                return;

            // Invoke the event only when the correct object enters the trigger
            if (other.gameObject == targetGameObject)
            {
                onTriggerEntered?.Invoke();
            }
        }

        #endregion
    }
}
