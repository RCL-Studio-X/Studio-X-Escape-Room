using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{
    public List<TextMeshProUGUI> pageList;
    public Button nextButton;
    public Button prevButton;
    public int currentPageIndex=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pageList[currentPageIndex].gameObject.SetActive(true);

    }

    public void nextPage()
    {
        if (currentPageIndex<pageList.Count - 1)
        {
            pageList[currentPageIndex].gameObject.SetActive(false);
            currentPageIndex += 1;
            pageList[currentPageIndex].gameObject.SetActive(true);

            prevButton.interactable=true;

            if(currentPageIndex== pageList.Count-1)
            {
                nextButton.interactable=false;
            }
        } 
    }
    
}
