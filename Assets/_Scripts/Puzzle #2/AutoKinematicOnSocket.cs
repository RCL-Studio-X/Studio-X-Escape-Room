using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoKinematicOnSocket : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private Rigidbody rb;
    
    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectEntered.AddListener(OnSelectEntered);
        grab.selectExited.AddListener(OnSelectExited);
    }

    // Called when the object is snapped into a socket
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor)
        {
            rb.isKinematic = true; // Stop physics
        }
    }

    // Called when the object is grabbed again
    private void OnSelectExited(SelectExitEventArgs args)
    {
        rb.isKinematic = false; // Restore physics
    }
}
