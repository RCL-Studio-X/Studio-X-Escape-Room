using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Controls the physical rotation and tilt of a maze board using an XR grab handle.
    /// </summary>
    public class MazeController : MonoBehaviour
    {
        #region Public Variable declarations

        [Header("References")]
        [Tooltip("The actual maze board that the marble rolls on.")]
        public Transform mazeBoard; 
        
        [Tooltip("The Kinematic Rigidbody attached to your MazeBoard!")]
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

        #endregion

        #region Private Variable declarations

        /// <summary> Tracks whether the handle is currently being held by the player. </summary>
        private bool _isGrabbed = false;

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Flags the board as being actively controlled when the handle is grabbed.
        /// </summary>
        public void OnHandleGrabbed()
        {
            _isGrabbed = true;
        }

        /// <summary>
        /// Flags the board to reset its rotation when the handle is released.
        /// </summary>
        public void OnHandleReleased()
        {
            _isGrabbed = false;
        }

        #endregion

        #region Private Method definitions

        /// <summary>
        /// Handles the physics-based rotation of the board and handle resetting.
        /// FixedUpdate must be used when moving Rigidbodies.
        /// </summary>
        private void FixedUpdate() 
        {
            if (_isGrabbed)
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

                // 5. Use MoveRotation so the walls physically push the marble instead of phasing through it.
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

        #endregion
    }
}