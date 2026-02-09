using StudioXRCL.EscapeRoom.XR;
using UnityEngine;
using UnityEngine.UI;

namespace StudioXRCL.EscapeRoom.UI
{
    /// <summary>
    /// Updates UI elements to reflect the current XR control settings
    /// such as movement mode, teleportation, and turning style.
    /// </summary>
    public class InterfaceUpdate : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("XR Settings UI")]
        [Tooltip("Dropdown used to switch between snap turn and smooth turn.")]
        public Dropdown smoothSnapDropdown;

        [Tooltip("Toggle that enables or disables continuous movement.")]
        public Toggle continuousMoveToggle;

        [Tooltip("Toggle that enables or disables teleportation movement.")]
        public Toggle teleportationToggle;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Reference to the XR controls manager that stores movement settings.
        /// </summary>
        private XRControlsManager _xrControlsManager;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Finds the XRControlsManager and initializes UI values.
        /// </summary>
        private void Start()
        {
            _xrControlsManager = FindFirstObjectByType<XRControlsManager>();

            if (!_xrControlsManager)
            {
                Debug.LogError("Unable to find XRControlsManager.");
                return;
            }

            UpdateValues();
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Synchronizes UI toggle and dropdown values with the XR control settings.
        /// </summary>
        public void UpdateValues()
        {
            if (!_xrControlsManager)
            {
                return;
            }

            if (continuousMoveToggle)
            {
                continuousMoveToggle.isOn = _xrControlsManager.smoothMotionEnabled;
            }

            if (teleportationToggle)
            {
                teleportationToggle.isOn = _xrControlsManager.teleportationEnabled;
            }

            SetDropdownValue();
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Updates the turn mode dropdown based on XR control settings.
        /// </summary>
        private void SetDropdownValue()
        {
            if (smoothSnapDropdown)
            {
                smoothSnapDropdown.value = _xrControlsManager.smoothTurnEnabled ? 1 : 0;
            }
        }

        #endregion
    }
}
