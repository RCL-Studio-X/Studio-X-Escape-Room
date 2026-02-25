using UnityEngine;
using UnityEngine.Events;

public class ButtonPress : MonoBehaviour
{
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    public float pressDepth = 0.02f; // Adjusted to 2cm for realistic VR scale
    bool isPressed = false;
    Vector3 startLocalPosition; // Using localPosition is safer for angled panels

    void Start()
    {
        // Save the exact starting coordinate
        startLocalPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            // 1. Visually move the button down
            transform.localPosition = new Vector3(startLocalPosition.x, startLocalPosition.y - pressDepth, startLocalPosition.z);
            
            // 2. Track the hand/finger and fire the event
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Only release if the object that pushed it is the one leaving
        if (other.gameObject == presser)
        {
            // 1. Visually return the button to the exact original height
            transform.localPosition = startLocalPosition;
            
            // 2. Fire the release event
            onRelease.Invoke();
            isPressed = false;
        }
    }
}