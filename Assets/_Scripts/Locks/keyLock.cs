using UnityEngine;


public class keyLock : MonoBehaviour
{
    public bool locked = true;
    public AudioSource audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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