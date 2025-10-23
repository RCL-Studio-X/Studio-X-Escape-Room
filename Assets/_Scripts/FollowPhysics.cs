using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    [Tooltip("The Transform that handles all the physics (the Invisible Physics Chalk).")]
    public Transform physicsTarget;

    // Use LateUpdate to ensure this runs after all Update and FixedUpdate (physics) calls.
    void LateUpdate()
    {
        // Safety check to ensure the target is assigned in the Inspector.
        if (physicsTarget == null)
        {
            Debug.LogError("FollowPhysics: Physics Target is not assigned. Drag the invisible physics object into this slot.");
            return;
        }

        // 1. Position: Copy the position of the physics target to the visual object.
        this.transform.position = physicsTarget.position;

        // 2. Rotation: Copy the rotation of the physics target to the visual object.
        this.transform.rotation = physicsTarget.rotation;
    }
}
