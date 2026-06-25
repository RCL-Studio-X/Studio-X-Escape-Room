using System.Collections;
using StudioXRCL.EscapeRoom.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;


namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Controls the logic and user interface for a multi-dial letter lock.
    /// </summary>
    public class LetterLock : MonoBehaviour
    {
        /// <summary>
        /// Represents an individual dial on the lock.
        /// </summary>
        [System.Serializable]
        public class LockDial
        {
            [Tooltip("The TextMeshPro text for this specific dial.")]
            public TextMeshProUGUI letterDisplay;

            [Tooltip("The correct array index to unlock this dial (e.g., 1 for 'B').")]
            public int correctIndex;

            [HideInInspector]
            [Tooltip("The current array index selected on this dial.")]
            public int currentIndex = 0;
        }

        #region Public Variable declarations

        [Header("Dials")]
        [Tooltip("Add your dials here.")]
        public LockDial[] dials;

        [Header("UI Buttons")]
        [Tooltip("Button used to clear the current sequence.")]
        public Button clearButton;

        [Tooltip("Button used to submit the entered sequence.")]
        public Button enterButton;

        [Tooltip("Button used to exit the UI canvas.")]
        public Button exitButton;

        [Tooltip("Button used to open the lock UI.")]
        public Button openButton;

        [Header("Indicators & Audio")]
        [Tooltip("Indicator lights that show the current input status.")]
        public LockIndicator[] lockIndicators;

        [Tooltip("Audio source played when successfully unlocked.")]
        public AudioSource audioSource;

        [Tooltip("When true, the lock is currently locked.")]
        public bool locked = true;

        [Header("User Interface & Events")]
        [Tooltip("UI object that hides after the lock succeeds.")]
        public GameObject userInterface;

        [Tooltip("UI object shown when the lock is inactive.")]
        public GameObject lockInterface;

        [Tooltip("Event invoked when the lock becomes unlocked.")]
        public UnityEvent onUnlocked;

        [Tooltip("Quick workaround for the skull puzzle.")]
        public bool usingAlternate;

        #endregion

        #region Private Variable declarations

        /// <summary> Array of letters the user can cycle through. </summary>
        private string[] _letterList = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        private string[] _letterListAlternate = { "A", "N", "C", "D", "E", "F", "S", "H", "I", "T" };

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Cycles to the next letter for a specific dial.
        /// </summary>
        /// <param name="dialIndex">The index of the dial to update.</param>
        public void NextLetter(int dialIndex)
        {
            LockDial dial = dials[dialIndex]; // Grab the specific dial

            if (usingAlternate)
            {
                if (dial.currentIndex == _letterListAlternate.Length - 1)
                {
                    dial.currentIndex = 0;
                }
                else
                {
                    dial.currentIndex++;
                }
                dial.letterDisplay.SetText(_letterListAlternate[dial.currentIndex]);
            }
            else
            {
                if (dial.currentIndex == _letterList.Length - 1)
                {
                    dial.currentIndex = 0;
                }
                else
                {
                    dial.currentIndex++;
                }
                dial.letterDisplay.SetText(_letterList[dial.currentIndex]);
            }
        }

        /// <summary>
        /// Cycles to the previous letter for a specific dial.
        /// </summary>
        /// <param name="dialIndex">The index of the dial to update.</param>
        public void PrevLetter(int dialIndex)
        {
            LockDial dial = dials[dialIndex];

            if (usingAlternate)
            {
                if (dial.currentIndex == 0)
                {
                    dial.currentIndex = _letterListAlternate.Length - 1;
                }
                else
                {
                    dial.currentIndex--;
                }
                dial.letterDisplay.SetText(_letterListAlternate[dial.currentIndex]);
            }
            else
            {
                if (dial.currentIndex == 0)
                {
                    dial.currentIndex = _letterList.Length - 1;
                }
                else
                {
                    dial.currentIndex--;
                }
                dial.letterDisplay.SetText(_letterList[dial.currentIndex]);
            }
            
        }

        
        #endregion

        #region Private Method definitions

        /// <summary>
        /// Clears the current sequence and resets all dials.
        /// </summary>
        private void ClearSequence()
        {   
            if (usingAlternate)
            {
                foreach (LockDial dial in dials)
                {
                    dial.currentIndex = 0;
                    dial.letterDisplay.SetText(_letterListAlternate[0]);
                }
            }
            else
            {
                foreach (LockDial dial in dials)
                {
                    dial.currentIndex = 0;
                    dial.letterDisplay.SetText(_letterList[0]);
                }
            }
            // One simple loop resets every single dial
            
            ChangeAllIndicatorsColor("white");
        }

        /// <summary>
        /// Exits the safe UI and returns to the lock interface.
        /// </summary>
        private void ExitUI()
        {
            ClearSequence();
            userInterface.SetActive(false);
            lockInterface.SetActive(true);
        }

        /// <summary>
        /// Enters the safe UI.
        /// </summary>
        private void EnterUI()
        {
            ClearSequence();
            userInterface.SetActive(true);
            lockInterface.SetActive(false);
        }

        /// <summary>
        /// Submits the entered sequence across all dials and unlocks if correct.
        /// </summary>
        private void OnEnter()
        {
            bool isCorrect = true;

            // Loop through all dials. If even ONE is wrong, fail the whole lock.

            if (usingAlternate)
            {
                string text = "";

                foreach (LockDial dial in dials)
                {
                    text += _letterListAlternate[dial.correctIndex];
                }

                // Now check if text contains S, T, A, N, C
                char[] required = { 'S', 'T', 'A', 'N', 'C' };

                bool hasAllLetters = required.All(letter => text.Contains(letter));
            }
            else
            {
                foreach (LockDial dial in dials)
                {
                    if (dial.currentIndex != dial.correctIndex)
                    {
                        isCorrect = false;
                        break; // Stop checking, we already know it's wrong
                    }
                }
            }
            

            if (isCorrect)
            {
                onUnlocked.Invoke();
                audioSource.Play();
                StartCoroutine(FlashIndicators("white", "green", 2f, .3f));
                StartCoroutine(HideUIAfterDelay(2f));
                locked = false;
            }
            else
            {
                ClearSequence();
                StartCoroutine(FlashIndicators("white", "red", 1.5f, .3f));
            }
        }


        /// <summary>
        /// Registers button click listeners on initialization.
        /// </summary>
        private void Start()
        {
            clearButton.onClick.AddListener(ClearSequence);
            exitButton.onClick.AddListener(ExitUI);
            openButton.onClick.AddListener(EnterUI);
            enterButton.onClick.AddListener(OnEnter);
        }

        /// <summary>
        /// Changes a specific indicator's color.
        /// </summary>
        /// <param name="index">Index of the indicator in the array.</param>
        /// <param name="color">Color name to apply.</param>
        private void ChangeIndicatorToColor(int index, string color)
        {
            if (lockIndicators[index] != null)
                lockIndicators[index].ChangeIndicatorImage(color);
        }

        /// <summary>
        /// Changes all active indicators to a given color.
        /// </summary>
        /// <param name="color">Color name to apply.</param>
        private void ChangeAllIndicatorsColor(string color)
        {
            for (int i = 0; i < lockIndicators.Length; i++)
                ChangeIndicatorToColor(i, color);
        }

        /// <summary>
        /// Hides the UI after a specific delay.
        /// </summary>
        /// <param name="delay">Delay in seconds before hiding the UI.</param>
        /// <returns>An IEnumerator to be used in a Coroutine.</returns>
        private IEnumerator HideUIAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (userInterface != null) userInterface.SetActive(false);
            if (lockInterface != null) lockInterface.SetActive(false);
        }

        /// <summary>
        /// Flashes the indicator lights between a base color and a flash color.
        /// </summary>
        /// <param name="baseColor">The default color to revert to.</param>
        /// <param name="flashColor">The secondary color to flash to.</param>
        /// <param name="totalTime">The total duration of the flashing effect.</param>
        /// <param name="interval">The speed at which the colors toggle.</param>
        /// <returns>An IEnumerator to be used in a Coroutine.</returns>
        private IEnumerator FlashIndicators(string baseColor, string flashColor, float totalTime, float interval)
        {
            float elapsed = 0f;
            bool flashing = false;

            while (elapsed < totalTime)
            {
                flashing = !flashing;
                ChangeAllIndicatorsColor(flashing ? flashColor : baseColor);
                yield return new WaitForSeconds(interval);
                elapsed += interval;
            }

            ChangeAllIndicatorsColor(baseColor);
        }

        #endregion
    }
}