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

        private Transform activeBoard; 

        void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & boardLayer) != 0)
            {
                activeBoard = other.transform; 
                StartTrail();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & boardLayer) != 0)
            {
                activeBoard = null; 
                EndTrail();
            }
        }

        void Update() 
        {
            if (isDrawing && currentTrail != null && activeBoard != null)
            {
                Vector3 localTipPos = activeBoard.InverseTransformPoint(chalkTip.position);
                localTipPos.z = 0.001f; 
                currentTrail.transform.position = activeBoard.TransformPoint(localTipPos);
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
            isDrawing = false; 
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