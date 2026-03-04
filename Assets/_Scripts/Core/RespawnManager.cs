using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RespawnManager : MonoBehaviour
{
    [Header("If null, will FindObjectOfType at runtime")]
    [SerializeField] private XRInteractionManager interactionManager;

    [Tooltip("If true, respawn all SymbolChunk in scene. If false, respawn only those listed below.")]
    [SerializeField] private bool respawnAllInScene = true;

    [SerializeField] private List<SymbolChunk> explicitChunks = new List<SymbolChunk>();

    public void RespawnEverything()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        if (interactionManager == null)
            interactionManager = FindObjectOfType<XRInteractionManager>();

        var chunks = GatherChunks();

        // 1) Force deselect anything currently selected (socket, hand, etc.)
        foreach (var chunk in chunks)
        {
            if (chunk == null) continue;

            var interactable = chunk.GetComponent<XRBaseInteractable>();
            if (interactable == null || interactionManager == null) continue;

            // Copy list because it mutates during SelectExit.
            var selecting = new List<IXRSelectInteractor>(interactable.interactorsSelecting);

            foreach (var interactor in selecting)
            {
                if (interactor != null)
                    interactionManager.SelectExit(interactor, interactable);
            }
        }

        // 2) Disable grab interactables so they don't instantly re-socket
        foreach (var chunk in chunks)
        {
            if (chunk == null) continue;
            var grab = chunk.GetComponent<XRGrabInteractable>();
            if (grab != null) grab.enabled = false;
        }

        // Wait 1 frame for XRI state to settle
        yield return null;

        // 3) Teleport/reset physics
        foreach (var chunk in chunks)
        {
            if (chunk == null) continue;

            // Ensure it can move again
            var rb = chunk.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            chunk.RespawnToCachedPose();
        }

        // Wait another frame (optional but helps avoid re-grab edge cases)
        yield return null;

        // 4) Re-enable grabbing
        foreach (var chunk in chunks)
        {
            if (chunk == null) continue;
            var grab = chunk.GetComponent<XRGrabInteractable>();
            if (grab != null) grab.enabled = true;
        }
    }

    private List<SymbolChunk> GatherChunks()
    {
        if (respawnAllInScene)
        {
            // True includes inactive objects; remove true if you don't want that.
            var found = FindObjectsOfType<SymbolChunk>(true);
            return new List<SymbolChunk>(found);
        }
        else
        {
            // Clean nulls
            explicitChunks.RemoveAll(c => c == null);
            return new List<SymbolChunk>(explicitChunks);
        }
    }
}