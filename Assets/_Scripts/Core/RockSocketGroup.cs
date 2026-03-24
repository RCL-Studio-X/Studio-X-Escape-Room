using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using StudioXRCL.EscapeRoom.Utilities;

namespace StudioXRCL.EscapeRoom.Core
{
    public class RockSocketGroup : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the PuzzleRock that defines the correct solution. If null, will attempt to find one on the same GameObject at runtime.")]
        [SerializeField] private PuzzleRock puzzleRock;
        [Tooltip("References to the RockSockets that make up this group. The order of sockets should correspond to the order of symbols expected by the PuzzleRock.")]
        [SerializeField] private RockSocket[] sockets;

        [Header("Behavior")]
        [Tooltip("If true, the puzzle will only check for a correct solution when all sockets are filled. If false, it will check on every change, allowing for partial solutions to be accepted.")]
        [SerializeField] private bool checkOnlyWhenAllSocketsFilled = true;
        [Tooltip("Delay in seconds before respawning chunks after a wrong answer is detected. This allows for any feedback (e.g., sound, visual) to be perceived by the player before the chunks are reset.")]
        [SerializeField] private float wrongAnswerRespawnDelay = 0.15f;

        [Header("Respawner")]
        [Tooltip("Reference to the RespawnManager responsible for resetting chunks. If null, it will be located via FindObjectOfType at runtime.")]
        [SerializeField] private RespawnManager respawner;

        /// <summary>
        /// Initializes the puzzleRock reference if not set.
        /// </summary>
        private void Awake()
        {
            if (puzzleRock == null) puzzleRock = GetComponent<PuzzleRock>();
        }

        /// <summary>
        /// Subscribes to socket change events.
        /// </summary>
        private void OnEnable()
        {
            foreach (var s in sockets)
                if (s != null) s.OnChanged += HandleSocketChanged;
        }

        /// <summary>
        /// Unsubscribes from socket change events.
        /// </summary>
        private void OnDisable()
        {
            foreach (var s in sockets)
                if (s != null) s.OnChanged -= HandleSocketChanged;
        }

        /// <summary>
        /// Handles changes in socket states and validates the puzzle solution.
        /// </summary>
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

        /// <summary>
        /// Checks if all sockets are filled with chunks.
        /// </summary>
        private bool AllFilled()
        {
            foreach (var s in sockets)
                if (s == null || s.CurrentChunk == null) return false;
            return true;
        }

        /// <summary>
        /// Retrieves the list of symbols from placed chunks.
        /// </summary>
        private List<SymbolType> GetPlacedSymbols()
        {
            var list = new List<SymbolType>(sockets.Length);
            foreach (var s in sockets)
            {
                if (s != null && s.CurrentChunk != null) list.Add(s.CurrentChunk.Symbol);
            }
            return list;
        }

        /// <summary>
        /// Rejects the current placement and respawns all chunks.
        /// </summary>
        private void RejectAndRespawn()
        {
            // Detach + respawn all currently placed chunks.

            Debug.Log("[Rock Puzzle] Wrong answer!");

            respawner.RespawnEverything();
        }

        /// <summary>
        /// Handles the solved state of the puzzle.
        /// </summary>
        private void OnSolved()
        {
            Debug.Log("[Rock Puzzle] Solved!");
            puzzleRock.onCorrectSolution?.Invoke();
            // TODO: Unlock next stage
        }
    }
}
