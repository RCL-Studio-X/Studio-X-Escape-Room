using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class directionalLockScript : MonoBehaviour
{
    public Button upButton, downButton, leftButton, rightButton;
    public AudioSource audio;
    public bool locked;
    public string targetSequence;
    private int targetLength;

    public Text seqText;
    
    public List<char> curSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curSequence = new List<char>();
        targetLength = targetSequence.Length;
        locked = true;

        upButton.onClick.AddListener(() => AddSeqeunce('u'));
        downButton.onClick.AddListener(() => AddSeqeunce('d'));
        leftButton.onClick.AddListener(() => AddSeqeunce('l'));
        rightButton.onClick.AddListener(() => AddSeqeunce('r'));
    }

    void AddSeqeunce(char dir)
    {
        curSequence.Add(dir);
        Debug.Log(dir);

        seqText.text = string.Join("-", curSequence);

        if (curSequence.Count == targetLength)
        {
            if (string.Join("", curSequence) == targetSequence)
            {
                audio.Play();
                locked = false;
            }
            else 
            {
                curSequence.Clear();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}