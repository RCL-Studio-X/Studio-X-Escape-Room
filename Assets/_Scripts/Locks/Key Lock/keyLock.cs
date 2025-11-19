using UnityEngine;
using UnityEngine.Events;


public class keyLock : MonoBehaviour
{
    public bool locked = true;
    public AudioSource audioSource;
    
    [Header("Events")]
    [Tooltip("Event invoked when the lock becomes unlocked.")]
    public UnityEvent onUnlocked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Unlock()
    {
        locked = false;
        audioSource.Play();
        onUnlocked?.Invoke();
    }

   // Update is called once per frame
    // void Update()
    // {   
    //     if (locked && gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().hasSelection) {
    //         locked = false;
    //         audio.Play();
    //     }
    // }
}
