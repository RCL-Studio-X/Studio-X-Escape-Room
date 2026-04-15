using UnityEngine;
using System.Collections.Generic;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Manages the mining puzzle area, detecting which minerals are placed inside and validating the solution.
    /// </summary>
    public class MiningPuzzleZone : MonoBehaviour
    {
        #region Public Variables

        [Header("Puzzle State")]
        [Tooltip("Indicates whether the puzzle has been successfully solved.")]
        public bool isSolved = false;

        [Header("Unity Events")]
        [Tooltip("Event triggered when the puzzle is solved.")]
        public UnityEngine.Events.UnityEvent onPuzzleSolved;

        #endregion

        #region Private Variables

        /// <summary>
        /// List to track the actual scripts on the objects inside the zone.
        /// </summary>
        private List<MineralData> _mineralsInZone = new List<MineralData>();

        /// <summary>
        /// The exact combination of minerals needed to win.
        /// </summary>
        private List<MineralType> _requiredSolution = new List<MineralType>
        {
            MineralType.Gold,
            MineralType.Amethyst,
            MineralType.Emerald,
            MineralType.Sapphire
        };

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when an object enters the trigger collider. Adds valid minerals to the tracking list.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering has the MineralData component
            MineralData mineral = other.GetComponent<MineralData>();

            if (mineral != null)
            {
                // Add it to our list and re-check the puzzle
                _mineralsInZone.Add(mineral);
                CheckPuzzle();
            }
        }

        /// <summary>
        /// Called when an object exits the trigger collider. Removes the mineral from the tracking list.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            MineralData mineral = other.GetComponent<MineralData>();

            if (mineral != null)
            {
                // Remove it from our list and re-check
                _mineralsInZone.Remove(mineral);
                CheckPuzzle();
            }
        }

        /// <summary>
        /// Checks if the current list of minerals exactly matches the required solution.
        /// </summary>
        private void CheckPuzzle()
        {
            // 1. If the count isn't exactly 4, it can't be right.
            if (_mineralsInZone.Count != 4)
            {
                SetSolvedState(false);
                return;
            }

            // 2. Create a temporary list of the types currently in the zone
            List<MineralType> currentTypes = new List<MineralType>();
            foreach (var m in _mineralsInZone)
            {
                currentTypes.Add(m.type);
            }

            // 3. Check if every required mineral is present in the current types
            bool allMatch = true;
            foreach (MineralType required in _requiredSolution)
            {
                if (!currentTypes.Contains(required))
                {
                    allMatch = false;
                    break;
                }
            }

            SetSolvedState(allMatch);
        }

        /// <summary>
        /// Updates the puzzle's solved state.
        /// </summary>
        /// <param name="state">True if the puzzle is solved, otherwise false.</param>
        private void SetSolvedState(bool state)
        {
            isSolved = state;

            if (isSolved)
            {
                onPuzzleSolved.Invoke();
            }
        }

        #endregion
    }
}