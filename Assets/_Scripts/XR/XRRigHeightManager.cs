using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

namespace StudioXRCL.EscapeRoom.XR
{
    /// <summary>
    /// Manages XR rig height for standing and sitting modes.
    /// </summary>
    public class XRRigHeightManager : MonoBehaviour
    {
        public enum RigHeightMode
        {
            Standing,
            Sitting
        }

        public enum StartupMode
        {
            None,       // keep whatever local position is already in the scene
            Standing,
            Sitting
        }

        #region Private Variable Declarations

        [Header("Rig Reference")]
        [Tooltip("The transform to move vertically. Usually Camera Offset / tracking origin parent.")]
        [SerializeField] private Transform rigRoot;

        [Header("Startup")]
        [SerializeField] private StartupMode startupMode = StartupMode.Standing;

        [Header("Height Offsets (meters)")]
        [Tooltip("Extra Y offset applied in sitting mode so a seated player is raised to approximate standing height.")]
        [SerializeField] private float sittingYOffset = 0.45f;

        private XROrigin xrOrigin;
        private Vector3 initialLocalPosition;
        private float initialOriginLocalY;
        private float baseCameraYOffset;
        private float currentYOffset;
        private bool baselineInitialized;

        #endregion

        #region Public Properties

        public RigHeightMode CurrentMode { get; private set; } = RigHeightMode.Standing;
        public float CurrentYOffset => currentYOffset;

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Caches XR Origin references and validates the rig root.
        /// </summary>
        private void Awake()
        {
            xrOrigin = GetComponent<XROrigin>();

            if (xrOrigin == null)
            {
                xrOrigin = GetComponentInParent<XROrigin>();
            }

            if (xrOrigin == null && rigRoot != null)
            {
                xrOrigin = rigRoot.GetComponentInParent<XROrigin>();
            }

            if (xrOrigin != null)
            {
                if (rigRoot == null && xrOrigin.CameraFloorOffsetObject != null)
                {
                    rigRoot = xrOrigin.CameraFloorOffsetObject.transform;
                }
            }

            if (rigRoot == null)
            {
                Debug.LogError($"{nameof(XRRigHeightManager)} requires rigRoot to be assigned.", this);
                enabled = false;
                return;
            }
        }

        /// <summary>
        /// Starts the delayed baseline initialization routine.
        /// </summary>
        private void Start()
        {
            StartCoroutine(InitializeBaselineAfterXRReady());
        }

        /// <summary>
        /// Registers the pre-render height correction callback.
        /// </summary>
        private void OnEnable()
        {
            Application.onBeforeRender += HandleBeforeRender;
        }

        /// <summary>
        /// Unregisters the pre-render height correction callback.
        /// </summary>
        private void OnDisable()
        {
            Application.onBeforeRender -= HandleBeforeRender;
        }

