using UnityEngine;

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
        // If a texture is already assigned in the Inspector, use it and exit.
        if (texture) 
        {
            _renderer.material.mainTexture = texture;
            return;
        }

        // Create a new texture
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        
        // **********************************************
        // THE FIX: Initialize the texture to pure white
        // **********************************************
        
        // 1. Create an array of white colors for the entire texture
        Color[] colors = new Color[(int)(textureSize.x * textureSize.y)];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white; // Assigning pure white (R=1, G=1, B=1, A=1)
        }

        // 2. Apply the white colors to the texture
        texture.SetPixels(colors);
        
        // 3. Apply the changes to the GPU
        texture.Apply();

        // 4. Assign the texture to the material
        _renderer.material.mainTexture = texture;
    }
}