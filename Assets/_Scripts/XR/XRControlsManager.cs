using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using StudioXRCL.EscapeRoom.UI;

namespace StudioXRCL.EscapeRoom.XR
{
    /// <summary>
    /// Central manager for XR locomotion, turning, teleportation,
    /// and wrist UI control settings.
    /// </summary>
    public class XRControlsManager : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Startup Settings")]
        [Tooltip("If true, all locomotion controls are disabled on start.")]
        public bool disableMovementOnStart = true;

        [Header("Default Movement Settings")]
        [Tooltip("Enable or disable smooth continuous movement by default.")]
        public bool smoothMotionEnabled = true;

        [Tooltip("Enable or disable teleportation movement by default.")]
        public bool teleportationEnabled = true;

        [Header("Default Turn Settings")]
        [Tooltip("Enable or disable smooth turning by default.")]
        public bool smoothTurnEnabled = false;

        [Tooltip("Snap turn angle in degrees.")]
        public float snapTurnAmount = 45f;

        [Tooltip("Smooth turn speed in degrees per second.")]
        public float smoothTurnSpeed = 60f;

        [Header("Controller Input Action Managers")]
        [Tooltip("Input Action Manager for the left controller.")]
        public ControllerInputActionManager leftControllerInputManager;

        [Tooltip("Input Action Manager for the right controller.")]
        public ControllerInputActionManager rightControllerInputManager;

        [Header("Teleportation Rays")]
        [Tooltip("XR Ray Interactor for left-hand teleportation.")]
        public XRRayInteractor leftTeleportationRay;

        [Tooltip("XR Ray Interactor for right-hand teleportation.")]
        public XRRayInteractor rightTeleportationRay;

        [Header("User Interface")]
        [Tooltip("Alignment trigger controlling wrist watch visibility.")]
        public AlignmentTrigger wristWatchAlignmentTrigger;

        [Tooltip("Wrist watch UI GameObject.")]
        public GameObject wristWatchUI;

        #endregion

        #region Private Variable Declarations

        /// <summary>Snap turn provider used for snap rotation.</summary>
        private SnapTurnProvider _snapTurnProvider;

        /// <summary>Continuous turn provider used for smooth rotation.</summary>
        private ContinuousTurnProvider _continuousTurnProvider;

