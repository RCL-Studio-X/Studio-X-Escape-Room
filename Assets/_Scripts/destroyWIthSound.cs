using UnityEngine;

public class DestroyWithSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public float pitch = 1.0f;

    private bool triggered = false;

    void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;
        triggered = true;

        //Hide visuals
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;


        //Disable physics(may don't need?)
        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;   // for newer Unity versions
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        //Play sound
        audioSource.spatialBlend = 1f;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);

        float duration = clip.length / Mathf.Max(0.01f, Mathf.Abs(pitch));
        Destroy(gameObject, duration);
    }

    public void SetPitch(float newPitch)
    {
        pitch = newPitch;
    }
}