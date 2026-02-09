using UnityEngine;
using System.Collections.Generic;

public class MiningPuzzleZone : MonoBehaviour
{
    [Header("Puzzle State")]
    public bool isSolved = false;

    // List to track the actual scripts on the objects inside the zone
    private List<MineralData> mineralsInZone = new List<MineralData>();

    // The exact combination needed to win
    private List<MineralType> requiredSolution = new List<MineralType>
    {
        MineralType.Gold,
        MineralType.Amethyst,
        MineralType.Emerald,
        MineralType.Sapphire
    };

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering has the MineralData component
        MineralData mineral = other.GetComponent<MineralData>();

        if (mineral != null)
        {
            // Add it to our list and re-check the puzzle
            mineralsInZone.Add(mineral);
            CheckPuzzle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MineralData mineral = other.GetComponent<MineralData>();

        if (mineral != null)
        {
            // Remove it from our list and re-check
            mineralsInZone.Remove(mineral);
            CheckPuzzle();
        }
    }

    private void CheckPuzzle()
    {
        // 1. If the count isn't exactly 4, it can't be right.
        if (mineralsInZone.Count != 4)
        {
            SetSolvedState(false);
            return;
        }

        // 2. Create a temporary list of the types currently in the zone
        List<MineralType> currentTypes = new List<MineralType>();
        foreach (var m in mineralsInZone)
        {
            currentTypes.Add(m.type);
        }

        // 3. Check if every required mineral is present in the current types
        bool allMatch = true;
        foreach (MineralType required in requiredSolution)
        {
            if (!currentTypes.Contains(required))
            {
                allMatch = false;
                break;
            }
        }

        SetSolvedState(allMatch);
    }

    private void SetSolvedState(bool state)
    {
        isSolved = state;

        if (isSolved)
        {
            Debug.Log("Puzzle Solved!");
            // Optional: Add visual feedback here 
        }
    }
}