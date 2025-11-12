using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class directionalLockIndicator : MonoBehaviour
{
    public Sprite whiteIndicatorSprite;
    public Sprite blueIndicatorSprite;
    public Sprite greenIndicatorSprite;
    public Sprite redIndicatorSprite;
    private Image image;
    void Start()
    {
        // This shouldn't fail because the script requires the Image component.
        image = GetComponent<Image>();
    }
    
    public void ChangeIndicatorImage(string color)
    {
        if (!image)
            return;
        
        switch (color)
        {
            case "blue":
                image.sprite = blueIndicatorSprite;
                break;
            case "green":
                image.sprite = greenIndicatorSprite;
                break;
            case "red":
                image.sprite = redIndicatorSprite;
                break;
            default:
                image.sprite = whiteIndicatorSprite;
                break;
        }
    }
}
