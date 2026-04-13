using UnityEngine;

namespace StudioXRCL.EscapeRoom.Maze
{
    public class MazeController : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The maximum degrees the board can tilt in any direction.")]
        public float maxTiltAngle = 20f;
        
        [Tooltip("How fast the board snaps back to flat when released.")]
        public float resetSpeed = 5f;

        // NEW: How fast the board follows your hand. Lower = slower and heavier.
        [Tooltip("How fast the board follows the hand. Lower is smoother.")]
        public float followSpeed = 8f; 

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

                // 3. Define the target rotation (remember to keep your negative signs if you needed them!)
                Quaternion targetRotation = Quaternion.Euler(-clampedX, 0f, -clampedZ);

                // 4. NEW: Smoothly Lerp the board to the target rotation so it feels heavy and doesn't break physics!
                mazeBoard.localRotation = Quaternion.Lerp(mazeBoard.localRotation, targetRotation, Time.deltaTime * followSpeed);
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
}
}