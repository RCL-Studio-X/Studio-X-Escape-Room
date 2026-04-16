using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Defines the types of minerals available for the puzzle.
    /// </summary>
    public enum MineralType
    {
        Obsidian,
        FoolsGold,
        Coal,
        Iron,
        Amethyst,
        Emerald,
        Sapphire,
        Diamond
    }

    /// <summary>
    /// Component to identify a game object as a specific mineral type.
    /// </summary>
    public class MineralData : MonoBehaviour
    {
        #region Public Variables

        [Header("Mineral Settings")]
        [Tooltip("The specific type of mineral this object represents.")]
        public MineralType type;

        #endregion
    }
}