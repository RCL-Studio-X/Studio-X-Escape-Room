using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    [Header("Whiteboard Settings")]
    [Tooltip("Texture applied to the whiteboard surface.")]
    public Texture2D texture;

    public Color fillColor;
    
    [Tooltip("Dimensions of the generated whiteboard texture.")]
    public Vector2 textureSize = new Vector2(2048, 2048);

    Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        if (texture) 
        {
            _renderer.material.mainTexture = texture;
            return;
        }

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        
        // Create an array with that color for every pixel
        Color[] fillPixels = new Color[texture.width * texture.height];
        
        for (int i = 0; i < fillPixels.Length; i++)
            fillPixels[i] = fillColor;
        
        // Set the pixels and apply the change
        texture.SetPixels(fillPixels);
        texture.Apply();

        _renderer.material.mainTexture = texture;
    }
}