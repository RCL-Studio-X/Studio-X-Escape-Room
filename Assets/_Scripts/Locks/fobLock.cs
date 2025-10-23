using UnityEngine;

public class fobLock : MonoBehaviour
{
    public GameObject fob;
    public bool locked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == fob.name) {
            locked = false;
        }
    }
}
