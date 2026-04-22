using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    [RequireComponent(typeof(Collider))]
    /// <summary>
    /// Dummy class to define the boundary of the safe zone for the Respawn script.
    /// </summary>
    public class Boundary : MonoBehaviour
    {
        /// <summary>
        /// Caches the collider component on this GameObject to be used as the safe zone boundary for the Respawn script.
        /// </summary>
        private Collider _safeZone;

        /// <summary>
        /// Unity Start method. Caches the collider component on this GameObject and ensures it is set as a trigger. Logs an error if no collider is found.
        /// </summary>
        private void Start()
        {
            _safeZone = GetComponent<Collider>();
            _safeZone.isTrigger = true;
            if (_safeZone == null)
            {
                Debug.LogError("Boundary script requires a Collider component on the same GameObject: " + name + ".");
            }
        }

        public Collider GetSafeZone()
        {
            return _safeZone;
        }
    }
}

