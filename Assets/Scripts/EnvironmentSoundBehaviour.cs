using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class EnvironmentSoundBehaviour : MonoBehaviour {

    private AudioSource myAudioSource;
    public AudioClip[] AudioClips;

    [Range(5, 50)]
    public float minWaitTime;
    [Range(5, 50)]
    public float maxWaitTime;

    private float timer;
    private float waitTime;

	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();

        waitTime = Random.Range(minWaitTime, maxWaitTime);
        timer = Random.Range(0, minWaitTime);

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer >= waitTime)
        {
            myAudioSource.PlayOneShot(AudioClips[Random.Range(0, AudioClips.Length)]);
            waitTime = Random.Range(minWaitTime, maxWaitTime);
            timer = 0;
        }
	}
}
