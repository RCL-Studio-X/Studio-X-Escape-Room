using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // <-- IMPORTANT for XRI 3.x

[RequireComponent(typeof(XRSocketInteractor))]
public class RockSocket : MonoBehaviour
{
    public SymbolChunk CurrentChunk { get; private set; }

    private XRSocketInteractor socket;
    public System.Action<RockSocket> OnChanged;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        CurrentChunk = args.interactableObject.transform.GetComponent<SymbolChunk>();
        OnChanged?.Invoke(this);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        var leaving = args.interactableObject.transform.GetComponent<SymbolChunk>();
        if (leaving == CurrentChunk) CurrentChunk = null;
        OnChanged?.Invoke(this);
    }
}