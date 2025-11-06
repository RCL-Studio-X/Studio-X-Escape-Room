using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class directionalLockScript : MonoBehaviour
{
    public Button upButton, downButton, leftButton, rightButton;
    public AudioSource audio;
    public bool locked = true;
    public string targetSequence;
    private int targetLength;

    public GameObject ui;
    public Text seqText;
    
    public List<char> curSequence;

    // C# does private by default for access if not specified.
    SkinnedMeshRenderer skinnedMeshRenderer;

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
    }

    void AddSequence(char dir)
    {
        curSequence.Add(dir);

        seqText.text = string.Join("-", curSequence);

        if (curSequence.Count == targetLength)
        {
            if (string.Join("", curSequence) == targetSequence)
            {
                audio.Play();
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