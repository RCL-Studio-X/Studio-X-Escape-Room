using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Toggles the active state of a list of GameObjects.
/// </summary>
public class ToggleActiveState : MonoBehaviour
{
    [Tooltip("Objects to toggle on/off")]
    public List<GameObject> targets = new List<GameObject>();

    [Tooltip("If true, targets will be active.")]
    public bool isOn = false;

    private void Start()
    {
        ApplyState();
    }

    public void TurnOn()
    {
        isOn = true;
        ApplyState();
    }

    public void TurnOff()
    {
        isOn = false;
        ApplyState();
    }

    public void Flip()
    {
        isOn = !isOn;
        ApplyState();
    }

    private void ApplyState()
    {
        foreach (var obj in targets)
        {
            if (obj != null)
                obj.SetActive(isOn);
        }
    }

    private void OnValidate()
    {
        ApplyState();
    }
}