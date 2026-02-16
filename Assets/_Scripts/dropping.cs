using UnityEngine;

public class dropping : MonoBehaviour
{
    public GameObject dropletPrefab;
    private float spawnInterval = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(drop), 0f, spawnInterval);
    }

    void drop()
    {
        Vector3 pos = new Vector3(-2, 1, -1);
        Instantiate(dropletPrefab, pos, Quaternion.identity);
    }
}
