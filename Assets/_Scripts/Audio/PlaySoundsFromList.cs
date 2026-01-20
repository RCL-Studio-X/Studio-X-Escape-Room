using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Play from a list of sounds using next, previous, and random.
/// Includes audio settings like 2D/3D and volume.
/// </summary>
public class PlaySoundsFromList : MonoBehaviour
{
    [Tooltip("Loop the currently playing sound")]
    public bool shouldLoop = false;

    [Tooltip("Use 3D spatial audio (true) or 2D stereo (false).")]
    public bool use3DAudio = true;

    [Tooltip("Audio volume (0 = mute, 1 = full volume).")]
    [Range(0f, 1f)]
    public float audioVolume = 1.0f;

    [Tooltip("The list of audio clips to play from")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource;
    private int index = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();

        ApplyAudioSettings();
        PlayCurrentClip();
    }

    public void NextClip()
    {
        if (audioClips.Count == 0) return;

        index = (index + 1) % audioClips.Count;
        PlayClip();
    }

    public void PreviousClip()
    {
        if (audioClips.Count == 0) return;

        index = (index - 1 + audioClips.Count) % audioClips.Count;
        PlayClip();
    }

    public void RandomClip()
    {
        if (audioClips.Count == 0) return;

        index = Random.Range(0, audioClips.Count);
        PlayClip();
    }

    public void PlayAtIndex(int value)
    {
        if (audioClips.Count == 0) return;

        index = Mathf.Clamp(value, 0, audioClips.Count - 1);
        PlayClip();
    }

    public void PauseClip()
    {
        audioSource.Pause();
    }

    public void StopClip()
    {
        audioSource.Stop();
    }

    public void PlayCurrentClip()
    {
        PlayClip();
    }

    private void PlayClip()
    {
        if (audioClips.Count == 0) return;

        audioSource.clip = audioClips[Mathf.Abs(index) % audioClips.Count];
        audioSource.Play();
    }

    private void ApplyAudioSettings()
    {
        audioSource.loop = shouldLoop;
        audioSource.spatialBlend = use3DAudio ? 1.0f : 0.0f;
        audioSource.volume = audioVolume;
    }

    private void OnValidate()
    {
        if (TryGetComponent(out AudioSource audioSource))
        {
            ApplyAudioSettings();
        }
    }
}
