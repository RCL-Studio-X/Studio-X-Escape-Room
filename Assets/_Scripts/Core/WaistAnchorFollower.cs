using UnityEngine;

public class WaistAnchorFollower : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float verticalOffset = -0.65f;
    [SerializeField] private float backwardOffset = -0.10f;

    void LateUpdate()
    {
        if (head == null) return;

        Vector3 flatForward = head.forward;
        flatForward.y = 0f;

        if (flatForward.sqrMagnitude < 0.001f)
            flatForward = Vector3.forward;

        flatForward.Normalize();

        transform.position =
            head.position +
            Vector3.up * verticalOffset +
            flatForward * backwardOffset;

        transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
    }
}