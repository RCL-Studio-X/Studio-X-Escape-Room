using UnityEngine;
using UnityEngine.Events;

public class fobLock : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Audio source that plays when the lock is unlocked.")]
    public AudioSource audioSource;

    [Header("Fob Reference")]
    [Tooltip("The fob object that can unlock this lock.")]
    public GameObject fob;

    [Header("Lock State")]
    [Tooltip("Indicates whether the lock starts in a locked state.")]
    public bool locked = true;

    [Header("Events")]
    [Tooltip("Event invoked when the lock becomes unlocked.")]
    public UnityEvent onUnlocked;

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!locked)
            return;

        // Compare by instance equality, avoids null comparisons and keeps performance safe
        if (other.gameObject == fob)
        {
            locked = false;
            audioSource.Play();
            onUnlocked?.Invoke();
        }
    }
}
