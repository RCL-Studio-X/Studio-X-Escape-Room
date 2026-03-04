// RockSocketGroup.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RockSocketGroup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PuzzleRock puzzleRock;
    [SerializeField] private RockSocket[] sockets;

    [Header("Behavior")]
    [SerializeField] private bool checkOnlyWhenAllSocketsFilled = true;
    [SerializeField] private float wrongAnswerRespawnDelay = 0.15f;

    [Header("Respawner")]
    [SerializeField] private RespawnManager respawner;

    private void Awake()
    {
        if (puzzleRock == null) puzzleRock = GetComponent<PuzzleRock>();
    }

    private void OnEnable()
    {
        foreach (var s in sockets)
            if (s != null) s.OnChanged += HandleSocketChanged;
    }

    private void OnDisable()
    {
        foreach (var s in sockets)
            if (s != null) s.OnChanged -= HandleSocketChanged;
    }

    private void HandleSocketChanged(RockSocket _)
    {
        if (checkOnlyWhenAllSocketsFilled && !AllFilled()) return;

        var placed = GetPlacedSymbols();
        bool ok = puzzleRock != null && puzzleRock.ValidateSolution(placed);

        if (!ok)
            Invoke(nameof(RejectAndRespawn), wrongAnswerRespawnDelay);
        else
            OnSolved();
    }

    private bool AllFilled()
    {
        foreach (var s in sockets)
            if (s == null || s.CurrentChunk == null) return false;
        return true;
    }

    private List<SymbolType> GetPlacedSymbols()
    {
        var list = new List<SymbolType>(sockets.Length);
        foreach (var s in sockets)
        {
            if (s != null && s.CurrentChunk != null) list.Add(s.CurrentChunk.Symbol);
        }
        return list;
    }

    private void RejectAndRespawn()
    {
        // Detach + respawn all currently placed chunks.

        Debug.Log("[Puzzle] Wrong answer!");

        respawner.RespawnEverything();
    }

    private void OnSolved()
    {
        Debug.Log("[Puzzle] Solved!");

        // Unlock next stage
    }
}