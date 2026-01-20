using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles a directional lock puzzle where users input a sequence of directions
    /// to unlock an object and trigger related events, visuals, and audio.
    /// </summary>
    public class LockDirectional : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Buttons")]
        [Tooltip("Button pressed for Up input.")]
        public Button upButton;

        [Tooltip("Button pressed for Down input.")]
        public Button downButton;

        [Tooltip("Button pressed for Left input.")]
        public Button leftButton;

        [Tooltip("Button pressed for Right input.")]
        public Button rightButton;

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
        public LockIndicator[] directionalLockIndicators;

        [Header("Audio")]
        [Tooltip("Audio source played when successfully unlocked.")]
        public AudioSource audioSource;

        [Header("State")]
        [Tooltip("When true, the lock is currently locked.")]
        public bool locked = true;

        [Header("Target Sequence")]
        [Tooltip("Directional sequence required to unlock.")]
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

        /// <summary> Length of the target directional sequence. </summary>
        private int _targetLength;

        /// <summary> Skinned mesh renderer used for unlock blendshape animation. </summary>
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Initializes required components and internal state.
        /// </summary>
        private void Awake()
        {
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            _targetLength = targetSequence.Length;
        }

        /// <summary>
        /// Registers button click listeners.
        /// </summary>
        private void Start()
        {
            upButton.onClick.AddListener(() => AddSequence('u'));
            downButton.onClick.AddListener(() => AddSequence('d'));
            leftButton.onClick.AddListener(() => AddSequence('l'));
            rightButton.onClick.AddListener(() => AddSequence('r'));

            clearButton.onClick.AddListener(ClearSequence);
            enterButton.onClick.AddListener(EnterDirectionalSequence);
            exitButton.onClick.AddListener(ExitUI);
            openButton.onClick.AddListener(EnterUI);
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Adds a directional input to the current sequence.
        /// </summary>
        /// <param name="dir">The direction character to add.</param>
        private void AddSequence(char dir)
        {
            if (curSequence.Count >= _targetLength)
                return;

            curSequence.Add(dir);

            if (curSequence.Count == _targetLength)
                SetButtonsInteractable(false, textButtons: false);

            ChangeIndicatorToColor(curSequence.Count - 1, "blue");
        }

        /// <summary>
        /// Validates the entered directional sequence.
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

                StartCoroutine(UnlockBlendshape());
                ChangeAllIndicatorsColor("green");

                SetButtonsInteractable(false);
                StartCoroutine(HideUIAfterDelay(1.5f));
                return;
            }

            StartCoroutine(FlashIndicators("white", "red", 1.2f, 0.15f));
            SetButtonsInteractable(false);
        }

        /// <summary>
        /// Clears the current input sequence and resets indicators.
        /// </summary>
        private void ClearSequence()
        {
            ChangeAllIndicatorsColor("white");
            curSequence.Clear();
            SetButtonsInteractable(true);
        }

        /// <summary>
        /// Exits the lock UI and returns to the lock interface.
        /// </summary>
        private void ExitUI()
        {
            ClearSequence();
            userInterface.SetActive(false);
            lockInterface.SetActive(true);
        }

        /// <summary>
        /// Enters the lock UI.
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
            if (directionalLockIndicators[index] == null)
                return;

            directionalLockIndicators[index].ChangeIndicatorImage(color);
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
        /// <param name="directionButtons">Whether direction buttons are affected.</param>
        /// <param name="textButtons">Whether text buttons are affected.</param>
        private void SetButtonsInteractable(bool state, bool directionButtons = true, bool textButtons = true)
        {
            if (directionButtons)
            {
                upButton.interactable = state;
                downButton.interactable = state;
                leftButton.interactable = state;
                rightButton.interactable = state;
            }

            if (textButtons)
            {
                clearButton.interactable = state;
                enterButton.interactable = state;
                exitButton.interactable = state;
            }
        }

        /// <summary>
        /// Animates the unlock blendshape.
        /// </summary>
        private IEnumerator UnlockBlendshape()
        {
            for (float s = 0f; s <= 100f; s++)
            {
                _skinnedMeshRenderer?.SetBlendShapeWeight(0, s);
                yield return null;
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
        /// Flashes indicators between two colors for feedback.
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
