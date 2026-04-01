using System.Collections.Generic; 
using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    public class CreateTrail : MonoBehaviour
    {
        [Header("References")]
        public GameObject trailPrefab;
        [Tooltip("Drag your invisible box collider tip here!")]
        public Transform chalkTip; 

        [Header("Board")]
        public LayerMask boardLayer;
        public float maxDistance = 5f;
        
        [Tooltip("How far back to pull the raycast so it doesn't get stuck inside the board.")]
        public float rayOriginBackOffset = 0.2f;

        [Header("Style")]
        public float width = 0.01f;
        public Color color = Color.black;

        private GameObject currentTrail;
        

        void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & boardLayer) != 0)
            {
                StartTrail();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & boardLayer) != 0)
            {
                EndTrail();
            }
        }

        void Update()
        {
            if (!currentTrail) return;
            
            Vector3 origin = chalkTip.position - (chalkTip.forward * rayOriginBackOffset);

            if (Physics.Raycast(origin, chalkTip.forward, out RaycastHit hit, maxDistance + rayOriginBackOffset, boardLayer))
            {
                currentTrail.transform.position = hit.point;
                currentTrail.transform.rotation = Quaternion.LookRotation(-hit.normal);
            }
        }

        public void StartTrail()
        {
            if (currentTrail) return;

            Vector3 origin = chalkTip.position - (chalkTip.forward * rayOriginBackOffset);

            if (Physics.Raycast(origin, chalkTip.forward, out RaycastHit hit, maxDistance + rayOriginBackOffset, boardLayer))
            {
                currentTrail = Instantiate(trailPrefab, hit.point, Quaternion.LookRotation(-hit.normal));
                
                // Grab the reset script from the board we just hit
                ChalkboardReset activeBoard = hit.collider.GetComponentInParent<ChalkboardReset>();
                
                // If the board has the script, put the ink in its specific container
                if (activeBoard != null && activeBoard.trailContainer != null)
                {
                    currentTrail.transform.SetParent(activeBoard.trailContainer);
                }

                ApplySettings(currentTrail);
            }
        }

        public void EndTrail()
        {
            currentTrail = null;
        }

        

        private void ApplySettings(GameObject trailObject)
        {
            var tr = trailObject.GetComponent<TrailRenderer>();

            tr.widthMultiplier = width;
            tr.startColor = color;
            tr.endColor = color;

            tr.alignment = LineAlignment.TransformZ;
            tr.time = Mathf.Infinity;
        }
    }
}