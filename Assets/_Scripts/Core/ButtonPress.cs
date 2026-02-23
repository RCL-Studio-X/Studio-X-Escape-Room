using UnityEngine;
using UnityEngine.Events;
public class ButtonPress : MonoBehaviour
{
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    public float pressDepth=.2f;
    bool isPressed=false;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            startPosition.y = -pressDepth;
            presser=other.gameObject;
            onPress.Invoke();
            isPressed=true;    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject==presser)
        {
            transform.position = startPosition;
            onRelease.Invoke();
            isPressed=false;
        }
        
    }
}