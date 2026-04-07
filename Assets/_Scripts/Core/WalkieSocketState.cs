using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WalkieSocketState : MonoBehaviour
{
    public InteractionLayerMask freeLayer;
    public InteractionLayerMask socketedLayer;

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor)
        {
            grab.interactionLayers = socketedLayer;
        }
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        grab.interactionLayers = freeLayer;
    }
}