using UnityEngine;
using System.Collections;
using ManusMachina;

public class HandMotion : MonoBehaviour {

    public HandSimulator handsim;

	// Use this for initialization
	void Awake ()
    {
        handsim = GetComponent<HandSimulator>();
	}
	
	// Update is called once per frame
	void Update () {
        float Index = handsim.glove.Fingers[1];
	}
}
