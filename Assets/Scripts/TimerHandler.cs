using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerHandler : MonoBehaviour {

    public bool TimeUp { get; private set; }
    public float MaxTime;
    private float CurrentTime;

    /// <summary>
    /// TextField for showing Timer
    /// </summary>
    public Text TimerText;
	
    void Start ()
    {
        StartTimer();
    }

	// Update is called once per frame
	void Update ()
    {
        if (TimeUp == false)
        {
            RunTimer();
            TimerText.text = Mathf.Round(CurrentTime).ToString();
        }
	}

    void StartTimer()
    {
        TimeUp = false;
        CurrentTime = MaxTime + 3;
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
