using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 10f;
    [SerializeField] private string outlineLayerName = "Outline";

    private int outlineLayer;
    private GameObject lastHighlightedObject;

    void Start()
    {
        outlineLayer = LayerMask.NameToLayer(outlineLayerName);
    }

    void Update()
    {
        HighlightRayCastCheck();
    }

    void HighlightRayCastCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * .5f, Screen.height * .5f));

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.TryGetComponent(out HighlightTag target))
            {
                GameObject targetObject = target.gameObject;
                if (lastHighlightedObject != targetObject)
                {
                    targetObject.layer = outlineLayer;
                    lastHighlightedObject = targetObject;
                }
                return;
            }
        }
        ClearHighlight();
    }
    
    void ClearHighlight()
    {
        if(lastHighlightedObject != null)
        {
            if(lastHighlightedObject.TryGetComponent(out HighlightTag target))
            {
                lastHighlightedObject.layer = target.originalLayer;
            }
            lastHighlightedObject = null;
        }
    }
}
