using UnityEngine;
using UnityEngine.UI;

namespace StudioX.EscapeRoom.UI
{
    /// <summary>
    /// Forces left and right page tab UI elements to fixed local X positions
    /// while preserving their initial local Y alignment.
    /// </summary>
    public class PageUI : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Page Tabs")]
        [Tooltip("Image representing the left page tab.")]
        public Image leftTab;

        [Tooltip("Image representing the right page tab.")]
        public Image rightTab;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Fixed local X position for the left tab.
        /// </summary>
        private const float LEFT_TAB_POS_X = -254f;

        /// <summary>
        /// Fixed local X position for the right tab.
        /// </summary>
        private const float RIGHT_TAB_POS_X = 258f;

        /// <summary>
        /// Initial local Y position of the left tab.
        /// </summary>
        private float _initialLeftLocalY;

        /// <summary>
        /// Initial local Y position of the right tab.
        /// </summary>
        private float _initialRightLocalY;

        /// <summary>
        /// Cached target local position for the left tab.
        /// </summary>
        private Vector3 _leftTabPosition;

        /// <summary>
        /// Cached target local position for the right tab.
        /// </summary>
        private Vector3 _rightTabPosition;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches the initial local Y positions of both tabs and
        /// pre-calculates their target local positions.
        /// </summary>
        private void Start()
        {
            if (leftTab != null)
            {
                _initialLeftLocalY = leftTab.transform.localPosition.y;
            }

            if (rightTab != null)
            {
                _initialRightLocalY = rightTab.transform.localPosition.y;
            }

            _leftTabPosition = new Vector3(LEFT_TAB_POS_X, _initialLeftLocalY, 0f);
            _rightTabPosition = new Vector3(RIGHT_TAB_POS_X, _initialRightLocalY, 0f);
        }

        /// <summary>
        /// Forces the tabs to remain at their target local positions every frame.
        /// </summary>
        private void Update()
        {
            if (leftTab != null)
            {
                leftTab.transform.localPosition = _leftTabPosition;
            }

            if (rightTab != null)
            {
                rightTab.transform.localPosition = _rightTabPosition;
            }
        }

        #endregion
    }
}
