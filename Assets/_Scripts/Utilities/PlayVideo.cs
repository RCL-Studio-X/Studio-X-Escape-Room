using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Plays a video from a list of clips using a mesh renderer and optional material.
/// Automatically adds a VideoPlayer component if one is not found.
/// </summary>
public class PlayVideo : MonoBehaviour
{
    [Tooltip("Whether video should play on start.")]
    public bool playAtStart = false;

    [Tooltip("Material used for video playback (defaults to URP/Unlit).")]
    public Material videoMaterial = null;

    [Tooltip("List of video clips to choose from.")]
    public List<VideoClip> videoClips = new List<VideoClip>();

    [Header("Audio Settings")]
    [Tooltip("Use 3D spatial audio (true) or 2D stereo (false).")]
    public bool use3DAudio = true;

    [Tooltip("Audio volume (0 = mute, 1 = full volume).")]
    [Range(0f, 1f)]
    public float audioVolume = 1.0f;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;

    private Material offMaterial;
    private int index = 0;

    private readonly string shaderUsed = "Universal Render Pipeline/Unlit";
    private readonly Color colorOff = Color.black;
    private readonly Color colorOn = Color.white;

    private void Awake()
    {
        // Ensure required components
        meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        videoPlayer = GetComponent<VideoPlayer>();
        if (!videoPlayer)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Apply audio settings
        audioSource.spatialBlend = use3DAudio ? 1.0f : 0.0f; // 1 = 3D, 0 = 2D
        audioSource.volume = audioVolume;

        // Cache initial material
        offMaterial = meshRenderer.material;

        // Setup video material if not set
        if (videoMaterial == null)
        {
            videoMaterial = new Material(Shader.Find(shaderUsed));
        }
        videoMaterial.color = colorOn;

        // Assign first clip
        if (videoClips.Count > 0)
        {
            videoPlayer.clip = videoClips[0];
        }

        // Link audio source to video player
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }

    private void Start()
    {
        if (playAtStart)
            Play();
        else
            Stop();
    }

    public void Play()
    {
        ApplyVideoMaterial();
        videoPlayer.Play();
    }

    public void Stop()
    {
        meshRenderer.material = offMaterial;
        videoPlayer.Stop();
    }

    public void TogglePlayStop()
    {
        SetPlay(!videoPlayer.isPlaying);
    }

    public void TogglePlayPause()
    {
        ApplyVideoMaterial();

        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    public void SetPlay(bool value)
    {
        if (value) Play();
        else Stop();
    }

    public void NextClip()
    {
        if (videoClips.Count == 0) return;

        index = (index + 1) % videoClips.Count;
        SetClipAndPlay();
    }

    public void PreviousClip()
    {
        if (videoClips.Count == 0) return;

        index = (index - 1 + videoClips.Count) % videoClips.Count;
        SetClipAndPlay();
    }

    public void RandomClip()
    {
        if (videoClips.Count == 0) return;

        index = Random.Range(0, videoClips.Count);
        SetClipAndPlay();
    }

    public void PlayAtIndex(int value)
    {
        if (videoClips.Count == 0) return;

        index = Mathf.Clamp(value, 0, videoClips.Count - 1);
        SetClipAndPlay();
    }

    private void SetClipAndPlay()
    {
        videoPlayer.clip = videoClips[index];
        videoMaterial.color = videoClips[index] ? colorOn : colorOff;
        Play();
    }

    private void ApplyVideoMaterial()
    {
        if (meshRenderer.material != videoMaterial)
        {
            meshRenderer.material = videoMaterial;
        }
    }

    private void OnValidate()
    {
        if (TryGetComponent(out VideoPlayer vp))
        {
            vp.targetMaterialProperty = "_BaseMap";
        }

        if (TryGetComponent(out AudioSource audioSrc))
        {
            audioSrc.spatialBlend = use3DAudio ? 1.0f : 0.0f;
            audioSrc.volume = audioVolume;
        }
    }
}
