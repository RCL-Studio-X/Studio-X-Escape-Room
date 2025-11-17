using UnityEngine;
using System.Collections;

public class LidOpener : MonoBehaviour
{
    [Header("Lid Settings")]
    [Tooltip("Duration in seconds for the lid to fully open.")]
    public float openDuration = 1f;

    Coroutine _openRoutine;

    public void Open()
    {
        if (_openRoutine != null) StopCoroutine(_openRoutine);
        _openRoutine = StartCoroutine(OpenLidRoutine());
    }

    IEnumerator OpenLidRoutine()
    {
        var startRotation = transform.rotation;
        var endRotation = Quaternion.Euler(
            startRotation.eulerAngles.x,
            startRotation.eulerAngles.y,
            -110f
        );

        var elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;

            var t = openDuration > 0f ? elapsed / openDuration : 1f;
            if (t > 1f) t = 1f;

            transform.rotation = Quaternion.Slerp(
                startRotation,
                endRotation,
                t
            );

            yield return null;
        }

        _openRoutine = null;
    }
}
