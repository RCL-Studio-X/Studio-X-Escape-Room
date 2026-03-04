// PuzzleRock.cs
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRock : MonoBehaviour
{
    [System.Serializable]
    public struct SymbolCount
    {
        public SymbolType symbol;
        public int count;
    }

    [Header("Expected solution (order-independent, repeats allowed)")]
    [SerializeField] private SymbolCount[] expected;

    private Dictionary<SymbolType, int> expectedMap;

    private void Awake()
    {
        expectedMap = BuildMap(expected);
    }

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
        if (placedMap.Count != expectedMap.Count) return false;

        foreach (var kv in expectedMap)
        {
            if (!placedMap.TryGetValue(kv.Key, out int c)) return false;
            if (c != kv.Value) return false;
        }
        return true;
    }

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