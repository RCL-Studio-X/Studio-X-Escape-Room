using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Ensures that this GameObject is selected in the Unity Editor
    /// when any of its child objects are clicked.
    /// </summary>
    [SelectionBase]
    public class SelectBase : MonoBehaviour
    {
        // Intentionally empty.
        // This component exists solely to control editor selection behavior.
    }
}
