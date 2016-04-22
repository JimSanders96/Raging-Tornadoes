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

    [Range(5, 50)]
    public int Score = 10;
    public bool IsMoving = false;
    public moveAxis MoveAxis;
    public GameObject ScoreText;

    [Range(1, 5)]
    public float ScoreTime;

    [Range(1, 20)]
    public float MoveDistance = 10;
    [Range(0.1f, 10)]
    public float Speed = 2;

    private int direction = 1;
    private Vector3 intitialLocation;
    private Vector3 targetLocation;

    private AudioSource myAudioSource;
    private List<GameObject> hitObjects = new List<GameObject>();

    private RandomTargetActivation rta;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        intitialLocation = transform.position;

        rta = GameObject.Find("GameManager").GetComponent<RandomTargetActivation>();
    }

    void Update()
    {
        if (IsMoving)
            moveTarget();
    }

    void OnCollisionEnter(Collision objectCollision)
    {
        if (objectCollision.gameObject.GetComponent<Throwable>() != null)
        {
            GameObject collisionObject = objectCollision.gameObject;

            if (!hitObjects.Contains(collisionObject))
            {
                CountScore();
            }

            hitObjects.Add(collisionObject);
        }
    }

    private void CountScore()
    {
        if (myAudioSource != null)
            myAudioSource.Play();

        GameObject scoreTextInstance = (GameObject)Instantiate(ScoreText, transform.position, Quaternion.identity);
        scoreTextInstance.GetComponent<scoreTextBehaviour>().SetText(Score.ToString());
        StartCoroutine(scoreTextInstance.GetComponent<scoreTextBehaviour>().SetDeath(ScoreTime));

        //Adding score to the total score;
        ScoreManager.instance.addScore(Score);

        rta.ActivateNewTarget();
    }

    /// <summary>
    /// Method for moving the target.
    /// </summary>
    private void moveTarget()
    {
        switch ((int)MoveAxis)
        {
            case 0:
                targetLocation = intitialLocation + transform.rotation * new Vector3(MoveDistance * direction, 0, 0);
                break;
            case 1:
                targetLocation = intitialLocation + transform.rotation * new Vector3(0, MoveDistance * direction, 0);
                break;
            case 2:
                targetLocation = intitialLocation + transform.rotation * new Vector3(0, 0, MoveDistance * direction);
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetLocation, ((Vector3.Distance(transform.position, targetLocation)) * Speed + Speed) * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetLocation) < 0.1f)
            direction *= -1;
    }
}
