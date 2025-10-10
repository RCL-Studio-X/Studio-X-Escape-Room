using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideHandOnGrab : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor interactor;
    public GameObject handModel;

    private void OnEnable()
    {
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnGrab);
        interactor.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        handModel.SetActive(false);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        handModel.SetActive(true);
    }
}
