using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;

public class PageUI : MonoBehaviour
{
    public Image leftTab;
    public Image rightTab;
    private float leftPosX=54;
    private float rightPosX=457;
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
        leftTab.transform.position = new Vector3(leftPosX, leftTab.transform.localPosition.y, 0);
        rightTab.transform.position = new Vector3(rightPosX, rightTab.transform.localPosition.y, 0);

    }


}
