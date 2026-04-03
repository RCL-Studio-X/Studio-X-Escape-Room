using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    /// <summary>
    /// Handles the logic for clearing drawn chalk trails from a specific chalkboard.
    /// </summary>
    public class ChalkboardReset : MonoBehaviour
    {
        #region Public Variable declarations

        [Header("References")]
        [Tooltip("Drag the empty child GameObject that holds the ink here!")]
        public Transform trailContainer; 

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Destroys all child trail objects within the trail container to clear the board.
        /// </summary>
        public void ResetChalkboard()   
        {
            Debug.Log("Resetting chalkboard");
            
            if (trailContainer == null) return; // Safety check

            // Loop backward to safely destroy every child piece of ink
            for (int i = trailContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(trailContainer.GetChild(i).gameObject);
            }
        }

        #endregion
    }
}