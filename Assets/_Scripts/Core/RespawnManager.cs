using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Handles respawning all symbol chunks in the scene (or an explicit list) back
    /// to their cached poses and resets interaction state.
    /// </summary>
    public class RespawnManager : MonoBehaviour
    {
        /// <summary>
        /// Interaction manager used to force deselects during respawn.
        /// If null, it will be located via FindObjectOfType at runtime.
        /// </summary>
        [Header("If null, will FindObjectOfType at runtime")]
        [Tooltip("Interaction manager used to force deselects during respawn. If null, it will be located via FindObjectOfType at runtime.")]
        [SerializeField] private XRInteractionManager interactionManager;

        /// <summary>
        /// If true, respawn all SymbolChunk instances in the scene.
        /// Otherwise only respawn the explicitly provided list.
        /// </summary>
        [Tooltip("If true, respawn all SymbolChunk in scene. If false, respawn only those listed below.")]
        [SerializeField] private bool respawnAllInScene = true;

        /// <summary>
        /// Explicit list of SymbolChunks to respawn when <see cref="respawnAllInScene"/> is false.
        /// </summary>
        [Tooltip("Explicit list of SymbolChunks to respawn when 'Respawn All In Scene' is false.")]
        [SerializeField] private List<SymbolChunk> explicitChunks = new List<SymbolChunk>();

        /// <summary>
        /// Initiates the respawn process for all symbol chunks.
        /// </summary>
        public void RespawnEverything()
        {
            StartCoroutine(RespawnRoutine());
        }

        /// <summary>
        /// Coroutine that performs the respawn sequence: deselects interactables, disables grabbing, resets poses, and re-enables grabbing.
        /// </summary>
        private IEnumerator RespawnRoutine()
        {
            if (interactionManager == null)
                interactionManager = FindFirstObjectByType<XRInteractionManager>();

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

        /// <summary>
        /// Gathers the list of SymbolChunks to respawn, either all in the scene or the explicit list.
        /// </summary>
        private List<SymbolChunk> GatherChunks()
        {
            if (respawnAllInScene)
            {
                // remove FindObjectsInactive.Include if you don't want to include inactive objects
                var found = FindObjectsByType<SymbolChunk>(FindObjectsInactive.Include, FindObjectsSortMode.None);
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
}
