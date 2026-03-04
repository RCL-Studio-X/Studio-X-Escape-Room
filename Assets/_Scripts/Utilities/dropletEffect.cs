using UnityEngine;

public class dropletEffect : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spawnPos = new Vector3(-1, 2, -1);
        
        Instantiate(prefab, spawnPos, prefab.transform.rotation);
    }
}
