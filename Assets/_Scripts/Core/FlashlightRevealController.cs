using UnityEngine;

/// <summary>
/// Controls the flashlight-based reveal system by updating global shader
/// properties used for stencil masking and reveal calculations.
/// </summary>
[DisallowMultipleComponent]
public class FlashlightRevealController : MonoBehaviour
{
    /// <summary>
    /// Reference to the Spot Light used as the reveal source.
    /// If not assigned, the component attempts to find a Spot Light
    /// on this GameObject or its children.
    /// </summary>
    [Header("Spot Light Reference (optional)")]
    [Tooltip("If not assigned, the script will try to find a Spot Light on this object or its children.")]
    [SerializeField] private Light _spotLight;

    /// <summary>
    /// Shader global property name for the flashlight world-space position.
    /// </summary>
    [SerializeField] private string _posName = "_FlashlightPosWS";

    /// <summary>
    /// Shader global property name for the flashlight forward direction.
    /// </summary>
    [SerializeField] private string _dirName = "_FlashlightDirWS";

    /// <summary>
    /// Shader global property name for the cosine of half the spotlight angle.
    /// </summary>
    [SerializeField] private string _cosHalfAngleName = "_FlashlightCosHalfAngle";

    /// <summary>
    /// Shader global property name for the flashlight range.
    /// </summary>
    [SerializeField] private string _rangeName = "_FlashlightRange";

    /// <summary>
    /// Determines whether a debug ray is drawn in the Scene view
    /// to visualize the flashlight direction.
    /// </summary>
    [Header("Debug")]
    [SerializeField] private bool _drawDebugRay = true;

    /// <summary>
    /// Length of the debug ray drawn to represent flashlight direction.
    /// </summary>
    [SerializeField] private float _debugRayLength = 2.0f;

    /// <summary>
    /// Cached shader property ID for the flashlight position.
    /// </summary>
    private int _posID;

    /// <summary>
    /// Cached shader property ID for the flashlight direction.
    /// </summary>
    private int _dirID;

    /// <summary>
    /// Cached shader property ID for the flashlight cosine half-angle.
    /// </summary>
    private int _cosID;

    /// <summary>
    /// Cached shader property ID for the flashlight range.
    /// </summary>
    private int _rangeID;

    /// <summary>
    /// Initializes shader property IDs and ensures a valid Spot Light reference.
    /// </summary>
    private void Awake()
    {
        _posID = Shader.PropertyToID(_posName);
        _dirID = Shader.PropertyToID(_dirName);
        _cosID = Shader.PropertyToID(_cosHalfAngleName);
        _rangeID = Shader.PropertyToID(_rangeName);

        EnsureSpotLight();
    }

    /// <summary>
    /// Updates cached shader property IDs when values are modified in the Inspector.
    /// Ensures the Spot Light reference is valid when not in Play Mode.
    /// </summary>
    private void OnValidate()
    {
        _posID = Shader.PropertyToID(_posName);
        _dirID = Shader.PropertyToID(_dirName);
        _cosID = Shader.PropertyToID(_cosHalfAngleName);
        _rangeID = Shader.PropertyToID(_rangeName);

        if (!Application.isPlaying)
        {
            EnsureSpotLight();
        }
    }

    /// <summary>
    /// Attempts to locate a Spot Light component on this GameObject
    /// or within its children if one is not already assigned.
    /// </summary>
    private void EnsureSpotLight()
    {
        if (_spotLight == null)
        {
            _spotLight = GetComponent<Light>();

            if (_spotLight == null)
            {
                _spotLight = GetComponentInChildren<Light>(true);
            }
        }
    }

    /// <summary>
    /// Updates global shader parameters each frame to match the current
    /// flashlight position, direction, angle, and range.
    /// Also optionally draws a debug ray for visualization.
    /// </summary>
    private void LateUpdate()
    {
        if (_spotLight == null)
        {
            EnsureSpotLight();
            if (_spotLight == null) return;
        }

        if (_spotLight.type != LightType.Spot)
        {
            return;
        }

        Vector3 pos = _spotLight.transform.position;
        Vector3 fwd = _spotLight.transform.forward.normalized;

        float halfAngleRad = 0.5f * _spotLight.spotAngle * Mathf.Deg2Rad;
        float cosHalf = Mathf.Cos(halfAngleRad);

        Shader.SetGlobalVector(_posID, new Vector4(pos.x, pos.y, pos.z, 1f));
        Shader.SetGlobalVector(_dirID, new Vector4(fwd.x, fwd.y, fwd.z, 0f));
        Shader.SetGlobalFloat(_cosID, cosHalf);
        Shader.SetGlobalFloat(_rangeID, _spotLight.range);

        if (_drawDebugRay)
        {
            Debug.DrawRay(pos, fwd * _debugRayLength, Color.yellow);
        }
    }
}