        // Cached defaults
        private bool _defaultSmoothMotion;
        private bool _defaultTeleportation;
        private bool _defaultSmoothTurn;
        private float _defaultSnapTurnAmount;
        private float _defaultSmoothTurnSpeed;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Locates required turn providers.
        /// </summary>
        private void Start()
        {
            _snapTurnProvider = GetComponentInChildren<SnapTurnProvider>();
            _continuousTurnProvider = GetComponentInChildren<ContinuousTurnProvider>();

            if (_snapTurnProvider == null || _continuousTurnProvider == null)
            {
                Debug.LogError(
                    "XRControlsManager: Missing turn providers. Attach this component to the XR Origin or its parent."
                );
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes XR controls and applies startup settings.
        /// </summary>
        public void StartSetUp()
        {
            if (wristWatchUI != null)
            {
                wristWatchUI.SetActive(false);
            }

            RecordDefaults();

            if (disableMovementOnStart)
            {
                DisableAllControls();
            }

            UpdateValues();
        }

        #endregion

        #region Value Updates

        /// <summary>
        /// Applies current locomotion and turning settings to XR systems.
        /// </summary>
        private void UpdateValues()
        {
            if (rightControllerInputManager != null)
            {
                rightControllerInputManager.smoothTurnEnabled = smoothTurnEnabled;
            }
            else
            {
                Debug.LogWarning("Right Controller Input Action Manager is not assigned.");
            }

            if (_snapTurnProvider != null)
            {
                _snapTurnProvider.turnAmount = snapTurnAmount;
            }

            if (_continuousTurnProvider != null)
            {
                _continuousTurnProvider.turnSpeed = smoothTurnSpeed;
            }

            if (leftControllerInputManager != null)
            {
                leftControllerInputManager.smoothMotionEnabled = smoothMotionEnabled;
            }
            else
            {
                Debug.LogWarning("Left Controller Input Action Manager is not assigned.");
            }

            if (rightTeleportationRay != null)
            {
                rightTeleportationRay.enabled = teleportationEnabled;
            }

            if (leftTeleportationRay != null)
            {
                leftTeleportationRay.enabled = teleportationEnabled;
            }

            InterfaceUpdate[] interfaces = FindObjectsByType<InterfaceUpdate>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (InterfaceUpdate iface in interfaces)
            {
                iface.UpdateValues();
            }
        }

        #endregion

        #region Defaults Handling

        /// <summary>
        /// Caches the initial control values as defaults.
        /// </summary>
        private void RecordDefaults()
        {
            XRControlsDefaultsCache.Cache(
                smoothMotionEnabled,
                teleportationEnabled,
                smoothTurnEnabled,
                snapTurnAmount,
                smoothTurnSpeed
            );

            _defaultSmoothMotion = XRControlsDefaultsCache.SmoothMotion;
            _defaultTeleportation = XRControlsDefaultsCache.Teleportation;
            _defaultSmoothTurn = XRControlsDefaultsCache.SmoothTurn;
            _defaultSnapTurnAmount = XRControlsDefaultsCache.SnapTurnAmount;
            _defaultSmoothTurnSpeed = XRControlsDefaultsCache.SmoothTurnSpeed;
        }

        /// <summary>
        /// Restores locomotion and turning values to their defaults.
        /// </summary>
        public void RestoreDefaults()
        {
            smoothMotionEnabled = _defaultSmoothMotion;
            teleportationEnabled = _defaultTeleportation;
            smoothTurnEnabled = _defaultSmoothTurn;
            snapTurnAmount = _defaultSnapTurnAmount;
            smoothTurnSpeed = _defaultSmoothTurnSpeed;

            if (wristWatchAlignmentTrigger != null)
            {
                wristWatchAlignmentTrigger.enabled = true;
            }

            UpdateValues();
        }

        #endregion

        #region Disable All Controls

        /// <summary>
        /// Disables all movement and turning controls.
        /// </summary>
        public void DisableAllControls()
        {
            smoothMotionEnabled = false;
            teleportationEnabled = false;
            smoothTurnEnabled = false;

            if (wristWatchAlignmentTrigger != null)
            {
                wristWatchAlignmentTrigger.enabled = false;
            }

            UpdateValues();
        }

        #endregion

        #region Public Setters

        /// <summary>
        /// Enables or disables smooth movement.
        /// </summary>
        public void SetSmoothMotion(bool isOn)
        {
            smoothMotionEnabled = isOn;
            UpdateValues();
        }

        /// <summary>
        /// Enables or disables smooth turning.
        /// </summary>
        public void SetSmoothTurn(bool isOn)
        {
            smoothTurnEnabled = isOn;
            UpdateValues();
        }

        /// <summary>
        /// Enables or disables teleportation.
        /// </summary>
        public void SetTeleportation(bool isOn)
        {
            teleportationEnabled = isOn;
            UpdateValues();
        }

        /// <summary>
        /// Sets the snap turn angle.
        /// </summary>
        public void SetSnapTurnAmount(float amount)
        {
            snapTurnAmount = amount;
            UpdateValues();
        }

        /// <summary>
        /// Sets the smooth turn speed.
        /// </summary>
        public void SetSmoothTurnSpeed(float speed)
        {
            smoothTurnSpeed = speed;
            UpdateValues();
        }

        /// <summary>
        /// Sets smooth turning based on a dropdown selection.
        /// </summary>
        /// <param name="dropdown">Dropdown UI element.</param>
        public void SetSmoothTurnFromDropdown(Dropdown dropdown)
        {
            SetSmoothTurn(dropdown.value != 0);
        }

        #endregion
    }

        /// <summary>
    /// Caches default XR control values so they can be restored later.
    /// </summary>
    public static class XRControlsDefaultsCache
    {
        /// <summary>Indicates whether defaults have already been cached.</summary>
        public static bool HasCached { get; private set; }

        /// <summary>Default smooth movement state.</summary>
        public static bool SmoothMotion;

        /// <summary>Default teleportation state.</summary>
        public static bool Teleportation;

        /// <summary>Default smooth turning state.</summary>
        public static bool SmoothTurn;

        /// <summary>Default snap turn amount.</summary>
        public static float SnapTurnAmount;

        /// <summary>Default smooth turn speed.</summary>
        public static float SmoothTurnSpeed;

        /// <summary>
        /// Caches XR control defaults once.
        /// </summary>
        public static void Cache(
            bool smoothMotion,
            bool teleportation,
            bool smoothTurn,
            float snapAmount,
            float smoothSpeed)
        {
            if (HasCached)
                return;

            SmoothMotion = smoothMotion;
            Teleportation = teleportation;
            SmoothTurn = smoothTurn;
            SnapTurnAmount = snapAmount;
            SmoothTurnSpeed = smoothSpeed;
            HasCached = true;
        }
    }
}
