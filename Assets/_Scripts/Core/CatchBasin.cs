using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    public class CatchBasin : MonoBehaviour
    {
        [Header("Respawn Setup")]
        public Transform startPoint;
        public string normalLayerName = "Default"; // Changed to string and added quotes!

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody ballRb = other.attachedRigidbody;

            // Catch the marble when it falls out of the bottom of the table
            if (ballRb != null && other.CompareTag("Marble"))
            {
                // Stop momentum
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;

                // Teleport back up top
                other.transform.position = startPoint.position;

                // Make it solid again so it lands on the board
                other.gameObject.layer = LayerMask.NameToLayer(normalLayerName);
            }
        }
    }
}