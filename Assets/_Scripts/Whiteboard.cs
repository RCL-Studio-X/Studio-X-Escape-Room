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
        if (texture) 
        {
            _renderer.material.mainTexture = texture;
            return;
        }

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        _renderer.material.mainTexture = texture;
    }
}