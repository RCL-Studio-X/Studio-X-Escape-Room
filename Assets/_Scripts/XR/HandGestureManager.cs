using UnityEngine;
using UnityEngine.InputSystem;

public class HandControllerManager : MonoBehaviour
{
    public InputActionReference triggerActionReference;
    public InputActionReference gripActionReference;

    public Animator handAnimator;

    void Start()
    {
        handAnimator = GetComponent<Animator>();
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        if(triggerActionReference != null && gripActionReference != null)
        {
            triggerActionReference.action.performed += ctx => UpdateHandAnimation("Trigger", ctx.ReadValue<float>());
            triggerActionReference.action.canceled += ctx => UpdateHandAnimation("Trigger", 0f);

            gripActionReference.action.performed += ctx => UpdateHandAnimation("Grip", ctx.ReadValue<float>());
            gripActionReference.action.canceled += ctx => UpdateHandAnimation("Grip", 0f);
        }
        else
        {
            Debug.LogWarning("Input Action References are not set in the Inspector.");
        }
    }

    private void UpdateHandAnimation(string parameterName, float value)
    {
        if(handAnimator != null)
        {
            handAnimator.SetFloat(parameterName, value);
        }
    }

    private void OnEnable()
    {
        triggerActionReference?.action.Enable();
        gripActionReference?.action.Enable();
    }

    private void OnDisable()
    {
        triggerActionReference?.action.Disable();
        gripActionReference?.action.Disable();
    }
}
