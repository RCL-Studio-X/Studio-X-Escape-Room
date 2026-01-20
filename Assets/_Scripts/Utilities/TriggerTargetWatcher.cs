using UnityEngine;
using UnityEngine.Events;

public class TriggerTargetWatcher : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("The specific GameObject that must enter this trigger to invoke the event.")]
    public GameObject targetGameObject;

    [Header("Events")]
    [Tooltip("Invoked when the target GameObject enters this trigger.")]
    public UnityEvent onTriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered the exit collider.");
        // Ensure the target is assigned
        if (!targetGameObject)
            return;

        // Invoke the event only when the correct object enters the trigger
        if (other.gameObject == targetGameObject)
            onTriggerEntered?.Invoke();
    }
}