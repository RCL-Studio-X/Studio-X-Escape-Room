using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{
    public List<Image> pageList; // All page content elements
    public Button nextButton;              // Button to go to the next page
    public Button prevButton;              // Button to go to the previous page
    public Canvas bookCanvas;
    public Button openButton;
    
    private int currentPageIndex = 0;      

    void Start()
    {
        // Check pageList has pages
        if (pageList == null || pageList.Count == 0)
        {
            Debug.LogError("Page list is empty or null in BookUI.");
            return;
        }

        // Set up initial state (Page 0 active, buttons disabled/enabled)
        UpdatePageDisplay();
        
        // Prev button should be disabled at the start (on page 0)
        prevButton.interactable = false;
    }
    
    // Helper function to update page visibility and button interactability
    private void UpdatePageDisplay()
    {
        // Deactivate all pages first
        foreach (var page in pageList)
        {
            page.gameObject.SetActive(false);
        }
        
        // Activate the current page
        pageList[currentPageIndex].gameObject.SetActive(true);

        // Update button states
        prevButton.interactable = (currentPageIndex > 0);
        nextButton.interactable = (currentPageIndex < pageList.Count - 1);
    }

    public void nextPage()
    {
        // Check if there is a next page available
        if (currentPageIndex < pageList.Count - 1)
        {
            currentPageIndex++;
            UpdatePageDisplay();
        }
    }

    public void prevPage()
    {
        // Check if there is a previous page available
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageDisplay();
        }
    }

    public void openBook()
    {
        bookCanvas.gameObject.SetActive(true);
        openButton.interactable = false;
    }

    public void closeBook()
    {
        bookCanvas.gameObject.SetActive(false);
        openButton.interactable = true;
    }
}