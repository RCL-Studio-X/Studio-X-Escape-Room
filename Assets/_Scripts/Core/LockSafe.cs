using System.Collections;
using System.Collections.Generic;
using StudioXRCL.EscapeRoom.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles a safe lock puzzle where users input a numeric combination
    /// via UI buttons to unlock the safe and trigger related events.
    /// </summary>
    public class LockSafe : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Buttons")]
        [Tooltip("Button used for input '0'.")]
        public Button zeroButton;

        [Tooltip("Button used for input '1'.")]
        public Button oneButton;

        [Tooltip("Button used for input '2'.")]
        public Button twoButton;

        [Tooltip("Button used for input '3'.")]
        public Button threeButton;

        [Tooltip("Button used for input '4'.")]
        public Button fourButton;

        [Tooltip("Button used for input '5'.")]
        public Button fiveButton;

        [Tooltip("Button used for input '6'.")]
        public Button sixButton;

        [Tooltip("Button used for input '7'.")]
        public Button sevenButton;

        [Tooltip("Button used for input '8'.")]
        public Button eightButton;

        [Tooltip("Button used for input '9'.")]
        public Button nineButton;

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

        [Header("Target Sequence")]
        [Tooltip("Numeric sequence required to unlock the safe.")]
        public string targetSequence;

        [Header("User Interface")]
        [Tooltip("UI object that hides after the lock succeeds.")]
        public GameObject userInterface;

        [Tooltip("UI object shown when the lock is inactive.")]
        public GameObject lockInterface;

        [Header("Events")]
        [Tooltip("Event invoked when the lock becomes unlocked.")]
        public UnityEvent onUnlocked;

        [Tooltip("Current sequence entered by the user.")]
        public List<char> curSequence = new List<char>();

        #endregion

        #region Private Variable Declarations

        /// <summary> Length of the target numeric sequence. </summary>
        private int _targetLength;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Initializes internal state and determines target sequence length.
        /// </summary>
        private void Awake()
        {
            _targetLength = targetSequence.Length;
        }

        /// <summary>
        /// Registers button click listeners.
        /// </summary>
        private void Start()
        {
            zeroButton.onClick.AddListener(() => AddSequence('0'));
            oneButton.onClick.AddListener(() => AddSequence('1'));
            twoButton.onClick.AddListener(() => AddSequence('2'));
            threeButton.onClick.AddListener(() => AddSequence('3'));
            fourButton.onClick.AddListener(() => AddSequence('4'));
            fiveButton.onClick.AddListener(() => AddSequence('5'));
            sixButton.onClick.AddListener(() => AddSequence('6'));
            sevenButton.onClick.AddListener(() => AddSequence('7'));
            eightButton.onClick.AddListener(() => AddSequence('8'));
            nineButton.onClick.AddListener(() => AddSequence('9'));

            clearButton.onClick.AddListener(ClearSequence);
            enterButton.onClick.AddListener(EnterDirectionalSequence);
            exitButton.onClick.AddListener(ExitUI);
            openButton.onClick.AddListener(EnterUI);
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Adds a numeric input to the current sequence.
        /// </summary>
        /// <param name="digit">The numeric character to add.</param>
        private void AddSequence(char digit)
        {
            if (curSequence.Count >= _targetLength)
                return;

            curSequence.Add(digit);

            if (curSequence.Count == _targetLength)
                SetButtonsInteractable(false, textButtons: false);

            ChangeIndicatorToColor(curSequence.Count - 1, "blue");
        }

        /// <summary>
        /// Validates the entered numeric sequence.
        /// </summary>
        private void EnterDirectionalSequence()
        {
            if (curSequence.Count != _targetLength)
                return;

            string sequenceString = string.Join("", curSequence);

            if (sequenceString == targetSequence)
            {
                audioSource?.Play();
                locked = false;
                onUnlocked?.Invoke();

                ChangeAllIndicatorsColor("green");
                SetButtonsInteractable(false);
                StartCoroutine(HideUIAfterDelay(1.5f));
                return;
            }

            StartCoroutine(FlashIndicators("white", "red", 1.2f, 0.15f));
            SetButtonsInteractable(false);
        }

        /// <summary>
        /// Clears the current sequence and resets indicators.
        /// </summary>
        private void ClearSequence()
        {
            ChangeAllIndicatorsColor("white");
            curSequence.Clear();
            SetButtonsInteractable(true);
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
            for (int i = 0; i < curSequence.Count; i++)
                ChangeIndicatorToColor(i, color);
        }

        /// <summary>
        /// Enables or disables button interactivity.
        /// </summary>
        /// <param name="state">Desired interactable state.</param>
        /// <param name="directionButtons">Whether numeric buttons are affected.</param>
        /// <param name="textButtons">Whether control buttons are affected.</param>
        private void SetButtonsInteractable(bool state, bool directionButtons = true, bool textButtons = true)
        {
            if (directionButtons)
            {
                zeroButton.interactable = state;
                oneButton.interactable = state;
                twoButton.interactable = state;
                threeButton.interactable = state;
                fourButton.interactable = state;
                fiveButton.interactable = state;
                sixButton.interactable = state;
                sevenButton.interactable = state;
                eightButton.interactable = state;
                nineButton.interactable = state;
            }

            if (textButtons)
            {
                clearButton.interactable = state;
                enterButton.interactable = state;
                exitButton.interactable = state;
            }
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

        /// <summary>
        /// Flashes indicators between two colors to indicate an incorrect entry.
        /// </summary>
        /// <param name="baseColor">Base color.</param>
        /// <param name="flashColor">Flash color.</param>
        /// <param name="totalTime">Total flashing duration.</param>
        /// <param name="interval">Flash interval.</param>
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
            SetButtonsInteractable(true);
            ClearSequence();
        }

        #endregion
    }
}
