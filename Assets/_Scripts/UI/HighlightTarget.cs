using UnityEngine;

namespace StudioX.EscapeRoom.Utilities
{
    /// <summary>
    /// Handles switching a target object's visual layer between its original layer
    /// and an outline layer for highlight effects.
    /// </summary>
    public class HighlightTarget : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Highlight Settings")]
        [Tooltip("The object's original layer before highlight.")]
        [HideInInspector]
        public int originalLayer;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Name of the layer used for outline highlighting.
        /// </summary>
        [SerializeField]
        [Header("Outline Layer")]
        [Tooltip("Name of the layer used for outline highlighting.")]
        private string outlineLayerName = "Outline";

        /// <summary>
        /// Child object containing the visible mesh to apply the highlight layer to.
        /// </summary>
        [SerializeField]
        [Header("Visual Target")]
        [Tooltip("Child object containing the visible mesh to apply the highlight layer to.")]
        private GameObject visualRoot;

        /// <summary>
        /// Cached layer index for the outline layer.
        /// </summary>
        private int _outlineLayer;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches the original layer and resolves the outline layer index.
        /// </summary>
        private void Awake()
        {
            if (visualRoot)
            {
                originalLayer = visualRoot.layer;
                _outlineLayer = LayerMask.NameToLayer(outlineLayerName);
            }
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Changes the visual target's layer to the outline layer.
        /// </summary>
        public void ChangeToOutlineLayer()
        {
            if (visualRoot)
            {
                visualRoot.layer = _outlineLayer;
            }
        }

        /// <summary>
        /// Restores the visual target's original layer.
        /// </summary>
        public void ChangeToOriginalLayer()
        {
            if (visualRoot)
            {
                visualRoot.layer = originalLayer;
            }
        }

        #endregion
    }
}
