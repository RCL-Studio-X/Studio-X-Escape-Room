using System.Collections.Generic;
using UnityEngine;
using StudioXRCL.EscapeRoom.Utilities;
using UnityEngine.Events;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Validates a set of placed symbols against an expected solution.
    /// Used by puzzle mechanics that require matching a set of symbols (order-independent).
    /// </summary>
    public class PuzzleRock : MonoBehaviour
    {
        /// <summary>
        /// Container for a symbol and the expected count of that symbol.
        /// </summary>
        [System.Serializable]
        public struct SymbolCount
        {
            /// <summary>
            /// The symbol to match.
            /// </summary>
            public SymbolType symbol;

            /// <summary>
            /// How many of the symbol are required in the solution.
            /// </summary>
            public int count;
        }

        /// <summary>
        /// Defines the expected symbols and their counts for the correct solution.
        /// Order does not matter, but counts must match exactly.
        /// </summary>
        [Header("Expected solution (order-independent, repeats allowed)")]
        [Tooltip("Defines the expected symbols and their counts for the correct solution. Order does not matter, but counts must match exactly.")]
        [SerializeField] private SymbolCount[] expected;

        /// <summary>
        /// Cached map of expected symbol counts for fast validation.
        /// </summary>
        private Dictionary<SymbolType, int> _expectedMap;

        public UnityEvent onCorrectSolution;

        /// <summary>
        /// Builds the expected symbol map when the object is initialized.
        /// </summary>
        private void Awake()
        {
            _expectedMap = BuildMap(expected);
        }

        /// <summary>
        /// Validates that the provided symbols match the expected solution exactly.
        /// </summary>
        /// <param name="placedSymbols">Symbols currently placed by the player.</param>
        /// <returns>True if the placed symbols match the expected symbols (same keys/counts).</returns>
        public bool ValidateSolution(List<SymbolType> placedSymbols)
        {
            var placedMap = new Dictionary<SymbolType, int>();
            foreach (var s in placedSymbols)
            {
                if (s == SymbolType.None) continue;
                placedMap.TryGetValue(s, out int c);
                placedMap[s] = c + 1;
            }

            // Compare maps exactly (same keys + same counts).
            if (placedMap.Count != _expectedMap.Count) return false;

            foreach (var kv in _expectedMap)
            {
                if (!placedMap.TryGetValue(kv.Key, out int c)) return false;
                if (c != kv.Value) return false;
            }
            return true;
        }

        /// <summary>
        /// Converts an array of SymbolCount into a map of symbol -> count.
        /// </summary>
        /// <param name="counts">Array of SymbolCount entries to convert.</param>
        /// <returns>Map of symbol counts.</returns>
        private static Dictionary<SymbolType, int> BuildMap(SymbolCount[] counts)
        {
            var map = new Dictionary<SymbolType, int>();
            if (counts == null) return map;

            foreach (var sc in counts)
            {
                if (sc.symbol == SymbolType.None) continue;
                map.TryGetValue(sc.symbol, out int existing);
                map[sc.symbol] = existing + Mathf.Max(0, sc.count);
            }
            return map;
        }
    }
}
