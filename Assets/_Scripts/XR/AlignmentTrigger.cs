using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows triggering actions through the <see cref="OnEnterAligned"/> / <see cref="OnExitAligned"/> events
/// when the list of required vector alignments is met.
/// For example, it can be used to trigger an event when the player's view aligns with an object.
/// </summary>
public class AlignmentTrigger : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;

    public enum Mode
    {
        View,
        World
    }

    [Serializable]
    public class AxisMatch
    {
        public Mode externalAxisMode;
        public Vector3 localAxis;
        public Vector3 externalAxis;

        [Range(0.0f, 1.0f)] public float tolerance = 0.3f;
    }

    [SerializeField] private AxisMatch[] requiredMatch;
    [SerializeField] private UnityEvent onEnterAligned;
    [SerializeField] private UnityEvent onExitAligned;

    private bool _wasAligned;

    private void Update()
    {
        bool allMatch = true;

        for (int i = 0; i < requiredMatch.Length && allMatch; i++)
        {
            AxisMatch match = requiredMatch[i];
            Vector3 worldLocal = transform.TransformVector(match.localAxis);
            Vector3 worldExternal = match.externalAxisMode == Mode.View
                ? _playerCamera.transform.TransformVector(match.externalAxis)
                : match.externalAxis;

            float dot = Vector3.Dot(worldLocal.normalized, worldExternal.normalized);

            //Debug.Log($"Match {i}: dot={dot:F3} (needs > {match.tolerance:F3})");

            allMatch &= dot > match.tolerance;
        }

        if (allMatch)
        {
            //Debug.Log($"All match is TRUE. _wasAligned={_wasAligned}");
            if (!_wasAligned)
            {
                onEnterAligned.Invoke();
                //Debug.Log("Entered aligned");
                _wasAligned = true;
            }
        }
        else
        {
            //Debug.Log($"All match is FALSE. _wasAligned={_wasAligned}");
            if (_wasAligned)
            {
                onExitAligned.Invoke();
                //Debug.Log("Exited aligned");
                _wasAligned = false;
            }
        }

    }
}
