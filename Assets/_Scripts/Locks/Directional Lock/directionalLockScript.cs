using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class directionalLockScript : MonoBehaviour
{
    public Button upButton, downButton, leftButton, rightButton;
    public Button clearButton, enterButton;
    public directionalLockIndicator[] directionalLockIndicators;
    public AudioSource audioSource;
    public bool locked = true;
    public string targetSequence;
    private int targetLength;

    public GameObject userInterface;
    
    public List<char> curSequence;
    
    private SkinnedMeshRenderer skinnedMeshRenderer;

    // Awake is called with the script is initialized, so also just once like start but before Start() is called.
    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        curSequence = new List<char>();
        targetLength = targetSequence.Length;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Adding AddSequence as the function to run for the Listeners
        upButton.onClick.AddListener(() => AddSequence('u'));
        downButton.onClick.AddListener(() => AddSequence('d'));
        leftButton.onClick.AddListener(() => AddSequence('l'));
        rightButton.onClick.AddListener(() => AddSequence('r'));
        clearButton.onClick.AddListener(() => ClearSequence());
        enterButton.onClick.AddListener(() => EnterDirectionalSequence());
    }

    private void AddSequence(char dir)
    {
        if (curSequence.Count >= targetLength)
            return;

        curSequence.Add(dir);
        ChangeIndicatorToColor(curSequence[curSequence.Count - 1], "blue");
    }

    public void EnterDirectionalSequence()
    {
        if (curSequence.Count != targetLength)
            return;

        if (string.Join("", curSequence) == targetSequence)
        {
            audioSource.Play();
            locked = false;
            StartCoroutine(UnlockBlendshape());
            ChangeAllIndicatorsColor("green");
            // todo: coroutine to set the active state of the userInterface to false after a specified number of seconds (param in coroutine function)
            
        } else {
            // todo: coroutine to flash the indicator lights from white to red
            // coroutine function should have param for num of seconds it flashes for and the flashing interval
            // temporarily disable the buttons clickability during this coroutine
        }
    }

    private void ClearSequence()
    {
        ChangeAllIndicatorsColor("white");
        curSequence.Clear();
    }

    private void ChangeIndicatorToColor(int index, string color)
    {
        if (!directionalLockIndicators[index])
            return;
        
        directionalLockIndicators[index].ChangeIndicatorImage(color);
    }

    private void ChangeAllIndicatorsColor(string color)
    {
        for (int i = 0; i < targetLength; i++)
            ChangeIndicatorToColor(curSequence[i], color); 
    }

    IEnumerator UnlockBlendshape() {
        for (float s = 0f; s < 100f; s++) {
            if (skinnedMeshRenderer)
                skinnedMeshRenderer.SetBlendShapeWeight (0, s);
            else
                Debug.Log("No skinned mesh renderer found on the directional lock GameObject.");
            yield return null;
        }
    }
}
