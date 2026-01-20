using UnityEngine;

namespace StudioX.EscapeRoom.Audio
{
    /// <summary>
    /// Plays a one-shot sound clip with configurable volume and random pitch variation.
    /// Automatically adds an AudioSource component if one is not found on the GameObject.
    /// </summary>
    public class PlayQuickSound : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Audio Settings")]

        [Tooltip("The sound clip to play.")]
        public AudioClip sound;

        [Tooltip("Volume at which the sound will be played.")]
        [Range(0f, 1f)]
        public float volume = 1.0f;

        [Tooltip("Random pitch variance applied to the playback (range is -value to +value).")]
        [Range(0f, 1f)]
        public float randomPitchVariance = 0.0f;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// AudioSource used to play the sound clip.
        /// </summary>
        private AudioSource _audioSource;

        /// <summary>
        /// Default pitch value applied after playback.
        /// </summary>
        private const float DEFAULT_PITCH = 1.0f;

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Initializes the AudioSource component and ensures it does not play on awake.
        /// </summary>
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.playOnAwake = false;
        }

        /// <summary>
        /// Ensures the AudioSource does not play automatically when values are changed in the Inspector.
        /// </summary>
        private void OnValidate()
        {
            AudioSource existingSource = GetComponent<AudioSource>();

            if (existingSource != null)
            {
                existingSource.playOnAwake = false;
            }
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Plays the configured sound clip with a randomized pitch variance.
        /// </summary>
        public void Play()
        {
            if (sound == null)
            {
                Debug.LogWarning($"No sound clip assigned on '{gameObject.name}'.");
                return;
            }

            float pitch = DEFAULT_PITCH + Random.Range(-randomPitchVariance, randomPitchVariance);
            _audioSource.pitch = pitch;

            _audioSource.PlayOneShot(sound, volume);

            _audioSource.pitch = DEFAULT_PITCH;
        }

        #endregion
    }
}
