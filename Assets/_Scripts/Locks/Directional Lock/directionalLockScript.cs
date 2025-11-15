using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class directionalLockScript : MonoBehaviour
{
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

    public List<char> curSequence;

    private int _targetLength;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private void Awake()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        curSequence = new List<char>();
        _targetLength = targetSequence.Length;
    }

    private void Start()
    {
        upButton.onClick.AddListener(() => AddSequence('u'));
        downButton.onClick.AddListener(() => AddSequence('d'));
        leftButton.onClick.AddListener(() => AddSequence('l'));
        rightButton.onClick.AddListener(() => AddSequence('r'));
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
            StartCoroutine(UnlockBlendshape());
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

    private IEnumerator UnlockBlendshape()
    {
        for (float s = 0f; s < 100f; s++)
        {
            if (_skinnedMeshRenderer)
                _skinnedMeshRenderer.SetBlendShapeWeight(0, s);

            yield return null;
        }
    }

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
