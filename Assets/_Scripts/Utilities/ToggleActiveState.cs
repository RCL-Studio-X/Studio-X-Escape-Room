using System.Collections.Generic;
using UnityEngine;

namespace StudioXRCL.EscapeRoom.Utilities
{
    /// <summary>
    /// Toggles the active state of a collection of target GameObjects.
    /// </summary>
    public class ToggleActiveState : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Target Settings")]
        [Tooltip("GameObjects whose active state will be toggled.")]
        public List<GameObject> targets = new List<GameObject>();

        [Header("State")]
        [Tooltip("If true, all target GameObjects will be active.")]
        public bool isOn = false;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Applies the initial active state when the scene starts.
        /// </summary>
        private void Start()
        {
            ApplyState();
        }

        /// <summary>
        /// Ensures the active state updates immediately when values change in the Inspector.
        /// </summary>
        private void OnValidate()
        {
            ApplyState();
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Sets the active state to on and applies it to all targets.
        /// </summary>
        public void TurnOn()
        {
            isOn = true;
            ApplyState();
        }

        /// <summary>
        /// Sets the active state to off and applies it to all targets.
        /// </summary>
        public void TurnOff()
        {
            isOn = false;
            ApplyState();
        }

        /// <summary>
        /// Toggles the active state and applies it to all targets.
        /// </summary>
        public void Flip()
        {
            isOn = !isOn;
            ApplyState();
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Applies the current active state to all target GameObjects.
        /// </summary>
        private void ApplyState()
        {
            foreach (GameObject target in targets)
            {
                if (target == null)
                    continue;

                target.SetActive(isOn);
            }
        }

        #endregion
    }
}
