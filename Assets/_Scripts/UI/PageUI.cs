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
    private float leftPosY=235;
    private float rightPosX=246;
    private float rightPosY=211;
    private Vector3 leftTabPos;
    private Vector3 rightTabPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftTabPos= new Vector3(leftPosX,leftPosY,0);
        rightTabPos= new Vector3(rightPosX, rightPosY,0);
        
    }

    // Update is called once per frame
    void Update()
    {
        leftTab.transform.localPosition = leftTabPos;
        rightTab.transform.localPosition = rightTabPos;
        
    }

}