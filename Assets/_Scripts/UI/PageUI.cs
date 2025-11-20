using UnityEngine;
using UnityEngine.UI;

public class PageUI : MonoBehaviour
{
    // Assign these in the Unity Inspector
    public Image leftTab;
    public Image rightTab;

    // Define the target X positions (Local Coordinates)
    private float leftPosX = -244f;
    private float rightPosX = 246f;
    
    // Variables to store the initial LOCAL Y positions
    private float initialLeftLocalY;
    private float initialRightLocalY;
    
    // Variables to hold the final target Vector3 positions
    private Vector3 leftTabPos;
    private Vector3 rightTabPos;

    void Start()
    {
        // 1. Store the CURRENT LOCAL Y positions of the tabs.
        // This makes sure the tabs stay vertically aligned where you placed them.
        if (leftTab != null)
        {
            initialLeftLocalY = leftTab.transform.localPosition.y;
        }
        if (rightTab != null)
        {
            initialRightLocalY = rightTab.transform.localPosition.y;
        }

        // 2. Pre-calculate the target Vector3 positions.
        leftTabPos = new Vector3(leftPosX, initialLeftLocalY, 0f);
        rightTabPos = new Vector3(rightPosX, initialRightLocalY, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Set the local positions every frame using the pre-calculated vectors.
        // This ensures they are constantly forced to the target (leftPosX, initialLocalY)
        if (leftTab != null)
        {
            leftTab.transform.localPosition = leftTabPos;
        }
        if (rightTab != null)
        {
            rightTab.transform.localPosition = rightTabPos;
        }
    }
}