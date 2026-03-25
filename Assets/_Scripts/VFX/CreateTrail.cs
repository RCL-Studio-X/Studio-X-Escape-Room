using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    public class CreateTrail : MonoBehaviour
    {
        [Header("References")]
        public GameObject trailPrefab;

        [Header("Board")]
        public LayerMask boardLayer;

        [Header("Style")]
        public float width = 0.01f;
        public Color color = Color.black;

        private GameObject currentTrail;
        [Tooltip("Invisible tip GameObject")]
        public Transform chalkTip;

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

        public void StartTrail()
        {
            if (currentTrail)
                return;

            // Spawn the trail exactly at the chalk's tip
            currentTrail = Instantiate(trailPrefab, chalkTip.position, chalkTip.rotation);
        
            // Parent it to the chalkTip so it follows perfectly
            currentTrail.transform.SetParent(chalkTip);

            ApplySettings(currentTrail);
        }

        public void EndTrail()
        {
            if (currentTrail != null)
            {
                // Un-parent the trail so it stays permanently behind on the board
                currentTrail.transform.SetParent(null); 
                
                // Clear memory to draw a new trail
                currentTrail = null;
            }
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