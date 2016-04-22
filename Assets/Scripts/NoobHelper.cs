using UnityEngine;
using System.Collections;

public class NoobHelper : MonoBehaviour
{

    //values that will be set in the Inspector
    /// <summary>
    /// Target: the target we throw at
    /// RotationSpeed: how fast the object will rotate to target
    /// DistanceToTarger: The distance between the target and the Camera / Player
    /// </summary>
    private Transform Target;
    public float RotationSpeed;
    private float DistanceToTarget;

    /// <summary>
    /// create a raycast towards the target, and log the distance
    /// </summary>
    public void RayCast()
    {
        Transform camera = Camera.main.transform;
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, 1000))
        {
            if (hit.transform.tag == "Target")
            {
                DistanceToTarget = hit.distance;
                Target = hit.transform;
            }
        }

    }

    /// <summary>
    /// the rotation of the object in relation to the target
    /// and the direction to fly in
    /// </summary>
    ///values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        ///<summary>
        ///check if the location of the thrown object is smaller then the distance between the target and player
        /// </summary>
        ///
        if (Vector3.Distance(this.transform.position, Camera.main.transform.position) < DistanceToTarget)
        {
            //find the vector pointing from our position to the target
            _direction = (Target.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
    }
}
