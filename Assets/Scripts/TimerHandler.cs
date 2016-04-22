using UnityEngine;
using System.Collections;

public class TimerHandler : MonoBehaviour {

    public bool TimeUp { get; private set; }
    public float TimerValue { get; set; }
    private float CurrentTime;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (TimeUp == false)
        {
            RunTimer();
        }
	}

    void StartTimer()
    {
        TimeUp = false;
        CurrentTime = TimerValue;
    }

    void RunTimer()
    {
        if (CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
        }
        else
        {
            TimeUp = true;
        }
    }
}
