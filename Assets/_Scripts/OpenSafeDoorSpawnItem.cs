using UnityEngine;

public class OpenSafeDoorSpawnItem : MonoBehaviour
{
    public float speed = 5f; // Interpolation speed
    private Quaternion target;
    public Transform itemSpawn;
    public bool opened;

    void Start()
    {
        // Assign to the class field, not a new local variable
        target = Quaternion.Euler(-90, 0, -240);
        opened = false;
    }

    public void Open()
    {
        opened = true;
    }

    void Update()
    {
        if (opened)
        {
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            Time.deltaTime * speed
            );
        }
    }
}