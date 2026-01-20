using System.Collections;
using UnityEngine;
using UnityEngine.XR; // Required for Haptics

public class CameraShaker : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("How long the earthquake lasts")]
    [SerializeField] private float duration = 3.0f;

    [Tooltip("How violently the world moves (Keep this LOW for VR: 0.01 - 0.1)")]
    [SerializeField] private float magnitude = 0.015f;

    [Header("Haptics (Controller Vibration)")]
    [SerializeField] private float hapticIntensity = 0.8f;

    [Header("Audio")]
    [SerializeField] private AudioSource earthquakeAudio;

    private Vector3 originalLocalPos;

    void Start()
    {
        // Automatically trigger earthquake when this object (the player) spawns
        TriggerEarthquake();
    }

    public void TriggerEarthquake()
    {
        originalLocalPos = transform.localPosition;
        StartCoroutine(ShakeRoutine());

        // Play Sound
        if (earthquakeAudio != null) earthquakeAudio.Play();

        // Trigger Haptics on both hands
        TriggerHaptics(XRNode.LeftHand);
        TriggerHaptics(XRNode.RightHand);
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Generate random offset inside a sphere
            // We ONLY change Position, NEVER Rotation (Rotation = Nausea)
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply the shake
            transform.localPosition = new Vector3(originalLocalPos.x + x, originalLocalPos.y + y, originalLocalPos.z);

            elapsed += Time.deltaTime;

            // Optional: Fade out the shake intensity over time
            // float decay = 1 - (elapsed / duration);
            // transform.localPosition *= decay;

            yield return null;
        }

        // Reset to perfect center when done
        transform.localPosition = originalLocalPos;
    }

    private void TriggerHaptics(XRNode node)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        if (device.isValid)
        {
            // channel 0, amplitude, duration
            device.SendHapticImpulse(0, hapticIntensity, duration);
        }
    }
}
