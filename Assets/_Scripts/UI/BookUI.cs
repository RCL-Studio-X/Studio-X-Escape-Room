using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StudioX.EscapeRoom.UI;

namespace StudioXRCL.EscapeRoom.UI
{
    /// <summary>
    /// Manages the UI behavior for an interactive book, including page navigation
    /// and opening/closing the book canvas.
    /// </summary>
    public class BookUI : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Page Settings")]
        [Tooltip("All page content GameObjects that make up the book.")]
        public List<GameObject> pageList;

        [Header("Navigation Buttons")]
        [Tooltip("Button used to navigate to the next page.")]
        public Button nextButton;

        [Tooltip("Button used to navigate to the previous page.")]
        public Button prevButton;

        [Header("Book Controls")]
        [Tooltip("Canvas that contains the book UI.")]
        public Canvas bookCanvas;

        [Tooltip("Button used to open the book.")]
        public Button openButton;

        [Tooltip("Button used to close the book.")]
        public Button closeButton;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Tracks the currently active page index.
        /// </summary>
        private int _currentPageIndex = 0;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Initializes the book UI state and validates page data.
        /// </summary>
        private void Start()
        {
            if (pageList == null || pageList.Count == 0)
            {
                Debug.LogError("Page list is empty or null in BookUI.");
                return;
            }

            UpdatePageDisplay();
            prevButton.interactable = false;
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Updates page visibility and navigation button interactability
        /// based on the current page index.
        /// </summary>
        public void UpdatePageDisplay()
        {
            foreach (GameObject page in pageList)
            {
                page.SetActive(false);
            }

            pageList[_currentPageIndex].SetActive(true);

            prevButton.interactable = _currentPageIndex > 0;
            nextButton.interactable = _currentPageIndex < pageList.Count - 1;
        }

        /// <summary>
        /// Advances the book to the next page if available.
        /// </summary>
        public void NextPage()
        {
            if (_currentPageIndex < pageList.Count - 1)
            {
                _currentPageIndex++;
                UpdatePageDisplay();
            }
        }

        /// <summary>
        /// Returns the book to the previous page if available.
        /// </summary>
        public void PreviousPage()
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                UpdatePageDisplay();
            }
        }

        /// <summary>
        /// Opens the book and enables its UI canvas.
        /// </summary>
        public void OpenBook()
        {
            bookCanvas.gameObject.SetActive(true);
            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// Closes the book and disables its UI canvas.
        /// </summary>
        public void CloseBook()
        {
            bookCanvas.gameObject.SetActive(false);
            openButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
        }

        #endregion
    }
}
