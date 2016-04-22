using UnityEngine;
using System.Collections;

public class Interactable_Firefly : Interactable {

    [SerializeField]
    private MeshRenderer hideRendererOnDeath;

    [SerializeField]
    private float destroyAfterSeconds = 5f;

    [SerializeField]
    private float movementSpeed = 1f;

    [SerializeField]
    private float flyZone = 5f;

    private Vector3 spawnPoint;
    private float maxX;
    private float maxY;
    private float maxZ;

	// Use this for initialization
	void Start () {
        base.onColission = Die;
        spawnPoint = transform.position;

        // Determine max XYZ values for any new point the fly will fly towards.
        maxX = spawnPoint.x + flyZone;
        maxY = spawnPoint.y + flyZone;
        maxZ = spawnPoint.z + flyZone;

        FlyAround();
	}
	
    /// <summary>
    /// Move to a random point within the fly-zone.
    /// </summary>
    private void FlyAround()
    {
        float pointX = spawnPoint.x + Random.Range(-maxX, maxX);
        float pointY = spawnPoint.y + Random.Range(-maxY, maxY);
        float pointZ = spawnPoint.z + Random.Range(-maxZ, maxZ);
        Vector3 newPoint = new Vector3(pointX, pointY, pointZ);
        StartCoroutine(MoveToPoint(newPoint));
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        float speed = movementSpeed * Random.Range(.7f,1.3f);

        while (Vector3.Distance(transform.position, point) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            yield return null;
        }

        // Fly towards a new random point within the fly-zone.
        Invoke("FlyAround",0);
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
