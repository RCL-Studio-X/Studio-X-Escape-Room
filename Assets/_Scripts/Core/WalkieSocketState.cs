using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles interaction layer switching for the walkie talkie when it is placed into or removed from a socket.
    /// Ensures only the correct hand can interact with it while socketed.
    /// </summary>
    public class WalkieSocketState : MonoBehaviour
    {
        #region Public Variables

        [Header("Interaction Layers")]

        [Tooltip("Interaction layer used when the walkie is free and can be grabbed by both hands.")]
        public InteractionLayerMask freeLayer;

        [Tooltip("Interaction layer used when the walkie is in the socket and should only be interactable by the left hand.")]
        public InteractionLayerMask socketedLayer;

        #endregion

        #region Private Variables

        /// <summary> Reference to the XR Grab Interactable component. </summary>
        private XRGrabInteractable _grabInteractable;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Initializes component references.
        /// </summary>
        private void Awake()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when the walkie is selected (grabbed or socketed).
        /// Switches to socketed interaction layer if placed into a socket.
        /// </summary>
        /// <param name="args">Event data for the select enter event.</param>
        public void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor)
            {
                _grabInteractable.interactionLayers = socketedLayer;
            }
        }

        /// <summary>
        /// Called when the walkie is deselected (released or removed from socket).
        /// Restores the free interaction layer.
        /// </summary>
        /// <param name="args">Event data for the select exit event.</param>
        public void OnSelectExited(SelectExitEventArgs args)
        {
            _grabInteractable.interactionLayers = freeLayer;
        }

        #endregion
    }
}