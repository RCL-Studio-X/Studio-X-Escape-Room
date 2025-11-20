using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;

public class PageUI : MonoBehaviour
{
    public Image leftTab;
    public Image rightTab;
    private float leftPosX=-0.332f;
    private float rightPosX=0.346f;

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
