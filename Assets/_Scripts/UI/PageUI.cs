using UnityEngine;
using UnityEngine.UI;

// Removed: using System.Numerics;

public class PageUI : MonoBehaviour
{
    private float zFowardPos = .15f;
    private float zbackPos = .2f;
    public GameObject tabFoward; 
    public GameObject tabBack; 

    void Update()
    {
        Vector3 forwardPos = new Vector3(tabFoward.transform.position.x, tabFoward.transform.position.y, zFowardPos);
        
        Vector3 backwardPos = new Vector3(tabBack.transform.position.x, tabBack.transform.position.y, zbackPos);

        if (tabFoward != null)
        {
            tabFoward.transform.position = forwardPos;
        }

        if (tabBack != null) 
        {
            tabBack.transform.position = backwardPos;
        }
    }
}