using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;

    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
    }

    public void Respawn()
    {
        transform.SetPositionAndRotation(_originalPosition, _originalRotation);
        transform.localScale = _originalScale;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Respawned!");
    }
}

