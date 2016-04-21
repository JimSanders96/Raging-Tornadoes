using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetBehaviour : MonoBehaviour
{
    public enum moveAxis{
        x,
        y,
        z
    }

    public int Score = 10;
    public bool IsMoving = false;
    public moveAxis MoveAxis;
    public int MoveDistance = 10;
    public float Speed = 2;

    private int direction = 1;
    private Vector3 intitialLocation;
    private Vector3 targetLocation;

    private AudioSource myAudioSource;
    private List<GameObject> hitObjects = new List<GameObject>();

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        intitialLocation = transform.position;
    }

    void Update()
    {
        if (IsMoving)
            moveTarget();
    }

    void OnCollisionEnter(Collision objectCollision)
    {
        GameObject collisionObject = objectCollision.gameObject;

        if (!hitObjects.Contains(collisionObject))
        {
            if (myAudioSource != null)
                myAudioSource.Play();

            //Adding score to the total score;
            ScoreManager.instance.addScore(Score);
        }

        hitObjects.Add(collisionObject);
    }

    /// <summary>
    /// Method for moving the target.
    /// </summary>
    private void moveTarget()
    {
        Debug.Log((int)MoveAxis);
        switch ((int)MoveAxis)
        {
            case 0:
                targetLocation = intitialLocation + new Vector3(MoveDistance * direction, 0, 0);
                break;
            case 1:
                targetLocation = intitialLocation + new Vector3(0, MoveDistance * direction, 0);
                break;
            case 2:
                targetLocation = intitialLocation + new Vector3(0, 0, MoveDistance * direction);
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetLocation, ((Vector3.Distance(transform.position, targetLocation)) / Speed + Speed) * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetLocation) < 0.1f)
            direction *= -1;
    }
}
