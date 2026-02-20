using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    ///<summary>
    /// idk 
    /// </summary>
    /// test
}

public class RespawnOnExitArea : MonoBehaviour
{
    [Tooltip("only respawn when exiting a trigger with this tag. Leave empty to respawn on any trigger exit.")]
    public string boundaryTag = "Playarea";

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;

    private Rigidbody _rb;

    void Start()
    {
        // Record starting transform
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;

        // Cache Rigidbody (if it exists)
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        // If boundaryTag is set, only trigger respawn when leaving THAT tagged trigger
        if (!string.IsNullOrEmpty(boundaryTag) && !other.CompareTag(boundaryTag))
            return;

        // Reset transform
        transform.SetPositionAndRotation(_originalPosition, _originalRotation);
        transform.localScale = _originalScale;

        // Reset velocity if Rigidbody exists
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}