using UnityEngine;
using System.Collections;
using ManusMachina;

public class HandMotion : MonoBehaviour {

    public HandSimulator handsim;

    public delegate void ThrowActivate();
    public static event ThrowActivate OnThrowActivate;

    // Use this for initialization
    void Awake ()
    {
        handsim = GetComponent<HandSimulator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (handsim.glove.Fingers[1] > 0.8 && handsim.glove.Fingers[2] > 0.8)
            if(OnThrowActivate != null)
                OnThrowActivate();
	}
}
