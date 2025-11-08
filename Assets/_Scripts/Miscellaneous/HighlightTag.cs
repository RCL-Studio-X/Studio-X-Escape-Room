using UnityEngine;

public class HighlightTag : MonoBehaviour
{
    [HideInInspector] public int originalLayer;

    void Awake()
    {
        originalLayer = gameObject.layer;
    }

}
