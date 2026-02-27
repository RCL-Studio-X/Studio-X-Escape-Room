using System.Collections;
using System.Collections.Generic;
using StudioXRCL.EscapeRoom.Audio;
using UnityEngine;

namespace StudioXRCL.EscapeRoom.Core
{
    /// <summary>
    /// Spawns droplets following a dynamic number sequence using Morse-style timing.
    /// Each digit produces five drops (dot/dash pattern), and the sequence loops forever.
    /// </summary>
    public class DropletManager : MonoBehaviour
    {
        #region Public Variable Declarations

        [Header("Droplet Settings")]

        [Tooltip("Prefab of the droplet to spawn.")]
        public GameObject dropletPrefab;

        [Tooltip("Dynamic number sequence. Each digit is converted into a 5-drop Morse pattern.")]
        public List<int> numberSequence = new List<int> { 1, 2, 3 };

        [Tooltip("Base interval between drops for DOT symbols.")]
        public float spawnInterval = 1.0f;

        [Tooltip("Multiplier applied to spawnInterval for DASH symbols.")]
        public float dashMultiplier = 3.0f;

        [Tooltip("World position where droplets will spawn.")]
        public Vector3 spawnPos = new Vector3(-2f, 1f, -1f);

        [Header("Pitch Settings")]

        [Tooltip("Pitch used when the symbol is a DOT.")]
        public float dotPitch = 1.2f;

        [Tooltip("Pitch used when the symbol is a DASH.")]
        public float dashPitch = 0.8f;

        #endregion

        #region Private Variable Declarations

        /// <summary>
        /// Reference to the active coroutine handling the drop loop.
        /// </summary>
        private Coroutine _loopCoroutine;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Starts the droplet spawning coroutine.
        /// </summary>
        private void Start()
        {
            _loopCoroutine = StartCoroutine(Loop());
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Main loop that continuously processes the number sequence
        /// and spawns droplets following Morse-style timing.
        /// </summary>
        /// <returns>IEnumerator for coroutine execution.</returns>
        private IEnumerator Loop()
        {
            while (true)
            {
                if (numberSequence == null || numberSequence.Count == 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                foreach (int digit in numberSequence)
                {
                    int d = Mathf.Abs(digit) % 10;
                    int effective = (d == 0) ? 10 : d;

                    for (int i = 1; i <= 5; i++)
                    {
                        bool dot;

                        if (effective <= 5)
                            dot = (i <= effective);
                        else
                            dot = (i > (effective - 5));

                        DropOne(dot);

                        float waitTime = dot ? spawnInterval : spawnInterval * dashMultiplier;
                        yield return new WaitForSeconds(waitTime);
                    }
                }
            }
        }

        /// <summary>
        /// Instantiates a droplet and assigns its pitch based on whether it represents a dot or dash.
        /// </summary>
        /// <param name="dot">True if the symbol is a dot; false if it is a dash.</param>
        private void DropOne(bool dot)
        {
            if (dropletPrefab == null)
                return;

            GameObject droplet = Instantiate(dropletPrefab, spawnPos, Quaternion.identity);

            DestroyWithSound destroyWithSound = droplet.GetComponent<DestroyWithSound>();
            if (destroyWithSound == null)
                return;

            destroyWithSound.currentPitch = dot ? dotPitch : dashPitch;
        }

        #endregion
    }
}