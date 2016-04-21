using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class InputManager : MonoBehaviour
{
    // TODO: Get a reference to the Manus Finger data (custom class)

    // TODO: Get a reference to the HTC-Vive data (custom class?) 

    public float movementSpeed = 5f;

    [SerializeField]
    bool useKeyboardAndMouse = true;

    [SerializeField]
    private GameObject[] throwablePrefabs;

    [SerializeField]
    private Transform throwableSpawnLocation;

    [SerializeField]
    private Vector3 throwDirection = new Vector3(0, 0, 1); // Forward

    [SerializeField]
    private float throwForce = 1000;

    private Throwable throwableInHand;

    private CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (useKeyboardAndMouse)
        {
            CheckMouseButtonPressed();
            CheckKeyboardInput();
        }        

        // For now: Don't change the transform values. Wait for Manus and vive input.
        UpdateThrowableSpawnTransform(throwableSpawnLocation.position, throwableSpawnLocation.rotation);
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
            ThrowObject(throwDirection, throwForce);
        }
        // When RMB is pressed, spawn a throwable.
        if (Input.GetMouseButtonDown(1))
        {
            SpawnNewThrowable();
        }
    }

    /// <summary>
    /// Move depending on which key was pressed (WASD)
    /// </summary>
    private void CheckKeyboardInput()
    {
        if(Input.GetKey(KeyCode.W))
        {
            cc.SimpleMove(transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cc.SimpleMove(-transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cc.SimpleMove(-transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cc.SimpleMove(transform.right * movementSpeed);
        }
    }

    #endregion
}
