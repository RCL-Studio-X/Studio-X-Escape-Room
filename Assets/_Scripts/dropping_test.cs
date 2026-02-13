using UnityEngine;

public class dropping_test : MonoBehaviour
{
    public GameObject dropletPrefab;
    public Transform spawnPoint;
    private float spawnInterval = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("nooo");
        InvokeRepeating(nameof(drop), 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void drop()
    {
        Vector3 pos = new Vector3(-2, 1, -1);
        Instantiate(dropletPrefab, pos, Quaternion.identity);
    }
}
