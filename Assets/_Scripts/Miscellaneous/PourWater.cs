using UnityEngine;

/// <summary>
/// Plays a water pouring particle effect and audio based on object tilt.
/// </summary>
public class PourWater : MonoBehaviour
{
    [Header("Water Effects")]
    [Tooltip("Particle system to simulate pouring water.")]
    public ParticleSystem particleSystemWater;

    [Header("Pouring Settings")]
    [Tooltip("The angle threshold in degrees to start pouring.")]
    [Range(0f, 90f)]
    public float pourThresholdDegrees = 40f;

    [Header("Audio Settings")]
    [Tooltip("Audio clip to play while pouring.")]
    public AudioClip pouringClip;

    [Tooltip("Use 3D spatial audio (true) or 2D stereo (false).")]
    public bool use3DAudio = true;

    [Tooltip("Audio volume (0 = mute, 1 = full volume).")]
    [Range(0f, 1f)]
    public float audioVolume = 1.0f;

    private AudioSource pouringAudio;
    private bool isPouring = false;

    private void Awake()
    {
        // Ensure audio source exists
        pouringAudio = GetComponent<AudioSource>();
        if (!pouringAudio)
            pouringAudio = gameObject.AddComponent<AudioSource>();

        // Configure audio source
        pouringAudio.playOnAwake = false;
        pouringAudio.loop = true;
        pouringAudio.clip = pouringClip;
        pouringAudio.spatialBlend = use3DAudio ? 1.0f : 0.0f;
        pouringAudio.volume = audioVolume;
    }

    private void Update()
    {
        HandlePouring();
    }

    private void HandlePouring()
    {
        float xRotation = NormalizeAngle(transform.eulerAngles.x);

        if (xRotation > pourThresholdDegrees)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        }
    }

    private float NormalizeAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    private void StartPouring()
    {
        if (isPouring) return;

        if (particleSystemWater)
            particleSystemWater.Play();

        if (pouringClip && pouringAudio)
            pouringAudio.Play();

        isPouring = true;
    }

    private void StopPouring()
    {
        if (!isPouring) return;

        if (particleSystemWater)
            particleSystemWater.Stop();

        if (pouringAudio)
            pouringAudio.Stop();

        isPouring = false;
    }

    private void OnValidate()
    {
        // Update audio source settings live in the editor
        if (!pouringAudio) pouringAudio = GetComponent<AudioSource>();
        if (pouringAudio)
        {
            pouringAudio.spatialBlend = use3DAudio ? 1.0f : 0.0f;
            pouringAudio.volume = audioVolume;
            pouringAudio.clip = pouringClip;
        }
    }
}
