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
public class LetterLock : MonoBehaviour
{

        [Header("Buttons")]
        [Tooltip("Button used for input 1.")]
        public Button nextButton1;
        public Button prevButton1;
        public TextMeshProUGUI letter1;
        private int letter1Index=0;


        [Tooltip("Button used for input 2.")]
        public Button nextButton2;
        public Button prevButton2;
        public TextMeshProUGUI letter2;
        private int letter2Index=0;


        [Tooltip("Button used for input 3.")]
        public Button nextButton3;
        public Button prevButton3;
        public TextMeshProUGUI letter3;
        private int letter3Index=0;


        [Tooltip("Button used for input 4.")]
        public Button nextButton4;
        public Button prevButton4;
        public TextMeshProUGUI letter4;
        private int letter4Index=0;


        [Tooltip("Button used for input 5.")]
        public Button nextButton5;
        public Button prevButton5;
        public TextMeshProUGUI letter5;
        private int letter5Index=0;


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

        [Tooltip(" Array of letters user can enter.")]
        private string[] letterList = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };


        /// <summary>
        /// Registers button click listeners.
        /// </summary>
        private void Start()
        {
            clearButton.onClick.AddListener(ClearSequence);
            exitButton.onClick.AddListener(ExitUI);
            openButton.onClick.AddListener(EnterUI);
        }

        public void letter1NextClicked()
        {
            if (letter1Index==letterList.Length-1) // if at the last letter then loop index back to the first team
            {
                letter1Index=0;
            } else
            {
                letter1Index++;
            }
            letter1.SetText(letterList[letter1Index]);
        }

        public void letter1PrevClicked()
        {
              if (letter1Index==0) // if at the first letter then loop index back to the last letter
            {
                letter1Index= letterList.Length - 1;
            } else
            {
                letter1Index--;
            }
            letter1.SetText(letterList[letter1Index]);
        }

        public void letter2NextClicked()
        {
            if (letter2Index==letterList.Length - 1) // if at the last letter then loop index back to the first team
            {
                letter2Index=0;
            } else
            {
                letter2Index++;
            }
            letter2.SetText(letterList[letter2Index]);
        }

        public void letter2PrevClicked()
        {
              if (letter2Index==0) // if at the first letter then loop index back to the last letter
            {
                letter2Index=9;
            } else
            {
                letter2Index--;
            }
            letter2.SetText(letterList[letter2Index]);
        }

        public void letter3NextClicked()
        {
            if (letter3Index==letterList.Length - 1) // if at the last letter then loop index back to the first team
            {
                letter3Index=0;
            } else
            {
                letter3Index++;
            }
            letter3.SetText(letterList[letter3Index]);
        }

        public void letter3PrevClicked()
        {
              if (letter3Index==0) // if at the first letter then loop index back to the last letter
            {
                letter3Index=letterList.Length - 1;
            } else
            {
                letter3Index--;
            }
            letter3.SetText(letterList[letter3Index]);
        }

        public void letter4NextClicked()
        {
            if (letter4Index==letterList.Length - 1) // if at the last letter then loop index back to the first team
            {
                letter4Index=0;
            } else
            {
                letter4Index++;
            }
            letter4.SetText(letterList[letter4Index]);
        }

        public void letter4PrevClicked()
        {
              if (letter4Index==0) // if at the first letter then loop index back to the last letter
            {
                letter4Index=letterList.Length - 1;
            } else
            {
                letter4Index--;
            }
            letter4.SetText(letterList[letter4Index]);
        }

        public void letter5NextClicked()
        {
            if (letter5Index==letterList.Length - 1) // if at the last letter then loop index back to the first team
            {
                letter5Index=0;
            } else
            {
                letter5Index++;
            }
            letter5.SetText(letterList[letter5Index]);
        }

        public void letter5PrevClicked()
        {
              if (letter5Index==0) // if at the first letter then loop index back to the last letter
            {
                letter5Index=letterList.Length - 1;
            } else
            {
                letter5Index--;
            }
            letter5.SetText(letterList[letter5Index]);
        }



        /// <summary>
        /// Clears the current sequence and resets indicators.
        /// </summary>
        public void ClearSequence()
        {
            //ChangeAllIndicatorsColor("white");
            letter1Index=0;
            letter1.SetText(letterList[letter1Index]);

            letter2Index=0;
            letter2.SetText(letterList[letter2Index]);

            letter3Index=0;
            letter3.SetText(letterList[letter3Index]);

            letter4Index=0;
            letter4.SetText(letterList[letter4Index]);

            letter5Index=0;
            letter5.SetText(letterList[letter5Index]);
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
        /// Changes a specific indicator's color.
        /// </summary>
        /// <param name="index">Index of the indicator.</param>
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
        /// Hides the UI after a delay.
        /// </summary>
        /// <param name="delay">Delay in seconds.</param>
        private IEnumerator HideUIAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (userInterface != null)
                userInterface.SetActive(false);

            if (lockInterface != null)
                lockInterface.SetActive(false);
        }

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

        public void onEnter()
        {
            if (letter1Index==1 && letter2Index==1 && letter3Index==1 && letter4Index==1 && letter5Index==1)//lock code (set to b,b,b,b,b)
            {
                onUnlocked.Invoke();
                audioSource.Play();
                StartCoroutine(FlashIndicators("white", "green", 2, .3f)); 
                StartCoroutine(HideUIAfterDelay(2));
                locked = false;
            } else
            {
            ClearSequence();
            StartCoroutine(FlashIndicators("white", "red", 1.5f, .3f));

            }
        }

    }
