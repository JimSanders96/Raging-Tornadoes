using UnityEngine;
using System.Collections;

public class Interactable_Lantern : Interactable
{
    [SerializeField]
    private Rigidbody lanternRigidbody;

    void Start()
    {
        base.onColission += ActivatePhysics;
    }

    private void ActivatePhysics()
    {
        lanternRigidbody.constraints = RigidbodyConstraints.None;
    }
}
