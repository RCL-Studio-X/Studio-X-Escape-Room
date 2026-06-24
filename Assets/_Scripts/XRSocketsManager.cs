using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketsManager : MonoBehaviour
{
    [System.Serializable]
    public class SocketRequirement
    {
        public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;   // The XR socket
        public string requiredTag;          // The tag the inserted object must have
        [HideInInspector] public bool isCorrect;
    }

    [Header("Socket Requirements (4 Sockets)")]
    public SocketRequirement[] sockets = new SocketRequirement[4];

    [Header("Events")]
    public UnityEvent onAllSocketsCorrect;

    private void Awake()
    {
        foreach (var s in sockets)
        {
            s.socket.selectEntered.AddListener(OnSocketEntered);
            s.socket.selectExited.AddListener(OnSocketExited);
        }
    }

    private void OnDestroy()
    {
        foreach (var s in sockets)
        {
            s.socket.selectEntered.RemoveListener(OnSocketEntered);
            s.socket.selectExited.RemoveListener(OnSocketExited);
        }
    }

    private void OnSocketEntered(SelectEnterEventArgs args)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor;
        GameObject inserted = args.interactableObject.transform.gameObject;

        foreach (var s in sockets)
        {
            if (s.socket == socket)
            {
                if (inserted.CompareTag(s.requiredTag))
                {
                    s.isCorrect = true;
                    Debug.Log($"[XR SOCKET] Correct object inserted into socket '{socket.name}'. Tag matched: {s.requiredTag}");
                }
                else
                {
                    s.isCorrect = false;
                    Debug.Log($"[XR SOCKET] WRONG object inserted into socket '{socket.name}'. Expected tag '{s.requiredTag}', but got '{inserted.tag}'");
                }
                break;
            }
        }

        if (AllSocketsCorrect())
        {
            Debug.Log("[XR SOCKET] All sockets correctly populated! Invoking event.");
            onAllSocketsCorrect.Invoke();
        }
    }

    private void OnSocketExited(SelectExitEventArgs args)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor;

        foreach (var s in sockets)
        {
            if (s.socket == socket)
            {
                s.isCorrect = false;
                Debug.Log($"[XR SOCKET] Object removed from socket '{socket.name}'. Marking as unfilled.");
                break;
            }
        }
    }

    private bool AllSocketsCorrect()
    {
        foreach (var s in sockets)
        {
            if (!s.isCorrect)
                return false;
        }
        return true;
    }
}
