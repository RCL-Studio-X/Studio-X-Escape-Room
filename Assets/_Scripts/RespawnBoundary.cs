using System.Collections.Generic;
using UnityEngine;

public class RespawnBoundary : MonoBehaviour
{
    private Collider boundaryCollider;

    // Objects that exited and need to be watched until they are either
    // back inside or released outside
    private HashSet<RespawnObject> pendingObjects = new HashSet<RespawnObject>();

    private void Awake()
    {
        boundaryCollider = GetComponent<Collider>();

        if (boundaryCollider == null)
        {
            Debug.LogError("[BOUNDARY] RespawnBoundary requires a Collider on the same GameObject.");
        }

        if (!boundaryCollider.isTrigger)
        {
            Debug.LogWarning("[BOUNDARY] Boundary collider should be set to Is Trigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RespawnObject respawn = other.GetComponentInParent<RespawnObject>();

        if (respawn == null)
        {
            Debug.Log("[EXIT] No RespawnObject found on " + other.name);
            return;
        }

        Debug.Log("[EXIT] Trigger exit detected from collider " + other.name + " for object " + respawn.name);

        pendingObjects.Add(respawn);
    }

    private void OnTriggerEnter(Collider other)
    {
        RespawnObject respawn = other.GetComponentInParent<RespawnObject>();

        if (respawn == null)
        {
            return;
        }

        if (pendingObjects.Contains(respawn))
        {
            Debug.Log("[ENTER] " + respawn.name + " came back into the safe zone. Stop watching.");
            pendingObjects.Remove(respawn);
        }
    }

    private void FixedUpdate()
    {
        if (pendingObjects.Count == 0)
        {
            return;
        }

        List<RespawnObject> finishedObjects = new List<RespawnObject>();

        foreach (RespawnObject respawn in pendingObjects)
        {
            if (respawn == null)
            {
                finishedObjects.Add(respawn);
                continue;
            }

            Rigidbody rb = respawn.GetComponent<Rigidbody>();

            bool isKinematic = rb != null && rb.isKinematic;
            bool isInside = IsObjectInsideBoundary(respawn);

            Debug.Log("[CHECK] " + respawn.name +
                      " | isKinematic = " + isKinematic +
                      " | isInside = " + isInside);

            // If it came back inside, stop tracking it
            if (isInside)
            {
                Debug.Log("[CHECK] " + respawn.name + " is inside the safe zone. Stop watching.");
                finishedObjects.Add(respawn);
                continue;
            }

            // If still being held outside, keep waiting
            if (isKinematic)
            {
                Debug.Log("[CHECK] " + respawn.name + " is outside but still being held. Keep watching.");
                continue;
            }

            // Outside and released
            Debug.Log("[CHECK] " + respawn.name + " is outside and released. Respawning.");
            respawn.Respawn();
            finishedObjects.Add(respawn);
        }

        foreach (RespawnObject respawn in finishedObjects)
        {
            pendingObjects.Remove(respawn);
        }
    }

    private bool IsObjectInsideBoundary(RespawnObject respawn)
    {
        Collider[] objectColliders = respawn.GetComponentsInChildren<Collider>();

        foreach (Collider objectCollider in objectColliders)
        {
            if (objectCollider == null || objectCollider.isTrigger)
            {
                continue;
            }

            if (boundaryCollider.bounds.Intersects(objectCollider.bounds))
            {
                Debug.Log("[INSIDE CHECK] " + respawn.name +
                          " is intersecting boundary via collider " + objectCollider.name);
                return true;
            }
        }

        return false;
    }
}