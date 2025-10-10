using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InterfaceUpdate: MonoBehaviour
{
    public Dropdown smoothSnapDropdown;
    public Toggle continuousMoveToggle;
    public Toggle teleportationToggle;
    
    private XRControlsManager _xrControlsManager;

    private void Start()
    {
        _xrControlsManager = FindFirstObjectByType<XRControlsManager>();
        if (!_xrControlsManager)
            Debug.LogError("Unable to find XRControlsManager");

        UpdateValues();
    }

    public void UpdateValues()
    {
        if (!_xrControlsManager) return;

        if (continuousMoveToggle)
            continuousMoveToggle.isOn = _xrControlsManager.smoothMotionEnabled;

        if (teleportationToggle)
            teleportationToggle.isOn = _xrControlsManager.teleportationEnabled;

        SetDropdownValue();
    }

    private void SetDropdownValue()
    {
        if (smoothSnapDropdown)
            smoothSnapDropdown.value = _xrControlsManager.smoothTurnEnabled ? 1 : 0;
    }

}
