using UnityEngine;
using System.Collections;

public class ViveThrowInput : MonoBehaviour {
    [SerializeField]
    private Transform viveController;
    private Vector3 lastPosition = Vector3.zero;

    public float DistanceSpeed { get; set; }

    void Start()
    {
        DistanceSpeed = 0;
    }
 
    void FixedUpdate()
    {
        DistanceSpeed = (((viveController.transform.position - lastPosition).magnitude) / Time.deltaTime);
        lastPosition = viveController.transform.position;
    }
}
