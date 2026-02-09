using UnityEngine;

[DisallowMultipleComponent]
public class FlashlightRevealController : MonoBehaviour
{
    [Header("Spot Light Reference (optional)")]
    [Tooltip("If not assigned, the script will try to find a Spot Light on this object or its children.")]
    [SerializeField] private Light spotLight;

    [Header("Shader Global Names (must match shaders exactly)")]
    [SerializeField] private string posName = "_FlashlightPosWS";
    [SerializeField] private string dirName = "_FlashlightDirWS";
    [SerializeField] private string cosHalfAngleName = "_FlashlightCosHalfAngle";
    [SerializeField] private string rangeName = "_FlashlightRange";

    [Header("Debug")]
    [SerializeField] private bool drawDebugRay = true;
    [SerializeField] private float debugRayLength = 2.0f;

    private int _posID, _dirID, _cosID, _rangeID;

    private void Awake()
    {
        _posID = Shader.PropertyToID(posName);
        _dirID = Shader.PropertyToID(dirName);
        _cosID = Shader.PropertyToID(cosHalfAngleName);
        _rangeID = Shader.PropertyToID(rangeName);

        EnsureSpotLight();
    }

    private void OnValidate()
    {
        // Keep IDs updated if you edit names in Inspector
        _posID = Shader.PropertyToID(posName);
        _dirID = Shader.PropertyToID(dirName);
        _cosID = Shader.PropertyToID(cosHalfAngleName);
        _rangeID = Shader.PropertyToID(rangeName);

        if (!Application.isPlaying)
            EnsureSpotLight();
    }

    private void EnsureSpotLight()
    {
        if (spotLight == null)
        {
            // Try self first, then children
            spotLight = GetComponent<Light>();
            if (spotLight == null)
                spotLight = GetComponentInChildren<Light>(true);
        }
    }

    private void LateUpdate()
    {
        if (spotLight == null)
        {
            // Try again in case light was added later
            EnsureSpotLight();
            if (spotLight == null) return;
        }

        if (spotLight.type != LightType.Spot)
        {
            // This system assumes a spot light (cone). If you use another type, nothing will match.
            return;
        }

        Vector3 pos = spotLight.transform.position;
        Vector3 fwd = spotLight.transform.forward.normalized;

        // cos(spotAngle/2) in radians
        float halfAngleRad = 0.5f * spotLight.spotAngle * Mathf.Deg2Rad;
        float cosHalf = Mathf.Cos(halfAngleRad);

        Shader.SetGlobalVector(_posID, new Vector4(pos.x, pos.y, pos.z, 1f));
        Shader.SetGlobalVector(_dirID, new Vector4(fwd.x, fwd.y, fwd.z, 0f));
        Shader.SetGlobalFloat(_cosID, cosHalf);
        Shader.SetGlobalFloat(_rangeID, spotLight.range);

        if (drawDebugRay)
        {
            Debug.DrawRay(pos, fwd * debugRayLength, Color.yellow);
        }
    }
}
