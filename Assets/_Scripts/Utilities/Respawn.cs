using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Respawns this object back to its original transform when it is no longer overlapping the assigned safe zone collider.
    /// </summary>
    public class Respawn : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Respawn Settings")]

        [Tooltip("The collider that defines the safe zone. If this object is no longer intersecting with this collider, it will respawn.")]
        public Collider safeZone;

        #endregion

        #region Private Variable Declarations

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

            _myCollider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Checks whether this object is still intersecting the safe zone.
        /// If not, the object is respawned to its original position.
        /// </summary>
        private void Update()
        {
            if (safeZone == null || _myCollider == null)
            {
                return;
            }

            if (!_myCollider.bounds.Intersects(safeZone.bounds))
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