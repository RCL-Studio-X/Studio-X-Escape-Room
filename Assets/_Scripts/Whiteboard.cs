using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    [Header("Whiteboard Settings")]
    [Tooltip("Texture applied to the whiteboard surface.")]
    public Texture2D texture;

    public Color fillColor;
    private bool _useColor = true;
    
    [Tooltip("Dimensions of the generated whiteboard texture.")]
    public Vector2 textureSize = new Vector2(2048, 2048);

    Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        fillColor.a = 0.0f;
        
    }

    void Start()
    {
        if (texture)
            _useColor = false;

        resetDrawing();
    }
    
    public void resetDrawing()
    {
        if (!_useColor)
        {
            _renderer.material.shader = Shader.Find("Unlit/Transparent");
            _renderer.material.mainTexture = texture;
            return;
        }

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);

        Color[] fillPixels = new Color[texture.width * texture.height];

        for (int i = 0; i < fillPixels.Length; i++)
            fillPixels[i] = fillColor;

        // Set the pixels and apply the change
        texture.SetPixels(fillPixels);
        texture.Apply();

        
        _renderer.material.shader = Shader.Find("Unlit/Transparent");
        _renderer.material.mainTexture = texture;
    }
}
