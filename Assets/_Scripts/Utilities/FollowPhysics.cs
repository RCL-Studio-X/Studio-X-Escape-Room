using UnityEngine;

/// <summary>
/// Attaches to the visible parent object (VisualChalk) and uses the Rigidbody's
/// velocity to predict the target's position in LateUpdate, which eliminates visual lag.
/// </summary>
public class FollowPhysics : MonoBehaviour
{
    [Tooltip("The Rigidbody component on the Invisible Physics Chalk.")]
    public Rigidbody physicsTargetRigidbody;

    // A small multiplier to tweak the prediction amount if needed (usually 1.0 is correct).
    [Tooltip("Tweak the prediction amount. 1.0 is usually correct.")]
    public float predictionFactor = 1.0f;

    private Transform targetTransform;

    void Start()
    {
        // Safety check and getting the Transform once for efficiency
        if (physicsTargetRigidbody == null)
        {
            Debug.LogError("FollowPhysics: Rigidbody is not assigned! Drag the Invisible Physics Chalk's Rigidbody into the slot.");
            return;
        }
        targetTransform = physicsTargetRigidbody.transform;
    }

    [System.Obsolete]
    void LateUpdate()
    {
        if (physicsTargetRigidbody == null || targetTransform == null)
        {
            return; // Skip if setup failed
        }

        // 1. Calculate the time that has passed since the last physics update (FixedUpdate) and the current moment (LateUpdate).
        float timeSincePhysics = Time.time - Time.fixedTime;

        // 2. Get the current velocity of the physics object.
        Vector3 velocity = physicsTargetRigidbody.velocity;

        // 3. Calculate the predicted position: Current position + (Velocity * Time Since Update * Prediction Factor)
        Vector3 predictedPosition = targetTransform.position + (velocity * timeSincePhysics * predictionFactor);

        // 4. Apply the predicted position to the visual object.
        this.transform.position = predictedPosition;

        // 5. Rotation can usually just follow directly, as angular velocity changes are less noticeable.
        this.transform.rotation = targetTransform.rotation;
    }
}