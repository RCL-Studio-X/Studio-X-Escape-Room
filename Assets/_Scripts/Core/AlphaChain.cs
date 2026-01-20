using UnityEngine;
using UnityEngine.Events;

public class AlphaChain : MonoBehaviour
{
    [Header("Condition Flags")]
    [Tooltip("Three booleans representing condition states. All start false and are set via SetConditionTrue.")]
    public bool[] conditions = new bool[3];

    [Header("Events")]
    [Tooltip("Invoked once all conditions have been set to true.")]
    public UnityEvent onAllConditionsTrue;

    public void SetConditionTrue(int index)
    {
        // Validate index
        if ((uint)index >= (uint)conditions.Length)
            return;

        conditions[index] = true;

        // Check if all conditions are true
        for (var i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i])
                return;
        }

        onAllConditionsTrue?.Invoke();
    }
}