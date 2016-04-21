using UnityEngine;
using System.Collections;
using ManusMachina;

public class HandMotion : MonoBehaviour {

    public HandSimulator handsim;

    public delegate void ThrowActivate();
    public static event ThrowActivate OnThrowActivate;

    public delegate void ThrowReload();
    public static event ThrowReload OnThrowReload;

    private bool isThrown;
    private bool isReloaded;

    // Use this for initialization
    void Awake ()
    {
        handsim = GetComponent<HandSimulator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (handsim.glove.Fingers[1] > 0.8 && handsim.glove.Fingers[2] > 0.8 && !isThrown)
        {
            if (OnThrowActivate != null)
            {
                OnThrowActivate();
                isThrown = true;
            }
        }
        else if (handsim.glove.Fingers[1] < 0.2 && handsim.glove.Fingers[2] < 0.2 && isReloaded)
        {
            if (OnThrowActivate != null)
            {
                OnThrowActivate();
                isReloaded = true;
            }
        }
        else
        {
            isThrown = false;
            isReloaded = false;
        }
    }
}
