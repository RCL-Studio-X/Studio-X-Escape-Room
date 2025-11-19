using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class PushLockScript : MonoBehaviour
{
    [Header("Buttons")]
    [Tooltip("Button pressed for Up input.")]
    public Button oneButton;
    [Tooltip("Button pressed for Down input.")]
    public Button twoButton;
    [Tooltip("Button pressed for Left input.")]
    public Button threeButton;
    [Tooltip("Button pressed for Right input.")]
    public Button fourButton;
    [Tooltip("Button used to clear the current sequence.")]
    public Button fiveButton;
    public Button sixButton;
    public Button sevenButton;
    public Button eightButton;
    public Button clearButton;
    [Tooltip("Button used to submit the entered sequence.")]
    public Button enterButton;
    [Tooltip("Button used to exit the UI Canvas")]
    public Button exitButton;

    [Header("Indicators")]
    [Tooltip("Indicator lights that show the current input.")]
    public directionalLockIndicator[] directionalLockIndicators;

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
    
    [Header("Events")]
    [Tooltip("Event invoked when the lock becomes unlocked.")]
    public UnityEvent onUnlocked;
    public List<char> curSequence;

    private int _targetLength;
    // private SkinnedMeshRenderer _skinnedMeshRenderer; Commenting out skinMeshrendered stuff for now

    private void Awake()
    {
        // _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>(); Commenting out skinMeshrendered stuff for now
        curSequence = new List<char>();
        _targetLength = targetSequence.Length;
    }

    private void Start()
    {
        // Added the buttons and the new sequences
        oneButton.onClick.AddListener(() => AddSequence('1'));
        twoButton.onClick.AddListener(() => AddSequence('2'));
        threeButton.onClick.AddListener(() => AddSequence('3'));
        fourButton.onClick.AddListener(() => AddSequence('4'));
        fiveButton.onClick.AddListener(() => AddSequence('5'));
        sixButton.onClick.AddListener(() => AddSequence('6'));
        sevenButton.onClick.AddListener(() => AddSequence('7'));
        eightButton.onClick.AddListener(() => AddSequence('8'));
        clearButton.onClick.AddListener(ClearSequence);
        enterButton.onClick.AddListener(EnterDirectionalSequence);
        exitButton.onClick.AddListener(ExitUI);
    }

    private void AddSequence(char dir)
    {
        if (curSequence.Count >= _targetLength)
            return;

        curSequence.Add(dir);
        
        if (curSequence.Count == _targetLength)
            SetButtonsInteractable(false, textButtons:false);
        
        ChangeIndicatorToColor(curSequence.Count - 1, "blue");
    }

    private void EnterDirectionalSequence()
    {
        if (curSequence.Count != _targetLength)
            return;

        var sequenceString = string.Join("", curSequence);

        if (sequenceString == targetSequence)
        {
            if (audioSource is { })
                audioSource.Play();

            locked = false;
            onUnlocked?.Invoke();
            // StartCoroutine(UnlockBlendshape()); Commenting out skinMeshrendered stuff for now
            ChangeAllIndicatorsColor("green");
            
            SetButtonsInteractable(false);
            StartCoroutine(HideUIAfterDelay(1.5f));  
            return;
        }
        

        StartCoroutine(FlashIndicators("white", "red", 1.2f, 0.15f));
        SetButtonsInteractable(false);
    }

    private void ClearSequence()
    {
        ChangeAllIndicatorsColor("white");
        curSequence.Clear();
        SetButtonsInteractable(true);
    }

    private void ExitUI()
    {
        ClearSequence();
        userInterface.SetActive(false);
    }

    private void ChangeIndicatorToColor(int index, string color)
    {
        if (directionalLockIndicators[index] is not { } indicator)
            return;

        indicator.ChangeIndicatorImage(color);
    }

    private void ChangeAllIndicatorsColor(string color)
    {
        for (int i = 0; i < curSequence.Count; i++)
            ChangeIndicatorToColor(i, color);
    }

    private void SetButtonsInteractable(bool state, bool directionButtons = true, bool textButtons = true)
    {
        if (directionButtons)
        {
            // updated to match buttons now
            oneButton.interactable = state;
            twoButton.interactable = state;
            threeButton.interactable = state;
            fourButton.interactable = state;
            fiveButton.interactable = state;
            sixButton.interactable = state;
            sevenButton.interactable = state;
            eightButton.interactable = state;
        }

        if (textButtons)
        {
            clearButton.interactable = state;
            enterButton.interactable = state;
            exitButton.interactable = state;
        }
    }

    /* Since there is no skinnedMeshrender I'm just going to comment out all of the animations for now
    
        private IEnumerator UnlockBlendshape()
    {
        for (float s = 0f; s < 100f; s++)
        {
            if (_skinnedMeshRenderer)
                _skinnedMeshRenderer.SetBlendShapeWeight(0, s);

            yield return null;
        }
    }
    */

    private IEnumerator HideUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (userInterface is { })
            userInterface.SetActive(false);
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
        SetButtonsInteractable(true);
        ClearSequence();
    }
}
