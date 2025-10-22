using UnityEngine;
using System.Linq; // Added this line to use Enumerable.Repeat, though we don't strictly need it here.

public class Whiteboard : MonoBehaviour
{
    [Header("Whiteboard Settings")]
    [Tooltip("Texture applied to the whiteboard surface.")]
    public Texture2D texture;

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

        // Create a new Texture2D
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        
        // --- CHALKBOARD INITIALIZATION: Dark Green ---
        
        // Define the dark green color (R=0.0, G=0.25, B=0.0, A=1.0)
        // Adjust the 0.25f value if you want a lighter or darker green.
        Color chalkboardColor = new Color(0.0f, 0.25f, 0.0f, 1.0f); 
        
        // Create an array of colors to fill the entire texture
        Color[] colors = new Color[(int)(textureSize.x * textureSize.y)];
        
        // Fill the array with the solid dark green color
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = chalkboardColor;
        }

        // Apply the solid colors to the texture
        texture.SetPixels(colors);
        
        // Upload the changes to the GPU
        texture.Apply();

        // Assign the texture to the material
        _renderer.material.mainTexture = texture;
    }
}