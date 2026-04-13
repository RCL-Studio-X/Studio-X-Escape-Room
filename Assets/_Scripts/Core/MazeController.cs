using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    public class MazeController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The actual maze board that the marble rolls on.")]
        public Transform mazeBoard; 
        
        [Tooltip("NEW: The Kinematic Rigidbody attached to your MazeBoard!")]
        public Rigidbody boardRb; 
        
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

        [Tooltip("How sensitive the board is to your hand movement. Higher = tilts more with less physical movement.")]
        public float movementSensitivity = 100f; 

        private bool isGrabbed = false;

        // NEW: FixedUpdate must be used when moving Rigidbodies!
        void FixedUpdate() 
        {
            if (isGrabbed)
            {
                // 1. Calculate how far your hand has MOVED from the center anchor (Joystick style)
                Vector3 positionalOffset = invisibleGrabbable.position - resetAnchor.position;

                // 2. Convert that physical movement into tilt angles. 
                // (Moving forward on Z tilts the board around the X axis. Moving right on X tilts around Z).
                float targetRotX = positionalOffset.z * movementSensitivity;
                float targetRotZ = positionalOffset.x * movementSensitivity;

                // 3. Clamp the angles so the board doesn't flip upside down
                targetRotX = Mathf.Clamp(targetRotX, -maxTiltAngle, maxTiltAngle);
                targetRotZ = Mathf.Clamp(targetRotZ, -maxTiltAngle, maxTiltAngle);

                // 4. Create the final rotation. 
                // NOTE: If it twists the wrong way, add a minus sign in front of targetRotX or targetRotZ!
                Quaternion targetRotation = Quaternion.Euler(targetRotX, 0f, -targetRotZ);

                // 5. THE PHYSICS FIX: Use MoveRotation so the walls physically push the marble instead of phasing through it.
                if (boardRb != null)
                {
                    boardRb.MoveRotation(targetRotation);
                }
                
                // Visual handle can just snap normally
                visualHandle.rotation = targetRotation; 
            }
            else
            {
                // Snap invisible handle back when you let go
                invisibleGrabbable.position = Vector3.Lerp(invisibleGrabbable.position, resetAnchor.position, Time.deltaTime * resetSpeed);
                invisibleGrabbable.rotation = Quaternion.Lerp(invisibleGrabbable.rotation, resetAnchor.rotation, Time.deltaTime * resetSpeed);
                
                // Smoothly level the board back out using Physics
                if (boardRb != null)
                {
                    Quaternion leveledOut = Quaternion.Lerp(boardRb.rotation, Quaternion.identity, Time.deltaTime * resetSpeed);
                    boardRb.MoveRotation(leveledOut);
                    visualHandle.rotation = leveledOut;
                }
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
    }
}