        /// <summary>
        /// Reapplies the current height after other systems update the rig.
        /// </summary>
        private void LateUpdate()
        {
            if (!enabled)
            {
                return;
            }

            if (!baselineInitialized)
            {
                return;
            }

            ApplyAbsoluteHeight(currentYOffset);
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Waits for XR initialization, then caches the baseline rig height.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator InitializeBaselineAfterXRReady()
        {
            yield return null;
            yield return new WaitForEndOfFrame();

            initialLocalPosition = rigRoot.localPosition;
            if (xrOrigin != null)
            {
                initialOriginLocalY = xrOrigin.Origin.transform.localPosition.y;
                baseCameraYOffset = xrOrigin.CameraYOffset;
            }

            baselineInitialized = true;

            switch (startupMode)
            {
                case StartupMode.Standing:
                    SetStandingMode();
                    break;

                case StartupMode.Sitting:
                    SetSittingMode();
                    break;

                case StartupMode.None:
                    currentYOffset = GetCurrentAppliedYOffset();
                    CurrentMode = RigHeightMode.Standing;
                    ApplyAbsoluteHeight(currentYOffset);
                    break;
            }
        }

        #endregion

        #region Public Method Definitions

        /// <summary>
        /// Reapplies the current height just before rendering.
        /// </summary>
        private void HandleBeforeRender()
        {
            if (!enabled)
            {
                return;
            }

            if (!baselineInitialized)
            {
                return;
            }

            ApplyAbsoluteHeight(currentYOffset);
        }

        /// <summary>
        /// Default mode for users physically standing in real life.
        /// </summary>
        public void SetStandingMode()
        {
            CurrentMode = RigHeightMode.Standing;
            ApplyYOffset(0f);
        }

        /// <summary>
        /// Mode for users physically sitting in real life.
        /// Adds vertical offset so they feel closer to standing height in VR.
        /// </summary>
        public void SetSittingMode()
        {
            CurrentMode = RigHeightMode.Sitting;
            ApplyYOffset(sittingYOffset);
        }

        /// <summary>
        /// Resets to the startup height baseline for the active rig setup.
        /// </summary>
        public void ResetToInitialHeight()
        {
            CurrentMode = RigHeightMode.Standing;
            currentYOffset = 0f;
            ApplyAbsoluteHeight(0f);
        }

        /// <summary>
        /// Sets the sitting-mode offset value. Reapplies immediately if already in sitting mode.
        /// </summary>
        public void SetSittingOffsetValue(float value)
        {
            sittingYOffset = value;

            if (CurrentMode == RigHeightMode.Sitting)
            {
                ApplyYOffset(sittingYOffset);
            }
        }

        #endregion

        #region Private Method Definitions

        /// <summary>
        /// Stores and applies the requested height offset.
        /// </summary>
        /// <param name="yOffset">Height offset in meters.</param>
        private void ApplyYOffset(float yOffset)
        {
            currentYOffset = yOffset;
            ApplyAbsoluteHeight(currentYOffset);
        }

        /// <summary>
        /// Gets the current height offset relative to the cached baseline.
        /// </summary>
        /// <returns>Current height offset in meters.</returns>
        private float GetCurrentAppliedYOffset()
        {
            if (!baselineInitialized)
            {
                return 0f;
            }

            if (xrOrigin != null)
            {
                if (UsesFloorTracking())
                {
                    return xrOrigin.Origin.transform.localPosition.y - initialOriginLocalY;
                }

                return xrOrigin.CameraYOffset - baseCameraYOffset;
            }

            return rigRoot.localPosition.y - initialLocalPosition.y;
        }

        /// <summary>
        /// Applies a height offset using the active XR rig setup.
        /// </summary>
        /// <param name="yOffset">Height offset in meters.</param>
        private void ApplyAbsoluteHeight(float yOffset)
        {
            if (!baselineInitialized)
            {
                return;
            }

            if (xrOrigin != null)
            {
                if (UsesFloorTracking())
                {
                    Vector3 originLocalPosition = xrOrigin.Origin.transform.localPosition;
                    originLocalPosition.y = initialOriginLocalY + yOffset;
                    xrOrigin.Origin.transform.localPosition = originLocalPosition;
                }
                else
                {
                    Vector3 originLocalPosition = xrOrigin.Origin.transform.localPosition;
                    originLocalPosition.y = initialOriginLocalY;
                    xrOrigin.Origin.transform.localPosition = originLocalPosition;
                    xrOrigin.CameraYOffset = baseCameraYOffset + yOffset;
                }

                return;
            }

            Vector3 newLocalPosition = initialLocalPosition;
            newLocalPosition.y += yOffset;
            rigRoot.localPosition = newLocalPosition;
        }

        /// <summary>
        /// Returns whether floor tracking is active or requested.
        /// </summary>
        /// <returns>True when floor tracking is in use.</returns>
        private bool UsesFloorTracking()
        {
            return xrOrigin != null &&
                (xrOrigin.CurrentTrackingOriginMode == TrackingOriginModeFlags.Floor ||
                 xrOrigin.RequestedTrackingOriginMode == XROrigin.TrackingOriginMode.Floor);
        }

        #endregion
    }
}
