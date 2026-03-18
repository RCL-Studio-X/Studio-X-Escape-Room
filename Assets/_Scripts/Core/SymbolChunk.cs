using UnityEngine;
using StudioXRCL.EscapeRoom.Utilities;

namespace StudioXRCL.EscapeRoom.Core
{
    [DisallowMultipleComponent]
    public class SymbolChunk : MonoBehaviour
    {
        [Tooltip("The symbol type represented by this chunk. This should correspond to the symbols expected by the PuzzleRock and RockSockets.")]
        [SerializeField] private SymbolType symbol = SymbolType.None;
        public SymbolType Symbol => symbol;

        private Vector3 _spawnPos;
        private Quaternion _spawnRot;
        private Transform _spawnParent;

        private Rigidbody rb;

        /// <summary>
        /// Initializes the Rigidbody and caches the spawn pose.
        /// </summary>
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            CacheSpawnPose();
        }

        /// <summary>
        /// Caches the current position, rotation, and parent transform.
        /// </summary>
        public void CacheSpawnPose()
        {
            _spawnParent = transform.parent;
            _spawnPos = transform.position;
            _spawnRot = transform.rotation;
        }

        /// <summary>
        /// Respawns the chunk to its cached pose, resetting physics if applicable.
        /// </summary>
        public void RespawnToCachedPose()
        {
            if (rb != null)
            {
                bool wasKinematic = rb.isKinematic;

                // Temporarily disable kinematic to safely reset velocity
                rb.isKinematic = false;

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                transform.SetPositionAndRotation(_spawnPos, _spawnRot);

                rb.Sleep();

                // Restore original state
                rb.isKinematic = wasKinematic;
            }
            else
            {
                transform.SetPositionAndRotation(_spawnPos, _spawnRot);
            }
        }

        /// <summary>
        /// Sets the symbol type for this chunk.
        /// </summary>
        // Optional: for design-time quick random assignment.
        public void SetSymbol(SymbolType newSymbol) => symbol = newSymbol;
    }
}
