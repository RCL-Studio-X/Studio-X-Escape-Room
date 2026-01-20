using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Attaches to a visual object and follows a physics-driven target by predicting
    /// its position using Rigidbody velocity. This reduces visible lag between
    /// FixedUpdate physics movement and rendered frames.
    /// </summary>
    public class FollowPhysics : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Physics Target")]
        [Tooltip("The Rigidbody component of the invisible physics-driven object.")]
        public Rigidbody physicsTargetRigidbody;

        [Header("Prediction Settings")]
        [Tooltip("Multiplier used to tweak the position prediction amount. 1.0 is usually correct.")]
        public float predictionFactor = 1.0f;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Cached transform of the physics target for efficiency.
        /// </summary>
        private Transform _targetTransform;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches required references and validates setup.
        /// </summary>
        private void Start()
        {
            if (physicsTargetRigidbody == null)
            {
                Debug.LogError(
                    $"{nameof(FollowPhysics)}: Rigidbody is not assigned. " +
                    "Please assign the physics target Rigidbody in the inspector."
                );
                enabled = false;
                return;
            }

            _targetTransform = physicsTargetRigidbody.transform;
        }

        /// <summary>
        /// Updates the visual object's transform after physics simulation,
        /// predicting the target position based on velocity.
        /// </summary>
        private void LateUpdate()
        {
            if (!_targetTransform)
                return;

            // Time elapsed since last FixedUpdate
            float timeSincePhysics = Time.time - Time.fixedTime;

            // Predict position based on current velocity
            Vector3 velocity = physicsTargetRigidbody.linearVelocity;
            Vector3 predictedPosition =
                _targetTransform.position +
                (velocity * timeSincePhysics * predictionFactor);

            transform.position = predictedPosition;
            transform.rotation = _targetTransform.rotation;
        }

        #endregion
    }
}
