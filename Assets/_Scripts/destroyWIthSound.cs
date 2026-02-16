using UnityEngine;
using System.Collections;

public class destroyWithSOund : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public float pitch = 1.0f;

    private bool triggered = false;

    void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;
        triggered = true;

        float duration = 0f;

        // Play 3D sound

        audioSource.spatialBlend = 1f; // 3D
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);

        duration = clip.length / Mathf.Max(0.01f, pitch);



        Destroy(gameObject);
    }

}
