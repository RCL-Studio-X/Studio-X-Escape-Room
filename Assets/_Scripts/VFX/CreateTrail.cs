using System.Collections.Generic;
using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    /// <summary>
    /// Handles the raycasting and instantiation logic to draw chalk trails on valid surfaces.
    /// </summary>
    public class CreateTrail : MonoBehaviour
    {
        #region Public Variable declarations

        [Header("References")]
        [Tooltip("The prefab used to generate the chalk trail.")]
        public GameObject trailPrefab;

        [Tooltip("Drag your invisible box collider tip here!")]
        public Transform chalkTip;

        [Header("Board")]
        [Tooltip("The layer mask representing writable chalkboards.")]
        public LayerMask boardLayer;

        [Tooltip("The maximum distance the raycast will check for a board.")]
        public float maxDistance = 5f;

        [Tooltip("How far back to pull the raycast so it doesn't get stuck inside the board.")]
        public float rayOriginBackOffset = 0.2f;

        [Header("Style")]
        [Tooltip("The width of the drawn trail.")]
        public float width = 0.01f;

        [Tooltip("The color of the drawn trail.")]
        public Color color = Color.black;

        #endregion

        #region Private Variable declarations

        /// <summary> The current active trail object being drawn. </summary>
        private GameObject _currentTrail;

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Begins drawing a new trail if the chalk tip is against a valid board.
        /// </summary>
        public void StartTrail()
        {
            if (_currentTrail) return;

            Vector3 origin = chalkTip.position - (chalkTip.forward * rayOriginBackOffset);

            if (Physics.Raycast(origin, chalkTip.forward, out RaycastHit hit, maxDistance + rayOriginBackOffset, boardLayer))
            {
                _currentTrail = Instantiate(trailPrefab, hit.point, Quaternion.LookRotation(-hit.normal));

                // Grab the reset script from the board we just hit
                ChalkboardReset activeBoard = hit.collider.GetComponentInParent<ChalkboardReset>();

                // If the board has the script, put the ink in its specific container
                if (activeBoard != null && activeBoard.trailContainer != null)
                {
                    _currentTrail.transform.SetParent(activeBoard.trailContainer);
                }

                ApplySettings(_currentTrail);
            }
        }

        /// <summary>
        /// Ends the current trail being drawn.
        /// </summary>
        public void EndTrail()
        {
            _currentTrail = null;
        }

        #endregion

        #region Private Method definitions

        /// <summary>
        /// Called when the chalk tip collider enters a trigger.
        /// </summary>
        /// <param name="other">The collider that was entered.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & boardLayer) != 0)
            {
                StartTrail();
            }
        }

        /// <summary>
        /// Called when the chalk tip collider exits a trigger.
        /// </summary>
        /// <param name="other">The collider that was exited.</param>
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & boardLayer) != 0)
            {
                EndTrail();
            }
        }

        /// <summary>
        /// Updates the position and rotation of the current trail frame by frame.
        /// </summary>
        private void Update()
        {
            if (!_currentTrail) return;

            Vector3 origin = chalkTip.position - (chalkTip.forward * rayOriginBackOffset);

            if (Physics.Raycast(origin, chalkTip.forward, out RaycastHit hit, maxDistance + rayOriginBackOffset, boardLayer))
            {
                _currentTrail.transform.position = hit.point;
                _currentTrail.transform.rotation = Quaternion.LookRotation(-hit.normal);
            }
        }

        /// <summary>
        /// Applies visual settings to the newly instantiated trail.
        /// </summary>
        /// <param name="trailObject">The trail game object to modify.</param>
        private void ApplySettings(GameObject trailObject)
        {
            var tr = trailObject.GetComponent<TrailRenderer>();

            tr.widthMultiplier = width;
            tr.startColor = color;
            tr.endColor = color;

            tr.alignment = LineAlignment.TransformZ;
            tr.time = Mathf.Infinity;
        }

        #endregion
    }
}