using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Core
{
    public class HoleTrigger : MonoBehaviour
    {
        // 1. Define the enum 
        public enum HoleType
        {
            Trap,
            Win
        }

        [Header("Hole Configuration")]
        [Tooltip("Select what happens when the marble enters this hole.")]
        public HoleType holeType = HoleType.Trap; // Defaults to Trap

        [Header("Trap Settings")]
        [Tooltip("The exact name of the layer we created for the falling ball.")]
        public string ghostLayerName = "GhostBall";

        [Header("Win Settings")]
        [Tooltip("Fire this event when the ball enters the specific Win hole.")]
        public UnityEvent onPuzzleWon;

        private void OnTriggerEnter(Collider other)
        {
            // First, make sure it's actually the marble
            if (other.attachedRigidbody != null && other.CompareTag("Marble"))
            {
                // check  enum to see what kind of hole this is
                switch (holeType)
                {
                    case HoleType.Trap:
                        // Make the marble a ghost so it falls to the catch basin
                        other.gameObject.layer = LayerMask.NameToLayer(ghostLayerName);
                        break;

                    case HoleType.Win:
                        // Stop the marble and trigger the win event
                        Debug.Log("Maze Won!");
                        onPuzzleWon.Invoke();
                        other.attachedRigidbody.isKinematic = true; 
                        break;
                }
            }
        }
    }
}