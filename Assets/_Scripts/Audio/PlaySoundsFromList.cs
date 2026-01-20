using System.Collections.Generic;
using UnityEngine;

namespace StudioXRCL.EscapeRoom.Audio
{
    /// <summary>
    /// Plays audio clips from a list using next, previous, random, or indexed selection.
    /// Supports looping, 2D/3D spatial audio, and volume control.
    /// </summary>
    public class PlaySoundsFromList : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Playback Settings")]

        [Tooltip("Loop the currently playing sound.")]
        public bool shouldLoop = false;

        [Tooltip("Use 3D spatial audio (true) or 2D stereo audio (false).")]
        public bool use3DAudio = true;

        [Tooltip("Audio volume (0 = mute, 1 = full volume).")]
        [Range(0f, 1f)]
        public float audioVolume = 1.0f;

        [Header("Audio Clips")]

        [Tooltip("List of audio clips available for playback.")]
        public List<AudioClip> audioClips = new List<AudioClip>();

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// AudioSource component responsible for audio playback.
        /// </summary>
        private AudioSource _audioSource;

        /// <summary>
        /// Index of the currently selected audio clip.
        /// </summary>
        private int _index = 0;

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Initializes the AudioSource component and starts playback of the current clip.
        /// </summary>
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            ApplyAudioSettings();
            PlayCurrentClip();
        }

        /// <summary>
        /// Plays the audio clip at the current index.
        /// </summary>
        private void PlayClip()
        {
            if (audioClips.Count == 0)
            {
                return;
            }

            _audioSource.clip = audioClips[Mathf.Abs(_index) % audioClips.Count];
            _audioSource.Play();
        }

        /// <summary>
        /// Applies looping, spatial blend, and volume settings to the AudioSource.
        /// </summary>
        private void ApplyAudioSettings()
        {
            _audioSource.loop = shouldLoop;
            _audioSource.spatialBlend = use3DAudio ? 1.0f : 0.0f;
            _audioSource.volume = audioVolume;
        }

        /// <summary>
        /// Updates AudioSource settings when values are changed in the Inspector.
        /// </summary>
        private void OnValidate()
        {
            if (TryGetComponent(out AudioSource audioSource))
            {
                _audioSource = audioSource;
                ApplyAudioSettings();
            }
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Advances to the next audio clip in the list and plays it.
        /// </summary>
        public void NextClip()
        {
            if (audioClips.Count == 0)
            {
                return;
            }

            _index = (_index + 1) % audioClips.Count;
            PlayClip();
        }

        /// <summary>
        /// Moves to the previous audio clip in the list and plays it.
        /// </summary>
        public void PreviousClip()
        {
            if (audioClips.Count == 0)
            {
                return;
            }

            _index = (_index - 1 + audioClips.Count) % audioClips.Count;
            PlayClip();
        }

        /// <summary>
        /// Selects and plays a random audio clip from the list.
        /// </summary>
        public void RandomClip()
        {
            if (audioClips.Count == 0)
            {
                return;
            }

            _index = Random.Range(0, audioClips.Count);
            PlayClip();
        }

        /// <summary>
        /// Plays an audio clip at the specified index.
        /// </summary>
        /// <param name="value">Index of the clip to play.</param>
        public void PlayAtIndex(int value)
        {
            if (audioClips.Count == 0)
            {
                return;
            }

            _index = Mathf.Clamp(value, 0, audioClips.Count - 1);
            PlayClip();
        }

        /// <summary>
        /// Pauses the currently playing audio clip.
        /// </summary>
        public void PauseClip()
        {
            _audioSource.Pause();
        }

        /// <summary>
        /// Stops playback of the current audio clip.
        /// </summary>
        public void StopClip()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// Plays the currently selected audio clip.
        /// </summary>
        public void PlayCurrentClip()
        {
            PlayClip();
        }

        #endregion
    }
}
