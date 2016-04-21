using UnityEngine;
using System.Collections;

public class Thrower : MonoBehaviour
{
    [SerializeField]
    private HandMotion handMotion;

    [SerializeField]
    private ViveThrowInput viveThrowInput;

    [SerializeField]
    private GameObject[] throwablePrefabs;

    [SerializeField]
    private Transform throwableSpawnLocation;

    [SerializeField]
    private float throwSpeedThreshold = 10f;

    [SerializeField]
    [Range(50, 1000)]
    private float throwForce = 500;

    private Throwable throwableInHand;
    void Start()
    {
        HandMotion.OnThrowActivate += Throw;
        HandMotion.OnThrowReload += SpawnNewThrowable;
    }

    void OnDestroy()
    {
        HandMotion.OnThrowActivate -= Throw;
        HandMotion.OnThrowReload -= SpawnNewThrowable;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseButtonPressed();
    }

    /// <summary>
    /// If the player is throwing hard enough, throw the throwable.
    /// </summary>
    private void Throw()
    {
        float speed = viveThrowInput.DistanceSpeed;
        if (speed >= throwSpeedThreshold)
            ThrowObject(throwForce * speed);
    }

    #region Throwing stuff

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
    /// If there is a throwable available, throw it forwards relative to the throwableSpawnLocation.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    private void ThrowObject(float force)
    {
        if (throwableInHand != null)
        {
            throwableInHand.Throw(throwableSpawnLocation.transform.forward, force);
            throwableInHand.transform.SetParent(null);
            throwableInHand = null;
        }
    }

    #endregion

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
            ThrowObject(throwForce);
        }
        // When RMB is pressed, spawn a throwable.
        if (Input.GetMouseButtonDown(1))
        {
            SpawnNewThrowable();
        }
    }
    #endregion
}
