using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Hides the droplet visuals and disables its physics on first collision,
    /// then plays an impact sound (with a configurable pitch) before destroying the droplet.
    /// </summary>
    public class DestroyWithSound : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Audio")]
        [Tooltip("AudioSource used to play the impact sound. Typically on this GameObject.")]
        public AudioSource audioSource;

        [Tooltip("Audio clip to play when the droplet collides.")]
        public AudioClip clip;

        [Header("Pitch")]
        [Tooltip("Pitch to use when playing the clip. Set this externally (e.g., by a GameManager) before collision.")]
        public float currentPitch = 1.0f;

        [Header("Destroy")]
        [Tooltip("Seconds after creation before the object destroys itself. Set slightly longer than your longest clip.")]
        public float lifetime = 3f;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Ensures the collision behavior only runs once.
        /// </summary>
        private bool _triggered = false;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Schedules the GameObject to destroy itself after <see cref="lifetime"/> seconds.
        /// </summary>
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Called by Unity when this object first collides with another collider.
        /// Hides visuals, disables physics, plays the impact sound, then destroys the object after the sound completes.
        /// </summary>
        /// <param name="collision">Collision data provided by Unity.</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (_triggered)
                return;

            _triggered = true;

            HideVisuals();
            DisablePhysics();
            PlayImpactSoundAndDestroy();
        }

        /// <summary>
        /// Disables all Renderers on this object and its children so it is no longer visible.
        /// </summary>
        private void HideVisuals()
        {
            foreach (Renderer rendererComponent in GetComponentsInChildren<Renderer>())
                rendererComponent.enabled = false;
        }

        /// <summary>
        /// Disables all Colliders on this object and its children, and stops Rigidbody motion if present.
        /// </summary>
        private void DisablePhysics()
        {
            foreach (Collider colliderComponent in GetComponentsInChildren<Collider>())
                colliderComponent.enabled = false;

            Rigidbody rigidbodyComponent = GetComponent<Rigidbody>();
            if (rigidbodyComponent == null)
                return;

            // Use velocity for compatibility across Unity versions (linearVelocity is not available in all versions).
            rigidbodyComponent.linearVelocity = Vector3.zero;
            rigidbodyComponent.angularVelocity = Vector3.zero;
            rigidbodyComponent.isKinematic = true;
        }

        /// <summary>
        /// Plays the configured impact sound using the configured pitch.
        /// Destruction is already scheduled via Start(); no need to handle it here.
        /// </summary>
        private void PlayImpactSoundAndDestroy()
        {
            if (audioSource == null || clip == null)
                return;

            audioSource.spatialBlend = 1f;
            audioSource.pitch = currentPitch;
            audioSource.PlayOneShot(clip);
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Sets the pitch used when playing the impact sound.
        /// Call this right after instantiating the droplet (before it collides).
        /// </summary>
        /// <param name="pitch">Desired pitch value.</param>
        public void SetPitch(float pitch)
        {
            currentPitch = pitch;
        }
  
        #endregion
    }
}