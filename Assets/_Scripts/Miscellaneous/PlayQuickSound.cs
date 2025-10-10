using UnityEngine;

/// <summary>
/// Plays a one-shot sound clip with configurable volume and random pitch variation.
/// Automatically adds an AudioSource component if one isn't found on the GameObject.
/// </summary>
public class PlayQuickSound : MonoBehaviour
{
    [Tooltip("The sound clip to play.")]
    public AudioClip sound;

    [Tooltip("Volume at which the sound will be played.")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Tooltip("Random pitch variance applied to the playback (range is -value to +value).")]
    [Range(0f, 1f)]
    public float randomPitchVariance = 0.0f;

    private AudioSource _audioSource;
    private const float DefaultPitch = 1.0f;

    private void Awake()
    {
        // Try to get the AudioSource component, or add one if not found
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Plays the configured sound with random pitch variance.
    /// </summary>
    public void Play()
    {
        if (sound == null)
        {
            Debug.LogWarning($"No sound clip assigned on '{gameObject.name}'.");
            return;
        }

        float pitch = DefaultPitch + Random.Range(-randomPitchVariance, randomPitchVariance);
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(sound, volume);
        _audioSource.pitch = DefaultPitch;
    }

    private void OnValidate()
    {
        AudioSource existingSource = GetComponent<AudioSource>();
        if (existingSource != null)
        {
            existingSource.playOnAwake = false;
        }
    }
}