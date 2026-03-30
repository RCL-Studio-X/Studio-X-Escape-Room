using UnityEngine;

public class ShoulderAnchorFollower : MonoBehaviour
{
    [SerializeField] private Transform head;

    [SerializeField] private float rightOffset = 0.22f;
    [SerializeField] private float downOffset = 0.25f;
    [SerializeField] private float backOffset = 0.08f;

    private void LateUpdate()
    {
        if (head == null) return;

        Vector3 flatForward = head.forward;
        flatForward.y = 0f;

        if (flatForward.sqrMagnitude < 0.001f)
            flatForward = Vector3.forward;

        flatForward.Normalize();

        Vector3 flatRight = Vector3.Cross(Vector3.up, flatForward).normalized;

        transform.position =
            head.position
            + flatRight * rightOffset
            - Vector3.up * downOffset
            - flatForward * backOffset;

        transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
    }
}