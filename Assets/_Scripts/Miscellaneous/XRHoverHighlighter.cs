
using UnityEngine; 

public class XRHoverHighlighter : MonoBehaviour
{
    [SerializeField] private string outlineLayerName = "Outline";

    private int outlineLayer;
    private int originalLayer;

    void Awake()
    {
        outlineLayer = LayerMask.NameToLayer(outlineLayerName);
        originalLayer = gameObject.layer;
    }

    public void OnHoverEnter(UnityEngine.XR.Interaction.Toolkit.HoverEnterEventArgs args)
    {
        gameObject.layer = outlineLayer;
        Debug.Log("Outline changed to: " + gameObject.layer + "!");
    }

    public void OnHoverExit(UnityEngine.XR.Interaction.Toolkit.HoverExitEventArgs args)
    {
        gameObject.layer = originalLayer;
    }
}
