using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;


public class keyScript : MonoBehaviour
{
    public bool selected = false;
    public GameObject lockGameObject;
    public keyLock script;
    
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool unlockCheck = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = lockGameObject.GetComponent<keyLock>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();    
        grabInteractable.selectEntered.AddListener(HandleSelectEnter);
        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    // There did exist a bug ehre where the check for unlock that references the unlock boolean in the keylock.cs
    // would still return true because it is late to update. Using a local bool fixes it.

    void HandleSelectEnter(SelectEnterEventArgs arg0)
    {
        if (unlockCheck == true) {
            StartCoroutine(checkUnlock());
            //Debug.Log("Select Entered");
        }
    }

    IEnumerator checkUnlock() {
        while (true)
        {
            if (script.locked && lockGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().hasSelection) {
                script.Unlock();
                //Debug.Log("Attempting to kill coroutine");
                unlockCheck = false;
                yield break;
            }
            // Debug.Log("checkUnlock: Alive");
            yield return null;
        }
        //Debug.Log("Out of While loop");
    }
}
