using UnityEngine;

namespace StudioXRCL.EscapeRoom.VFX
{
    public class ChalkboardReset : MonoBehaviour
    {
        [Tooltip("Drag the empty child GameObject that holds the ink here!")]
        public Transform trailContainer; 

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
    }
}