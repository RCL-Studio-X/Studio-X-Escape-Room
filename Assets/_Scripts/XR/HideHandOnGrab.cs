using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace StudioXRCL.EscapeRoom.XR
{
    /// <summary>
    /// Hides or shows a hand model when an XR Direct Interactor
    /// grabs or releases an interactable object.
    /// </summary>
    public class HideHandOnGrab : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Interactor Settings")]
        [Tooltip("XR Direct Interactor used to detect grab and release events.")]
        public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor interactor;

        [Header("Hand Visual")]
        [Tooltip("Hand model GameObject to hide or show.")]
        public GameObject handModel;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Subscribes to grab and release events when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            if (interactor == null)
                return;

            interactor.selectEntered.AddListener(OnGrab);
            interactor.selectExited.AddListener(OnRelease);
        }

        /// <summary>
        /// Unsubscribes from grab and release events when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            if (interactor == null)
                return;

            interactor.selectEntered.RemoveListener(OnGrab);
            interactor.selectExited.RemoveListener(OnRelease);
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Called when the interactor grabs an interactable.
        /// Hides the hand model.
        /// </summary>
        /// <param name="args">Event data for the grab action.</param>
        private void OnGrab(SelectEnterEventArgs args)
        {
            if (handModel == null)
                return;

            handModel.SetActive(false);
        }

        /// <summary>
        /// Called when the interactor releases an interactable.
        /// Shows the hand model.
        /// </summary>
        /// <param name="args">Event data for the release action.</param>
        private void OnRelease(SelectExitEventArgs args)
        {
            if (handModel == null)
                return;

            handModel.SetActive(true);
        }

        #endregion
    }
}
