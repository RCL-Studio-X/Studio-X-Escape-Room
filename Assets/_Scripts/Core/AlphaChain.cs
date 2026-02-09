using UnityEngine;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Manages a chain of boolean conditions and invokes an event once all conditions are set to true.
    /// </summary>
    public class AlphaChain : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Condition Flags")]

        [Tooltip("Boolean condition states. Each condition starts false and can be set true via SetConditionTrue.")]
        public bool[] conditions = new bool[3];

        [Header("Events")]

        [Tooltip("Invoked once all conditions have been set to true.")]
        public UnityEvent onAllConditionsTrue;

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Sets a condition at the specified index to true and checks if all conditions are satisfied.
        /// If all conditions are true, the completion event is invoked.
        /// </summary>
        /// <param name="index">Index of the condition to set to true.</param>
        public void SetConditionTrue(int index)
        {
            if ((uint)index >= (uint)conditions.Length)
            {
                return;
            }

            conditions[index] = true;

            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i])
                {
                    return;
                }
            }

            onAllConditionsTrue?.Invoke();
        }

        #endregion
    }
}
