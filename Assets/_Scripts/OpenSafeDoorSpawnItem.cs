using UnityEngine;

public class OpenSafeDoorSpawnItem : MonoBehaviour
{
    public float speed = 5f; 
    private Quaternion target;

    [Header("Spawn Settings")]
    public Transform itemSpawn;        // Where the item appears
    public GameObject prefabToSpawn;   // What to spawn
    private bool hasSpawned = false;   // Prevent double-spawn

    public bool opened;

    void Start()
    {
        target = Quaternion.Euler(-90, 0, -240);
        opened = false;
    }

    public void Open()
    {
        opened = true;

        if (!hasSpawned && prefabToSpawn != null && itemSpawn != null)
        {
            Instantiate(prefabToSpawn, itemSpawn.position, itemSpawn.rotation);
            hasSpawned = true;
        }
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
