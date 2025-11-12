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

    public GameObject ui;
    
    public List<char> curSequence;
    
    private SkinnedMeshRenderer skinnedMeshRenderer;

    // Awake is called with the script is initialized, so also just once like start but before Start() is called.
    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
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
    }

    private void AddSequence(char dir)
    {
        curSequence.Add(dir);
        
        

        if (curSequence.Count == targetLength)
        {
            if (string.Join("", curSequence) == targetSequence)
            {
                GetComponent<AudioSource>().Play();
                locked = false;
                ui.SetActive(false);
                StartCoroutine(UnlockBlendshape());
            }
            else 
            {
                curSequence.Clear();
            }
        }
    }

    private void ClearSequence()
    {
        
    }

    private void changeIndicatorToColor(int index, string color)
    {
        
    }

    IEnumerator UnlockBlendshape() {
        for (float s = 0f; s < 100f; s++) {
            skinnedMeshRenderer.SetBlendShapeWeight (0, s);
            yield return null;
        }
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
}