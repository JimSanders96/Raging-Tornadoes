using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour {

    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.useGravity = false;
    }

    /// <summary>
    /// Launch the object in the given direction with the given speed.
    /// </summary>
    /// <param name="force"></param>
    /// <param name="speed"></param>
    public void Throw(Vector3 direction, float speed)
    {
        Vector3 force = direction * speed;
        myRigidbody.AddForce(force);
        myRigidbody.useGravity = true;
    }
}
