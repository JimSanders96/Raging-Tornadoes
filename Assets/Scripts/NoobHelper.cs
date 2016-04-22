using UnityEngine;
using System.Collections;

public class NoobHelper : MonoBehaviour
{

    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed;

    void RayCasting()
    {
        Transform camera = Camera.main.transform;
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, 1000))
        {
            if (hit.transform.tag == "Target")
            {
                Target = hit.transform;
            }
        }

    }

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        //find the vector pointing from our position to the target
        _direction = (Target.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
    }
}