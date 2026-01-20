using UnityEngine;
using UnityEngine.InputSystem;

namespace StudioXRCL.EscapeRoom.XR
{
    /// <summary>
    /// Manages hand animation parameters based on XR controller
    /// trigger and grip input actions.
    /// </summary>
    public class HandControllerManager : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Input Actions")]
        [Tooltip("Input action reference for the trigger control.")]
        public InputActionReference triggerActionReference;

        [Tooltip("Input action reference for the grip control.")]
        public InputActionReference gripActionReference;

        [Header("Animation")]
        [Tooltip("Animator controlling the hand model.")]
        public Animator handAnimator;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches required components and sets up input action callbacks.
        /// </summary>
        private void Start()
        {
            if (handAnimator == null)
            {
                handAnimator = GetComponent<Animator>();
            }

            SetupInputActions();
        }

        /// <summary>
        /// Enables input actions when the object becomes active.
        /// </summary>
        private void OnEnable()
        {
            triggerActionReference?.action.Enable();
            gripActionReference?.action.Enable();
        }

        /// <summary>
        /// Disables input actions when the object becomes inactive.
        /// </summary>
        private void OnDisable()
        {
            triggerActionReference?.action.Disable();
            gripActionReference?.action.Disable();
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Registers callbacks for trigger and grip input actions.
        /// </summary>
        private void SetupInputActions()
        {
            if (triggerActionReference == null || gripActionReference == null)
            {
                Debug.LogWarning("HandControllerManager: Input Action References are not set in the Inspector.");
                return;
            }

            triggerActionReference.action.performed += ctx =>
                UpdateHandAnimation("Trigger", ctx.ReadValue<float>());

            triggerActionReference.action.canceled += _ =>
                UpdateHandAnimation("Trigger", 0f);

            gripActionReference.action.performed += ctx =>
                UpdateHandAnimation("Grip", ctx.ReadValue<float>());

            gripActionReference.action.canceled += _ =>
                UpdateHandAnimation("Grip", 0f);
        }

        /// <summary>
        /// Updates the specified animator float parameter.
        /// </summary>
        /// <param name="parameterName">Animator parameter name.</param>
        /// <param name="value">Value to apply.</param>
        private void UpdateHandAnimation(string parameterName, float value)
        {
            if (handAnimator == null)
                return;

            handAnimator.SetFloat(parameterName, value);
        }

        #endregion
    }
}
