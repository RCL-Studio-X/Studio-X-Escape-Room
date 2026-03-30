using System.Runtime.CompilerServices;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [Tooltip("GameObjects that should be reset when they touch the collision trigger.")]
    public GameObject gameObjectToRespawn;

    // Store the original transform values
    private Vector3 _originalPositions;
    private Quaternion _originalRotations;
    private Vector3 _originalScales;
    private bool IsInTheZone;
    //private string _tags;

    void Start()
    {
        _originalPositions = new Vector3();
        _originalRotations = new Quaternion();
        _originalScales = new Vector3();
        //_tags = new string[length];

        _originalPositions = gameObjectToRespawn.transform.position;
        _originalRotations = gameObjectToRespawn.transform.rotation;
        _originalScales = gameObjectToRespawn.transform.localScale;
        //_tags[i] = gameObjectsToRespawn[i].tag;

    }

    private void OntriggerEnter(Collider collider)
    {
        // wroking on that
    }

    private void RespawnObject()
    {
        var go = gameObjectToRespawn;

        // Reset transform
        gameObjectToRespawn.transform.SetPositionAndRotation(_originalPositions, _originalRotations);
        gameObjectToRespawn.transform.localScale = _originalScales;

        // Reset velocity if Rigidbody exists
        Rigidbody rb = gameObjectToRespawn.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }



    private void Update()
    {
        if (!IsInTheZone)
        {
            RespawnObject();
        }

    }
}