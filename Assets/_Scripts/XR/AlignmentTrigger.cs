using System;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.XR
{
    /// <summary>
    /// Invokes events when a set of required vector alignments are met or no longer met.
    /// Common use cases include detecting when the player's view aligns with an object
    /// or when an object aligns with a world-space direction.
    /// </summary>
    public class AlignmentTrigger : MonoBehaviour
    {
        #region Nested Types

        /// <summary>
        /// Defines how the external axis should be interpreted.
        /// </summary>
        public enum Mode
        {
            View,
            World
        }

        /// <summary>
        /// Defines a single axis alignment requirement.
        /// </summary>
        [Serializable]
        public class AxisMatch
        {
            [Tooltip("Determines whether the external axis is evaluated in view space or world space.")]
            public Mode externalAxisMode;

            [Tooltip("Local-space axis on this object that must align.")]
            public Vector3 localAxis;

            [Tooltip("External axis to compare against.")]
            public Vector3 externalAxis;

            [Tooltip("Minimum dot product required for this axis to be considered aligned.")]
            [Range(0.0f, 1.0f)]
            public float tolerance = 0.3f;
        }

        #endregion

        #region Public Variable Declarations

        [Header("Alignment Settings")]
        [Tooltip("Camera used when evaluating view-space alignment.")]
        [SerializeField] private Camera playerCamera;

        [Tooltip("All axis alignment conditions that must be satisfied.")]
        [SerializeField] private AxisMatch[] requiredMatches;

        [Header("Events")]
        [Tooltip("Invoked when all alignment conditions become satisfied.")]
        [SerializeField] private UnityEvent onEnterAligned;

        [Tooltip("Invoked when alignment conditions are no longer satisfied.")]
        [SerializeField] private UnityEvent onExitAligned;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Tracks whether the object was aligned during the previous frame.
        /// </summary>
        private bool _wasAligned;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Evaluates alignment conditions every frame and invokes events
        /// when alignment state changes.
        /// </summary>
        private void Update()
        {
            bool allMatch = true;

            for (int i = 0; i < requiredMatches.Length && allMatch; i++)
            {
                AxisMatch match = requiredMatches[i];

                Vector3 worldLocalAxis = transform.TransformVector(match.localAxis);
                Vector3 worldExternalAxis = match.externalAxisMode == Mode.View
                    ? playerCamera.transform.TransformVector(match.externalAxis)
                    : match.externalAxis;

                float dot = Vector3.Dot(worldLocalAxis.normalized, worldExternalAxis.normalized);
                allMatch &= dot > match.tolerance;
            }

            if (allMatch)
            {
                if (!_wasAligned)
                {
                    onEnterAligned?.Invoke();
                    _wasAligned = true;
                }
            }
            else
            {
                if (_wasAligned)
                {
                    onExitAligned?.Invoke();
                    _wasAligned = false;
                }
            }
        }

        #endregion
    }
}
