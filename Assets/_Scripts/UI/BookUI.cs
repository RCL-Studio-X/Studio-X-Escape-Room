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

    }

    public void nextPage()
    {
        if (pageList[pageList.Count - 1])
        {
            nextButton.interactable = false;
        }
        else
        {
            StartCoroutine(nextPageFunction());
        }
    }
    
    IEnumerator nextPageFunction()
    {
        pageList[currentPageIndex].enabled = false;
        yield return new WaitForSeconds(.1f);
        currentPageIndex += 1;
        pageList[currentPageIndex].enabled = true;
    }
}
