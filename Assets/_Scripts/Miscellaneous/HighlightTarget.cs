using UnityEngine;

public class HighlightTarget : MonoBehaviour
{
    [Header("Highlight Settings")]
    [Tooltip("The object's original layer before highlight.")]
    [HideInInspector] 
    public int originalLayer;

    [SerializeField, Header("Outline Layer")]
    [Tooltip("Name of the layer used for outline highlighting.")]
    private string outlineLayerName = "Outline";

    [SerializeField, Header("Visual Target")]
    [Tooltip("Child object containing the visible mesh to apply the highlight layer to.")]
    private GameObject visualRoot;

    private int _outlineLayer;

    private void Awake()
    {
        // Stores original layer of the visual child.
        if (visualRoot)
        {
            originalLayer = visualRoot.layer;
            _outlineLayer = LayerMask.NameToLayer(outlineLayerName);
        }
    }

    public void ChangeToOutlineLayer()
    {
        if (visualRoot)
            visualRoot.layer = _outlineLayer;
    }

    public void ChangeToOriginalLayer()
    {
        if (visualRoot)
            visualRoot.layer = originalLayer;
    }
}
