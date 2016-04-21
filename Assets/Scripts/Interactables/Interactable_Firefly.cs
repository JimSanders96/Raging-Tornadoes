using UnityEngine;
using System.Collections;

public class Interactable_Firefly : Interactable {

    [SerializeField]
    private MeshRenderer hideRendererOnDeath;

    [SerializeField]
    private float destroyAfterSeconds = 5f;

	// Use this for initialization
	void Start () {
        base.onColission = Die;
	}
	
	// Update is called once per frame
	void Update () {
        FlyAround();
	}

    private void FlyAround()
    {
        //TODO Move around in a certain area within its spawn area.
    }

    private void Die()
    {
        StartCoroutine("HideAndDestroy");
    }

    private IEnumerator HideAndDestroy()
    {
        hideRendererOnDeath.enabled = false;

        yield return new WaitForSeconds(destroyAfterSeconds);

        Destroy(gameObject);
    }
}
