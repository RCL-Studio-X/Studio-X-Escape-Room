using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using StudioXRCL.EscapeRoom.Core;

namespace StudioXRCL.EscapeRoom.Utilities
{
    [RequireComponent(typeof(XRSocketInteractor))]
    public class RockSocket : MonoBehaviour
    {
        public SymbolChunk CurrentChunk { get; private set; }

        private XRSocketInteractor _socket;
        public System.Action<RockSocket> OnChanged;

        /// <summary>
        /// Initializes the socket interactor reference.
        /// </summary>
        private void Awake()
        {
            _socket = GetComponent<XRSocketInteractor>();
        }

        /// <summary>
        /// Subscribes to socket select events.
        /// </summary>
        private void OnEnable()
        {
            _socket.selectEntered.AddListener(OnSelectEntered);
            _socket.selectExited.AddListener(OnSelectExited);
        }

        /// <summary>
        /// Unsubscribes from socket select events.
        /// </summary>
        private void OnDisable()
        {
            _socket.selectEntered.RemoveListener(OnSelectEntered);
            _socket.selectExited.RemoveListener(OnSelectExited);
        }

        /// <summary>
        /// Handles when an object is selected into the socket.
        /// </summary>
        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            CurrentChunk = args.interactableObject.transform.GetComponent<SymbolChunk>();
            OnChanged?.Invoke(this);
        }

        /// <summary>
        /// Handles when an object is deselected from the socket.
        /// </summary>
        private void OnSelectExited(SelectExitEventArgs args)
        {
            var leaving = args.interactableObject.transform.GetComponent<SymbolChunk>();
            if (leaving == CurrentChunk) CurrentChunk = null;
            OnChanged?.Invoke(this);
        }
    }
}
