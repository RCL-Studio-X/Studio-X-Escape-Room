using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MazeRotator : MonoBehaviour
{
    public GameObject maze;
    public float rotationSpeed = 0.01f;

    private bool isHoldingL = false;
    public Button buttonLeft;

    private bool isHoldingR = false;
    public Button buttonRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonLeft.onClick.AddListener(OnClickL);
        buttonRight.onClick.AddListener(OnClickR);
    }

    // Update is called once per frame
    void Update()
    {
        //maze.transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime);
    }

    private void OnClickL()
    {
        StopAllCoroutines();
        StartCoroutine(RotateMazeL());
    }

    private void OnClickR()
    {
        StopAllCoroutines();
        StartCoroutine(RotateMazeR());
    }

    IEnumerator RotateMazeR() {
        while (true) {
            maze.transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime);
        yield return null;
        }
    }

    IEnumerator RotateMazeL() {
        while (true) {
            maze.transform.Rotate(new Vector3(0, 0, -20) * Time.deltaTime);
        yield return null;
        }
    }
}
