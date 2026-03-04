using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Spawns droplet prefabs at a specified position.
    /// </summary>
    public class DropletEffect : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Droplet Settings")]

        [Tooltip("Prefab to spawn as a droplet.")]
        public GameObject prefab;

        [Tooltip("World position where droplets will spawn.")]
        public Vector3 spawnPosition = new Vector3(-1f, 2f, -1f);

        [Tooltip("Speed applied to the spawned droplet if it has a Rigidbody.")]
        public float speed = 1f;

        #endregion

        #region Private Variable Declarations

        /// <summary> Cached reference to this transform. </summary>
        private Transform _transform;

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Unity Awake method. Caches required component references.
        /// </summary>
        private void Awake()
        {
            _transform = transform;
        }

        /// <summary>
        /// Unity Update method. Spawns a droplet prefab at the configured position.
        /// </summary>
        private void Update()
        {
            SpawnDroplet();
        }

        /// <summary>
        /// Instantiates the droplet prefab and applies speed if a Rigidbody is present.
        /// </summary>
        private void SpawnDroplet()
        {
            if (prefab == null)
            {
                return;
            }

            GameObject dropletInstance = Instantiate(prefab, spawnPosition, prefab.transform.rotation);

            Rigidbody rb = dropletInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.down * speed;
            }
        }

        #endregion
    }
}