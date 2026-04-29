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
        [Tooltip("Boundaries that define the safe zone. If this object is not intersecting ANY of their safe-zone colliders, it will respawn.")]
        public Boundary[] boundaries;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Safe-zone colliders gathered from <see cref="boundaries"/>. If this object is no longer intersecting ANY of these colliders, it will respawn.
        /// </summary>
        private Collider[] _safeZones;

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

            if (boundaries != null && boundaries.Length > 0)
            {
                int safeZoneCount = 0;
                for (int i = 0; i < boundaries.Length; i++)
                {
                    Boundary boundary = boundaries[i];
                    if (boundary == null)
                    {
                        continue;
                    }

                    Collider safeZone = boundary.GetSafeZone();
                    if (safeZone != null)
                    {
                        safeZoneCount++;
                    }
                }

                if (safeZoneCount == 0)
                {
                    Debug.LogError("Boundaries were assigned, but none produced a valid safe zone collider for " + name + ". Please ensure each Boundary has a valid safe zone collider.");
                    _safeZones = null;
                }
                else
                {
                    _safeZones = new Collider[safeZoneCount];
                    int writeIndex = 0;
                    for (int i = 0; i < boundaries.Length; i++)
                    {
                        Boundary boundary = boundaries[i];
                        if (boundary == null)
                        {
                            continue;
                        }

                        Collider safeZone = boundary.GetSafeZone();
                        if (safeZone == null)
                        {
                            continue;
                        }

                        _safeZones[writeIndex] = safeZone;
                        writeIndex++;
                    }
                }
            } else
            {
                Debug.LogError("No Boundaries assigned to Respawn script. Please assign at least one Boundary GameObject to " + name + ".");
            }
        }

        /// <summary>
        /// Checks whether this object is still intersecting the safe zone.
        /// If not, the object is respawned to its original position.
        /// </summary>
        private void Update()
        {
            if (_safeZones == null || _safeZones.Length == 0 || _myCollider == null)
            {
                return;
            }

            bool isIntersectingAnySafeZone = false;
            for (int i = 0; i < _safeZones.Length; i++)
            {
                Collider safeZone = _safeZones[i];
                if (safeZone == null)
                {
                    continue;
                }

                if (_myCollider.bounds.Intersects(safeZone.bounds))
                {
                    isIntersectingAnySafeZone = true;
                    break;
                }
            }

            if (!isIntersectingAnySafeZone)
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
