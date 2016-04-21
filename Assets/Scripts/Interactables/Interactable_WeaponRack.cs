using UnityEngine;
using System.Collections;

public class Interactable_WeaponRack : Interactable {

    [SerializeField]
    private Rigidbody[] weapons;

    [SerializeField]
    private float bumpIntensityMin = 25f;

    [SerializeField]
    private float bumpIntensityMax = 100f;

    private bool hasCollided = false;

	// Use this for initialization
	void Start () {
        base.onColission = ActivatePhysicsOnWeapons;
	}

    private void ActivatePhysicsOnWeapons()
    {
        if (!hasCollided)
        {
            foreach (Rigidbody weapon in weapons)
            {
                weapon.constraints = RigidbodyConstraints.None;
                weapon.AddForce(GetRandomBumpForce());
            }
            hasCollided = true;
        }        
    }

    private Vector3 GetRandomBumpForce()
    {
        float randomX = Random.Range(bumpIntensityMin, bumpIntensityMax);
        float randomY = Random.Range(bumpIntensityMin, bumpIntensityMax);
        float randomZ = Random.Range(bumpIntensityMin, bumpIntensityMax);

        return new Vector3(randomX, randomY, randomZ);
    }
	
}
