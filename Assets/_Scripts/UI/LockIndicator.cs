using UnityEngine;
using UnityEngine.UI;

namespace StudioXRCL.EscapeRoom.UI
{
    /// <summary>
    /// Controls the visual state of a lock indicator by switching
    /// its sprite based on a provided color identifier.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class LockIndicator : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Indicator Sprites")]
        [Tooltip("Sprite used for the default (white) indicator state.")]
        public Sprite whiteIndicatorSprite;

        [Tooltip("Sprite used for the blue indicator state.")]
        public Sprite blueIndicatorSprite;

        [Tooltip("Sprite used for the green indicator state.")]
        public Sprite greenIndicatorSprite;

        [Tooltip("Sprite used for the red indicator state.")]
        public Sprite redIndicatorSprite;

        #endregion

        #region Private Variable Declarations

        /// <summary> Image component used to display the indicator sprite. </summary>
        private Image _image;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches the required Image component.
        /// </summary>
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Changes the indicator sprite based on the provided color string.
        /// </summary>
        /// <param name="color">Color identifier (e.g., "blue", "green", "red").</param>
        public void ChangeIndicatorImage(string color)
        {
            if (_image == null)
                return;

            switch (color)
            {
                case "blue":
                    _image.sprite = blueIndicatorSprite;
                    break;

                case "green":
                    _image.sprite = greenIndicatorSprite;
                    break;

                case "red":
                    _image.sprite = redIndicatorSprite;
                    break;

                default:
                    _image.sprite = whiteIndicatorSprite;
                    break;
            }
        }

        #endregion
    }
}
