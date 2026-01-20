using UnityEngine;

namespace StudioXRCL.EscapeRoom.UI
{
    /// <summary>
    /// Manages a drawable whiteboard surface by either applying
    /// a provided texture or generating a transparent texture
    /// filled with a specified color.
    /// </summary>
    public class Whiteboard : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Whiteboard Settings")]
        [Tooltip("Texture applied to the whiteboard surface. If set, color fill is ignored.")]
        public Texture2D texture;

        [Tooltip("Fill color used when generating a blank whiteboard texture.")]
        public Color fillColor;

        [Tooltip("Dimensions of the generated whiteboard texture.")]
        public Vector2 textureSize = new Vector2(2048, 2048);

        #endregion

        #region Private Variable Declarations

        /// <summary> Whether the whiteboard should generate a color-filled texture. </summary>
        private bool _useColor = true;

        /// <summary> Cached renderer for applying materials and textures. </summary>
        private Renderer _renderer;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches the renderer and initializes fill color transparency.
        /// </summary>
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            fillColor.a = 0.0f;
        }

        /// <summary>
        /// Determines whether to use a provided texture or generate one,
        /// then initializes the whiteboard surface.
        /// </summary>
        private void Start()
        {
            if (texture != null)
                _useColor = false;

            ResetDrawing();
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Resets the whiteboard surface by either applying the provided
        /// texture or generating a new transparent texture filled with color.
        /// </summary>
        public void ResetDrawing()
        {
            if (!_useColor)
            {
                ApplyTexture(texture);
                return;
            }

            texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
            Color[] fillPixels = new Color[texture.width * texture.height];

            for (int i = 0; i < fillPixels.Length; i++)
                fillPixels[i] = fillColor;

            texture.SetPixels(fillPixels);
            texture.Apply();

            ApplyTexture(texture);
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Applies a texture to the renderer using an unlit transparent shader.
        /// </summary>
        /// <param name="appliedTexture">Texture to apply to the whiteboard.</param>
        private void ApplyTexture(Texture appliedTexture)
        {
            if (_renderer == null)
                return;

            _renderer.material.shader = Shader.Find("Unlit/Transparent");
            _renderer.material.mainTexture = appliedTexture;
        }

        #endregion
    }
}
