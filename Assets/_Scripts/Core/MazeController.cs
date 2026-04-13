using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
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
        
        [Tooltip("How fast the board snaps back to flat when released.")]
        public float resetSpeed = 5f;

        private bool isGrabbed = false;

        void Update()
        {
            if (isGrabbed)
            {
                // 1. Get raw rotation from hand
                Vector3 rawEuler = invisibleGrabbable.localEulerAngles;

                // 2. Clamp it
                float clampedX = Mathf.Clamp(NormalizeAngle(rawEuler.x), -maxTiltAngle, maxTiltAngle);
                float clampedZ = Mathf.Clamp(NormalizeAngle(rawEuler.z), -maxTiltAngle, maxTiltAngle);

                // 3. Define the target rotation (with inverted axes)
                Quaternion targetRotation = Quaternion.Euler(-clampedX, 0f, -clampedZ);

                // 4. Directly apply 1:1 rotation (No follow speed or smoothing)
                mazeBoard.localRotation = targetRotation;
                visualHandle.localRotation = mazeBoard.localRotation;
            }
            else
            {
                // Snap invisible handle back
                invisibleGrabbable.position = Vector3.Lerp(invisibleGrabbable.position, resetAnchor.position, Time.deltaTime * resetSpeed);
                invisibleGrabbable.rotation = Quaternion.Lerp(invisibleGrabbable.rotation, resetAnchor.rotation, Time.deltaTime * resetSpeed);
                
                // Level the board out
                mazeBoard.localRotation = Quaternion.Lerp(mazeBoard.localRotation, Quaternion.identity, Time.deltaTime * resetSpeed);
                visualHandle.localRotation = mazeBoard.localRotation;
            }
        }

        public void OnHandleGrabbed()
        {
            isGrabbed = true;
        }

        public void OnHandleReleased()
        {
            isGrabbed = false;
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > 180f) return angle - 360f;
            return angle;
        }
    }
}