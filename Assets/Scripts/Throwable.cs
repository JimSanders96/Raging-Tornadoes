using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(AudioSource))]
public class Throwable : MonoBehaviour
{

    [SerializeField]
    private bool isSharp = true;

    [SerializeField]
    private AudioClip[] throwSounds;

    private Rigidbody myRigidbody;
    private Collider myCollider;

    private AudioSource audioSource;

    private NoobHelper noobHelper;

    void Start()
    {
        // Disable collider to prevent colliding with your own hand.
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.useGravity = false;

        audioSource = GetComponent<AudioSource>();

        // NoobHelper is not a required component.
        try
        {
            noobHelper = GetComponent<NoobHelper>();
        }
        catch
        {

        }
    }

    /// <summary>
    /// If this throwable is marked as being sharp, make it stick to the object it collided with.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if (isSharp)
            StickToObject(col.transform);
    }

    /// <summary>
    /// Launch the object in the given direction with the given speed.
    /// </summary>
    /// <param name="force"></param>
    /// <param name="speed"></param>
    public void Throw(Vector3 direction, float speed)
    {
        // Activate aim assist
        if (noobHelper != null)
            noobHelper.RayCast();

        Vector3 force = direction * speed;
        myRigidbody.AddForce(force);
        myRigidbody.useGravity = true;
        StartCoroutine("EnableCollider");

        // Play a sound
        if (throwSounds.Length > 0)
            PlayThrowSound();
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(.5f);
        myCollider.enabled = true;
    }

    /// <summary>
    /// Add all rigidbody constraints to myRigidbody and make this.transform child of the target.
    /// </summary>
    /// <param name="target"></param>
    private void StickToObject(Transform target)
    {
        transform.SetParent(target);
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void PlayThrowSound()
    {
        audioSource.PlayOneShot(GetRandomThrowSound());
    }

    private AudioClip GetRandomThrowSound()
    {
        AudioClip sound = null;
        if (throwSounds.Length > 0)
        {
            sound = throwSounds[Random.Range(0, throwSounds.Length)];

        }

        return sound;
    }
}
