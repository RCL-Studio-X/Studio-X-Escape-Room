using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    public class CreateTrail : MonoBehaviour
    {
        [Header("References")]
        public GameObject trailPrefab;
        public Transform chalkTip;

        [Header("Board")]
        public LayerMask boardLayer;

        [Header("Style")]
        public float width = 0.01f;
        public Color color = Color.black;

        private GameObject currentTrail;
        private bool isDrawing = false; 

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
            if (currentTrail) return;
            
            currentTrail = Instantiate(trailPrefab, chalkTip.position, chalkTip.rotation);
            ApplySettings(currentTrail);
            
            isDrawing = true; 
        }

        public void EndTrail()
        {
            isDrawing = false; // Turn off the Update loop
            currentTrail = null;
        }

        void Update() 
        {
            if (isDrawing && currentTrail != null)
            {
                currentTrail.transform.position = chalkTip.position;
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