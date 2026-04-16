using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// A simple utility script that invokes a UnityEvent when the script starts. 
    /// This can be used to trigger actions at the beginning of a scene or when an object is instantiated.
    /// </summary>
    public class OnStart : MonoBehaviour
    {
        [Header("Unity Events")]
        [Tooltip("Event triggered when the script starts.")]
        public UnityEvent onStart;
        
        /// <summary>
        /// Invokes the onStart event when the script starts.
        /// </summary>
        void Start()
        {
            onStart.Invoke();
        }
    }
}
