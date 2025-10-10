using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRControlsManager : MonoBehaviour
{
    public bool disableMovementOnStart = true;
    [Header("Default Movement Settings")]
    [Tooltip("Enable or disable smooth continuous movement by default.")]
    public bool smoothMotionEnabled = true;

    [Tooltip("Enable or disable teleportation movement by default.")]
    public bool teleportationEnabled = true;

    [Header("Default Turn Settings")]
    [Tooltip("Enable or disable smooth turning by default.")]
    public bool smoothTurnEnabled = false;

    [Tooltip("The snap turn angle (in degrees).")]
    public float snapTurnAmount = 45f;

    [Tooltip("The smooth turn speed (degrees per second).")]
    public float smoothTurnSpeed = 60f;

    [Header("Controller Input Action Managers")]
    [Tooltip("Input Action Manager for the Left Controller.")]
    public ControllerInputActionManager leftControllerInputManager;

    [Tooltip("Input Action Manager for the Right Controller.")]
    public ControllerInputActionManager rightControllerInputManager;

    [Header("Teleportation Rays")]
    [Tooltip("XR Ray Interactor attached to Left Teleport Interactor GameObject.")]
    public XRRayInteractor leftTeleportationRay;

    [Tooltip("XR Ray Interactor attached to Right Teleport Interactor GameObject.")]
    public XRRayInteractor rightTeleportationRay;

    [Header("User Interface")]
    [Tooltip("Alignment Trigger for the wrist watch UI.")]
    public AlignmentTrigger wristWatchAlignmentTrigger;
    [Tooltip("GameObject for wrist watch UI.")]
    public GameObject wristWatchUI;

    private SnapTurnProvider _snapTurnProvider;
    private ContinuousTurnProvider _continuousTurnProvider;

    // Defaults cache
    private bool _defaultSmoothMotion;
    private bool _defaultTeleportation;
    private bool _defaultSmoothTurn;
    private float _defaultSnapTurnAmount;
    private float _defaultSmoothTurnSpeed;

    void Start()
    {
        _snapTurnProvider = GetComponentInChildren<SnapTurnProvider>();
        _continuousTurnProvider = GetComponentInChildren<ContinuousTurnProvider>();

        if (!_snapTurnProvider || !_continuousTurnProvider)
        {
            Debug.LogError("Unable to find necessary turn providers. Make sure the XR Controls Manager is attached to the XR Origin or its parent GameObject.");
        }
    }

    public void StartSetUp()
    {
        if (wristWatchUI)
            wristWatchUI.SetActive(false);

        RecordDefaults();
        
        if (disableMovementOnStart)
            DisableAllControls();
        
        UpdateValues();
    }

    private void UpdateValues()
    {
        if (rightControllerInputManager)
            rightControllerInputManager.smoothTurnEnabled = smoothTurnEnabled;
        else
            Debug.LogWarning("Right Controller Input Action Manager not assigned in the inspector.");

        if (_snapTurnProvider)
            _snapTurnProvider.turnAmount = snapTurnAmount;

        if (_continuousTurnProvider)
            _continuousTurnProvider.turnSpeed = smoothTurnSpeed;

        if (leftControllerInputManager)
            leftControllerInputManager.smoothMotionEnabled = smoothMotionEnabled;
        else
            Debug.LogWarning("Left Controller Input Action Manager not assigned in the inspector.");

        if (rightTeleportationRay)
            rightTeleportationRay.enabled = teleportationEnabled;
        else
            Debug.LogWarning("Right Teleportation Ray not assigned in the inspector.");

        if (leftTeleportationRay)
            leftTeleportationRay.enabled = teleportationEnabled;
        else
            Debug.LogWarning("Left Teleportation Ray not assigned in the inspector.");

        InterfaceUpdate[] interfaces = FindObjectsByType<InterfaceUpdate>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (InterfaceUpdate iface in interfaces)
            iface.UpdateValues();
    }

    #region Defaults Handling

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

    public void RestoreDefaults()
    {
        //Debug.Log("Restore Defaults called.");
        smoothMotionEnabled = _defaultSmoothMotion;
        teleportationEnabled = _defaultTeleportation;
        smoothTurnEnabled = _defaultSmoothTurn;
        snapTurnAmount = _defaultSnapTurnAmount;
        smoothTurnSpeed = _defaultSmoothTurnSpeed;

        if (wristWatchAlignmentTrigger)
            wristWatchAlignmentTrigger.enabled = true;

        UpdateValues();
    }

    #endregion

    #region Disable All Controls

    public void DisableAllControls()
    {
        //Debug.Log("Disable All Controls Called.");
        smoothMotionEnabled = false;
        teleportationEnabled = false;
        smoothTurnEnabled = false;

        if (wristWatchAlignmentTrigger)
            wristWatchAlignmentTrigger.enabled = false;

        UpdateValues();
    }

    #endregion

    #region Public Setters

    public void SetSmoothMotion(bool isOn)
    {
        smoothMotionEnabled = isOn;
        UpdateValues();
    }

    public void SetSmoothTurn(bool isOn)
    {
        smoothTurnEnabled = isOn;
        UpdateValues();
    }

    public void SetTeleportation(bool isOn)
    {
        teleportationEnabled = isOn;
        UpdateValues();
    }

    public void SetSnapTurnAmount(float amount)
    {
        snapTurnAmount = amount;
        UpdateValues();
    }

    public void SetSmoothTurnSpeed(float speed)
    {
        smoothTurnSpeed = speed;
        UpdateValues();
    }

    public void SetSmoothTurnFromDropdown(Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                SetSmoothTurn(false);
                break;
            default:
                SetSmoothTurn(true);
                break;
        }
    }

    #endregion
}

public static class XRControlsDefaultsCache
{
    public static bool HasCached { get; private set; }
    public static bool SmoothMotion;
    public static bool Teleportation;
    public static bool SmoothTurn;
    public static float SnapTurnAmount;
    public static float SmoothTurnSpeed;

    public static void Cache(bool smoothMotion, bool teleportation, bool smoothTurn, float snapAmount, float smoothSpeed)
    {
        if (HasCached) return;
        SmoothMotion = smoothMotion;
        Teleportation = teleportation;
        SmoothTurn = smoothTurn;
        SnapTurnAmount = snapAmount;
        SmoothTurnSpeed = smoothSpeed;
        HasCached = true;
    }
}