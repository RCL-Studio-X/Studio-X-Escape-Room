using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    public class CreateTrail : MonoBehaviour
    {
        [Header("References")]
        public GameObject trailPrefab;

        [Header("Board")]
        public LayerMask boardLayer;
        public float maxDistance = 5f;

        [Header("Style")]
        public float width = 0.01f;
        public Color color = Color.black;

        private GameObject currentTrail;

        void Update()
        {
            if (!currentTrail)
                return;

            if (Physics.Raycast(transform.position,
                                transform.forward,
                                out RaycastHit hit,
                                maxDistance,
                                boardLayer))
            {
                // Stick to board
                currentTrail.transform.position = hit.point;

                // Orient to board surface
                currentTrail.transform.rotation =
                    Quaternion.LookRotation(-hit.normal);
            }
        }

        public void StartTrail()
        {
            if (currentTrail)
                return;

            if (Physics.Raycast(transform.position,
                                transform.forward,
                                out RaycastHit hit,
                                maxDistance,
                                boardLayer))
            {
                currentTrail = Instantiate(trailPrefab, hit.point, Quaternion.identity);
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

            // Important: keeps it flat
            tr.alignment = LineAlignment.TransformZ;
            tr.time = Mathf.Infinity;
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Chalkboard"))
            {
                StartTrail();
            }

            Debug.Log("Collision Entered with " + other.gameObject.name);
        }

        void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Chalkboard"))
            {
                EndTrail();
            }

            Debug.Log("Collision Exited with " + other.gameObject.name);
        }

    }
}
