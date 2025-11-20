using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;

public class PageUI : MonoBehaviour
{
    //public Image backTab;
    //public Image fowardTab;
    public Image leftTab;
    public Image rightTab;
    private float leftPosX=-244;
    private float rightPosX=246;
    private Vector3 leftTabPos;
    private Vector3 rightTabPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        // Only change X, keep current Y and Z
        leftTab.transform.localPosition = new Vector3(leftPosX, leftTab.transform.localPosition.y, leftTab.transform.localPosition.z);
        rightTab.transform.localPosition = new Vector3(rightPosX, rightTab.transform.localPosition.y, rightTab.transform.localPosition.z);

    }


}
