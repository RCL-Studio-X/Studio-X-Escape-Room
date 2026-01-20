using System.Linq;
using UnityEngine;

namespace StudioXRCL.EscapeRoom.UI
{
    /// <summary>
    /// Allows a marker object to draw onto a Whiteboard by raycasting
    /// from its tip and modifying the whiteboard texture at the point of contact.
    /// </summary>
    public class WhiteboardMarker : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Marker Settings")]
        [Tooltip("The tip of the marker that makes contact with the whiteboard surface.")]
        [SerializeField] private Transform tip;

        [Tooltip("The diameter of the marker tip in pixels when drawing on the whiteboard.")]
        [SerializeField] private int penSize = 5;

        #endregion

        #region Private Variable Declarations

        /// <summary> Renderer used to determine the marker's drawing color. </summary>
        private Renderer _renderer;

        /// <summary> Cached color array used when drawing pixels. </summary>
        private Color[] _colors;

        /// <summary> Height of the marker tip used to determine raycast distance. </summary>
        private float _tipHeight;

        /// <summary> Raycast hit data for detecting whiteboard contact. </summary>
        private RaycastHit _touch;

        /// <summary> Reference to the currently contacted whiteboard. </summary>
        private Whiteboard _whiteboard;

        /// <summary> Current texture coordinate of the marker tip. </summary>
        private Vector2 _touchPos;

        /// <summary> Previous frame's texture coordinate. </summary>
        private Vector2 _lastTouchPos;

        /// <summary> Whether the marker was touching the whiteboard in the previous frame. </summary>
        private bool _touchedLastFrame;

        /// <summary> Rigidbody used to control marker physics constraints. </summary>
        private Rigidbody _markerRigidbody;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Initializes cached references and prepares drawing color data.
        /// </summary>
        private void Start()
        {
            _renderer = tip.GetComponent<Renderer>();

            Color baseColor = _renderer.material.color;
            int colorCount = penSize * penSize;
            _colors = Enumerable.Repeat(baseColor, colorCount).ToArray();

            _tipHeight = tip.localScale.y;
            _markerRigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Updates the drawing logic each frame.
        /// </summary>
        private void Update()
        {
            Draw();
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Performs raycasting from the marker tip and draws onto the
        /// whiteboard texture when contact is detected.
        /// </summary>
        private void Draw()
        {
            Vector3 tipPosition = tip.position;
            Vector3 tipDirection = transform.up;
            float rayDistance = _tipHeight * 2f;

            if (!Physics.Raycast(tipPosition - tipDirection * _tipHeight, tipDirection, out _touch, rayDistance))
            {
                ResetTouchState();
                _markerRigidbody.constraints = RigidbodyConstraints.None;
                return;
            }

            if (!_touch.transform.TryGetComponent(out Whiteboard board))
            {
                ResetTouchState();
                return;
            }

            _whiteboard ??= board;
            _markerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _touchPos.Set(_touch.textureCoord.x, _touch.textureCoord.y);

            Vector2 textureSize = _whiteboard.textureSize;
            int x = (int)(_touchPos.x * textureSize.x - penSize * 0.5f);
            int y = (int)(_touchPos.y * textureSize.y - penSize * 0.5f);

            if (x < 0 || y < 0 || x > textureSize.x || y > textureSize.y)
            {
                _touchedLastFrame = false;
                return;
            }

            if (_touchedLastFrame)
            {
                Texture2D texture = _whiteboard.texture;
                texture.SetPixels(x, y, penSize, penSize, _colors);

                for (float f = 0.01f; f < 1f; f += 0.04f)
                {
                    int lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                    int lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                    texture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);
                }

                texture.Apply();
            }

            _lastTouchPos.Set(x, y);
            _touchedLastFrame = true;
        }

        /// <summary>
        /// Resets cached touch state when the marker is no longer
        /// in contact with a whiteboard.
        /// </summary>
        private void ResetTouchState()
        {
            _whiteboard = null;
            _touchedLastFrame = false;
        }

        #endregion
    }
}
