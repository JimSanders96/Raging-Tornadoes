using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    // TODO: Get a reference to the Manus Finger data (custom class)

    // TODO: Get a reference to the HTC-Vive data (custom class?) 

    public float movementSpeed = 5f;

    [SerializeField]
    private GameObject[] throwablePrefabs;

    [SerializeField]
    private Transform throwableSpawnLocation;

    [SerializeField] [Range(500, 20000)]
    private float throwForce = 1000;

    private Throwable throwableInHand;

    // Update is called once per frame
    void Update()
    {
        CheckMouseButtonPressed();  

        // For now: Don't change the transform values. Wait for Manus and vive input.
        //UpdateThrowableSpawnTransform(throwableSpawnLocation.position, throwableSpawnLocation.rotation);
    }   

    /// <summary>
    /// Instantiate a random throwablePrefab at the throwableSpawnLocation and store a reference to its Throwable script.
    /// </summary>
    private void SpawnNewThrowable()
    {
        GameObject prefab = throwablePrefabs[Random.Range(0, throwablePrefabs.Length)];
        if (throwableInHand == null)
        {
            GameObject throwable = (GameObject)Instantiate(prefab, throwableSpawnLocation.position, throwableSpawnLocation.rotation);
            throwableInHand = throwable.GetComponent<Throwable>();
            throwable.transform.SetParent(throwableSpawnLocation);
        }
    }

    /// <summary>
    /// If there is a throwable available, throw it.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    private void ThrowObject(Vector3 direction, float force)
    {
        if (throwableInHand != null)
        {
            throwableInHand.Throw(direction, force);
            throwableInHand.transform.SetParent(null);
            throwableInHand = null;

        }
    }

    /// <summary>
    /// Update the position and rotation of the throwableSpawnLocation Transform
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    private void UpdateThrowableSpawnTransform(Vector3 position, Quaternion rotation)
    {
        throwableSpawnLocation.position = position;
        throwableSpawnLocation.rotation = rotation;
    }

    #region Non-VR input

    /// <summary>
    /// For now, when the LMB is pressed, throw the throwableInHand.
    /// When RMB is pressed, spawn a random throwable.
    /// </summary>
    private void CheckMouseButtonPressed()
    {
        // When LMB is pressed, throw the object.
        if (Input.GetMouseButtonDown(0))
        {
            ThrowObject(throwableSpawnLocation.transform.forward, throwForce);
        }
        // When RMB is pressed, spawn a throwable.
        if (Input.GetMouseButtonDown(1))
        {
            SpawnNewThrowable();
        }
    }
    #endregion
}
