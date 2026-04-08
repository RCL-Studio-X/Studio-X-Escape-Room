using UnityEngine;

namespace StudioXRCL.EscapeRoom.Maze
{
    public class MazeController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The actual maze board that the marble rolls on.")]
        public Transform mazeBoard;
        
        [Tooltip("The invisible object with the XR Grab Interactable.")]
        public Transform invisibleGrabbable;
        
        [Tooltip("The physical 3D handle the player sees on the table.")]
        public Transform visualHandle;
        
        [Tooltip("An empty GameObject marking where the handle should rest.")]
        public Transform resetAnchor;

        [Header("Settings")]
        [Tooltip("The maximum degrees the board can tilt in any direction.")]
        public float maxTiltAngle = 20f;
        
        [Tooltip("How fast the handle snaps back when released.")]
        public float resetSpeed = 5f;

        private bool isGrabbed = false;

        void Update()
        {
            if (isGrabbed)
            {
                // 1. Get the raw rotation from the invisible handle (player's hand)
                Vector3 rawEuler = invisibleGrabbable.localEulerAngles;

                // 2. Normalize and Clamp the angles so it doesn't flip upside down
                float clampedX = Mathf.Clamp(NormalizeAngle(rawEuler.x), -maxTiltAngle, maxTiltAngle);
                float clampedZ = Mathf.Clamp(NormalizeAngle(rawEuler.z), -maxTiltAngle, maxTiltAngle);

                // 3. Apply the safe, clamped rotation to the Board and the Visual Handle
                // We leave Y at 0 so the board doesn't spin like a record!
                Quaternion safeRotation = Quaternion.Euler(-clampedX, 0f, -clampedZ);
                mazeBoard.localRotation = safeRotation;
                visualHandle.localRotation = safeRotation;
            }
            else
            {
                // 4. Smoothly snap the invisible handle back to the center when released
                invisibleGrabbable.position = Vector3.Lerp(invisibleGrabbable.position, resetAnchor.position, Time.deltaTime * resetSpeed);
                invisibleGrabbable.rotation = Quaternion.Lerp(invisibleGrabbable.rotation, resetAnchor.rotation, Time.deltaTime * resetSpeed);
                
                // Optional: Slowly level the board back out to flat when they let go
                mazeBoard.localRotation = Quaternion.Lerp(mazeBoard.localRotation, Quaternion.identity, Time.deltaTime * resetSpeed);
                visualHandle.localRotation = mazeBoard.localRotation;
            }
        }

        // Hook this to XR Grab Interactable -> Select Entered
        public void OnHandleGrabbed()
        {
            isGrabbed = true;
        }

        // Hook this to XR Grab Interactable -> Select Exited
        public void OnHandleReleased()
        {
            isGrabbed = false;
        }

        // Helper function to fix Unity's 360-degree wrap-around math
        private float NormalizeAngle(float angle)
        {
            if (angle > 180f) return angle - 360f;
            return angle;
        }
    }
}