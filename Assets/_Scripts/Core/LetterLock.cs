using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using StudioXRCL.EscapeRoom.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Controls the logic and user interface for a 5-letter combination lock.
    /// </summary>
    public class LetterLock : MonoBehaviour
    {
        #region Public Variable declarations

        [Header("Buttons")]
        [Tooltip("Button used for input 1 next letter.")]
        public Button nextButton1;
        [Tooltip("Button used for input 1 previous letter.")]
        public Button prevButton1;
        [Tooltip("Text display for input 1.")]
        public TextMeshProUGUI letter1;

        [Tooltip("Button used for input 2 next letter.")]
        public Button nextButton2;
        [Tooltip("Button used for input 2 previous letter.")]
        public Button prevButton2;
        [Tooltip("Text display for input 2.")]
        public TextMeshProUGUI letter2;

        [Tooltip("Button used for input 3 next letter.")]
        public Button nextButton3;
        [Tooltip("Button used for input 3 previous letter.")]
        public Button prevButton3;
        [Tooltip("Text display for input 3.")]
        public TextMeshProUGUI letter3;

        [Tooltip("Button used for input 4 next letter.")]
        public Button nextButton4;
        [Tooltip("Button used for input 4 previous letter.")]
        public Button prevButton4;
        [Tooltip("Text display for input 4.")]
        public TextMeshProUGUI letter4;

        [Tooltip("Button used for input 5 next letter.")]
        public Button nextButton5;
        [Tooltip("Button used for input 5 previous letter.")]
        public Button prevButton5;
        [Tooltip("Text display for input 5.")]
        public TextMeshProUGUI letter5;

        [Tooltip("Button used to clear the current sequence.")]
        public Button clearButton;

        [Tooltip("Button used to submit the entered sequence.")]
        public Button enterButton;

        [Tooltip("Button used to exit the UI canvas.")]
        public Button exitButton;

        [Tooltip("Button used to open the lock UI.")]
        public Button openButton;

        [Header("Indicators")]
        [Tooltip("Indicator lights that show the current input.")]
        public LockIndicator[] lockIndicators;

        [Header("Audio")]
        [Tooltip("Audio source played when successfully unlocked.")]
        public AudioSource audioSource;

        [Header("State")]
        [Tooltip("When true, the lock is currently locked.")]
        public bool locked = true;

        [Header("User Interface")]
        [Tooltip("UI object that hides after the lock succeeds.")]
        public GameObject userInterface;

        [Tooltip("UI object shown when the lock is inactive.")]
        public GameObject lockInterface;

        [Header("Events")]
        [Tooltip("Event invoked when the lock becomes unlocked.")]
        public UnityEvent onUnlocked;

        #endregion

        #region Private Variable declarations

        /// <summary> Current index for letter 1. </summary>
        private int _letter1Index = 0;

        /// <summary> Current index for letter 2. </summary>
        private int _letter2Index = 0;

        /// <summary> Current index for letter 3. </summary>
        private int _letter3Index = 0;

        /// <summary> Current index for letter 4. </summary>
        private int _letter4Index = 0;

        /// <summary> Current index for letter 5. </summary>
        private int _letter5Index = 0;

        /// <summary> Array of letters user can enter. </summary>
        private string[] _letterList = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        #endregion

        #region Public Method definitions

        /// <summary>
        /// Cycles to the next letter for input 1.
        /// </summary>
        public void Letter1NextClicked()
        {
            if (_letter1Index == _letterList.Length - 1)
            {
                _letter1Index = 0;
            }
            else
            {
                _letter1Index++;
            }
            letter1.SetText(_letterList[_letter1Index]);
        }

        /// <summary>
        /// Cycles to the previous letter for input 1.
        /// </summary>
        public void Letter1PrevClicked()
        {
            if (_letter1Index == 0)
            {
                _letter1Index = _letterList.Length - 1;
            }
            else
            {
                _letter1Index--;
            }
            letter1.SetText(_letterList[_letter1Index]);
        }

        /// <summary>
        /// Cycles to the next letter for input 2.
        /// </summary>
        public void Letter2NextClicked()
        {
            if (_letter2Index == _letterList.Length - 1)
            {
                _letter2Index = 0;
            }
            else
            {
                _letter2Index++;
            }
            letter2.SetText(_letterList[_letter2Index]);
        }

        /// <summary>
        /// Cycles to the previous letter for input 2.
        /// </summary>
        public void Letter2PrevClicked()
        {
            if (_letter2Index == 0)
            {
                _letter2Index = _letterList.Length - 1;
            }
            else
            {
                _letter2Index--;
            }
            letter2.SetText(_letterList[_letter2Index]);
        }

        /// <summary>
        /// Cycles to the next letter for input 3.
        /// </summary>
        public void Letter3NextClicked()
        {
            if (_letter3Index == _letterList.Length - 1)
            {
                _letter3Index = 0;
            }
            else
            {
                _letter3Index++;
            }
            letter3.SetText(_letterList[_letter3Index]);
        }

        /// <summary>
        /// Cycles to the previous letter for input 3.
        /// </summary>
        public void Letter3PrevClicked()
        {
            if (_letter3Index == 0)
            {
                _letter3Index = _letterList.Length - 1;
            }
            else
            {
                _letter3Index--;
            }
            letter3.SetText(_letterList[_letter3Index]);
        }

        /// <summary>
        /// Cycles to the next letter for input 4.
        /// </summary>
        public void Letter4NextClicked()
        {
            if (_letter4Index == _letterList.Length - 1)
            {
                _letter4Index = 0;
            }
            else
            {
                _letter4Index++;
            }
            letter4.SetText(_letterList[_letter4Index]);
        }

        /// <summary>
        /// Cycles to the previous letter for input 4.
        /// </summary>
        public void Letter4PrevClicked()
        {
            if (_letter4Index == 0)
            {
                _letter4Index = _letterList.Length - 1;
            }
            else
            {
                _letter4Index--;
            }
            letter4.SetText(_letterList[_letter4Index]);
        }

        /// <summary>
        /// Cycles to the next letter for input 5.
        /// </summary>
        public void Letter5NextClicked()
        {
            if (_letter5Index == _letterList.Length - 1)
            {
                _letter5Index = 0;
            }
            else
            {
                _letter5Index++;
            }
            letter5.SetText(_letterList[_letter5Index]);
        }

        /// <summary>
        /// Cycles to the previous letter for input 5.
        /// </summary>
        public void Letter5PrevClicked()
        {
            if (_letter5Index == 0)
            {
                _letter5Index = _letterList.Length - 1;
            }
            else
            {
                _letter5Index--;
            }
            letter5.SetText(_letterList[_letter5Index]);
        }

        /// <summary>
        /// Clears the current sequence and resets the displayed letters.
        /// </summary>
        public void ClearSequence()
        {
            _letter1Index = 0;
            letter1.SetText(_letterList[_letter1Index]);

            _letter2Index = 0;
            letter2.SetText(_letterList[_letter2Index]);

            _letter3Index = 0;
            letter3.SetText(_letterList[_letter3Index]);

            _letter4Index = 0;
            letter4.SetText(_letterList[_letter4Index]);

            _letter5Index = 0;
            letter5.SetText(_letterList[_letter5Index]);
        }

        /// <summary>
        /// Exits the safe UI and returns to the lock interface.
        /// </summary>
        public void ExitUI()
        {
            ClearSequence();
            userInterface.SetActive(false);
            lockInterface.SetActive(true);
        }

        /// <summary>
        /// Enters the safe UI.
        /// </summary>
        public void EnterUI()
        {
            ClearSequence();
            userInterface.SetActive(true);
            lockInterface.SetActive(false);
        }

        /// <summary>
        /// Submits the entered sequence and unlocks if the combination is correct.
        /// </summary>
        public void OnEnter()
        {
            // Lock code is set to B, B, B, B, B (index 1 for all)
            if (_letter1Index == 1 && _letter2Index == 1 && _letter3Index == 1 && _letter4Index == 1 && _letter5Index == 1)
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

        #endregion

        #region Private Method definitions

        /// <summary>
        /// Registers button click listeners on initialization.
        /// </summary>
        private void Start()
        {
            clearButton.onClick.AddListener(ClearSequence);
            exitButton.onClick.AddListener(ExitUI);
            openButton.onClick.AddListener(EnterUI);
        }

        /// <summary>
        /// Changes a specific indicator's color.
        /// </summary>
        /// <param name="index">Index of the indicator in the array.</param>
        /// <param name="color">Color name to apply.</param>
        private void ChangeIndicatorToColor(int index, string color)
        {
            if (lockIndicators[index] == null)
                return;

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

            if (userInterface != null)
                userInterface.SetActive(false);

            if (lockInterface != null)
                lockInterface.SetActive(false);
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
            ClearSequence();
        }

        #endregion
    }
}