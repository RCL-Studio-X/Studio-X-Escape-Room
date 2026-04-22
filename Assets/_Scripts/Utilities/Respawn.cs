using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Respawns this object back to its original transform when it is no longer overlapping the assigned safe zone collider.
    /// </summary>
    public class Respawn : MonoBehaviour
    {
        #region Public Variable Declarations

        [Tooltip("The collider that detects when this object is outside of the safe zone. If left empty, the script will attempt to use a collider attached to this GameObject.")]
        public Collider detectionCollider;
        [Tooltip("The boundary that defines the safe zone.")]
        public Boundary boundary;

        #endregion

        #region Private Variable Declarations

        /// <summary> The collider that defines the safe zone. If this object is no longer intersecting with this collider, it will respawn. </summary>
        private Collider _safeZone;

        /// <summary> The original world position of this object at startup. </summary>
        private Vector3 _originalPosition;

        /// <summary> The original world rotation of this object at startup. </summary>
        private Quaternion _originalRotation;

        /// <summary> The original local scale of this object at startup. </summary>
        private Vector3 _originalScale;

        /// <summary> The collider attached to this object. </summary>
        private Collider _myCollider;

        /// <summary> The rigidbody attached to this object, if one exists. </summary>
        private Rigidbody _rigidbody;

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Stores the original transform values of this object and caches required components.
        /// </summary>
        private void Start()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
            _originalScale = transform.localScale;

            if (detectionCollider != null)
            {
                _myCollider = detectionCollider;
            } else
            {
                _myCollider = GetComponent<Collider>();
                if (_myCollider == null)
                {
                    Debug.LogError("Respawn script requires a Collider component on the same GameObject or an assigned detectionCollider. Please add a Collider or assign one in the inspector for " + name + ".");
                }
            }

            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                Debug.LogWarning("No Rigidbody component found on this GameObject: " + name + ". Velocity will not be reset on respawn.");
            }

            if (boundary != null)
            {
                _safeZone = boundary.GetSafeZone();
            } else
            {
                Debug.LogError("No Boundary assigned to Respawn script. Please assign a Boundary GameObject to" + name + ".");
            }
        }

        /// <summary>
        /// Checks whether this object is still intersecting the safe zone.
        /// If not, the object is respawned to its original position.
        /// </summary>
        private void Update()
        {
            if (_safeZone == null || _myCollider == null)
            {
                return;
            }

            if (!_myCollider.bounds.Intersects(_safeZone.bounds))
            {
                RespawnObject();
            }
        }

        /// <summary>
        /// Resets this object to its original transform values and clears its physics velocity if a rigidbody is present.
        /// </summary>
        private void RespawnObject()
        {
            transform.SetPositionAndRotation(_originalPosition, _originalRotation);
            transform.localScale = _originalScale;

            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        #endregion
    }
}
