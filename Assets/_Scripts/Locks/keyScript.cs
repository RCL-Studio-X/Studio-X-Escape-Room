using UnityEngine;
using System.Collections;

public class keyScript : MonoBehaviour
{
    public bool selected = false;
    public GameObject lockGameObject;
    public keyLock script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = lockGameObject.GetComponent<keyLock>();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    void OnSelectEntered()
    {
        Debug.Log("Coroutine Started!");
        StartCoroutine(checkUnlock());
    }

    void OnSelectExited()
    {
        Debug.Log("Coroutine finished!");
        StopCoroutine(checkUnlock());
    }

    IEnumerator checkUnlock() {
        while (true)
        {
            if (script.locked && lockGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().hasSelection) {
                script.locked = false;
                lockGameObject.GetComponent<AudioSource>().Play();
            }
            Debug.Log("Alive");
            yield return null;
        }
    }
}
