using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetBehaviour : MonoBehaviour {

    public int Score = 10;

    private AudioSource myAudioSource;
    private List<GameObject> hitObjects = new List<GameObject>();
	
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
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
}
