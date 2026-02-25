using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MirrorRotation : MonoBehaviour
{
    public Transform mirroredObject;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (mirroredObject && rb)
        {
            rb.MoveRotation(mirroredObject.rotation);
        }
    }
}
