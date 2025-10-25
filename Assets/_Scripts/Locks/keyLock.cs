using UnityEngine;


public class keyLock : MonoBehaviour
{
    public bool locked = true;
    private bool selected = false;
    public AudioSource audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (selected && gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().hasSelection) {
            locked = false;
            audio.Play();
        }
    }

    void onSelectEnter()
    {
        selected = true;
    }

    void onSelectExit()
    {
        selected = false;
    }
}