using UnityEngine;

public class HighlighTarget : MonoBehaviour
{
    [HideInInspector] public int originalLayer;

   
    void Awake()
    {
        originalLayer = gameObject.layer
    }


}
