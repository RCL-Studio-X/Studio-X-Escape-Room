using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles a lock that is unlocked by inserting or colliding with a specific fob object.
    /// When the correct fob enters the trigger, the lock unlocks and invokes related events.
    /// </summary>
    public class LockFob : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Audio")]
        [Tooltip("Audio source that plays when the lock is unlocked.")]
        public AudioSource audioSource;

        [Header("Fob Reference")]
        [Tooltip("The fob object that can unlock this lock.")]
        public GameObject fob;

        [Header("Lock State")]
        [Tooltip("Indicates whether the lock starts in a locked state.")]
        public bool locked = true;

        [Header("Events")]
        [Tooltip("Event invoked when the lock becomes unlocked.")]
        public UnityEvent onUnlocked;

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Detects when a collider enters the trigger and checks if it is the correct fob.
        /// If so, unlocks the lock, plays audio, and invokes the unlock event.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!locked)
                return;

            if (other.gameObject != fob)
                return;

            locked = false;

            if (audioSource != null)
                audioSource.Play();

            onUnlocked?.Invoke();
        }

        #endregion
    }
}
