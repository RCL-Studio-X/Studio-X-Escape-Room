using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Stores data specific to an individual card, such as the team it belongs to.
    /// </summary>
    public class CardData : MonoBehaviour
    {
        #region Public Variable declarations

        [Tooltip("Which team does this card belong to? (0 = Team 1, 1 = Team 2, etc.)")]
        public int targetTeamIndex;

        #endregion
    }
}