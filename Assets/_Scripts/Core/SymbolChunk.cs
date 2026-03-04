// SymbolChunk.cs
using UnityEngine;

[DisallowMultipleComponent]
public class SymbolChunk : MonoBehaviour
{
    [SerializeField] private SymbolType symbol = SymbolType.None;
    public SymbolType Symbol => symbol;

    private Vector3 spawnPos;
    private Quaternion spawnRot;
    private Transform spawnParent;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CacheSpawnPose();
    }

    public void CacheSpawnPose()
    {
        spawnParent = transform.parent;
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }

    public void RespawnToCachedPose()
    {
        Debug.Log("[Chunk] Respawning chunk: " + name);
        if (rb != null)
        {
            bool wasKinematic = rb.isKinematic;

            // Temporarily disable kinematic to safely reset velocity
            rb.isKinematic = false;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.SetPositionAndRotation(spawnPos, spawnRot);

            rb.Sleep();

            // Restore original state
            rb.isKinematic = wasKinematic;
        }
        else
        {
            transform.SetPositionAndRotation(spawnPos, spawnRot);
        }
    }

    // Optional: for design-time quick random assignment.
    public void SetSymbol(SymbolType newSymbol) => symbol = newSymbol;
}