using UnityEngine;
using System.Collections;
using ManusMachina;

public class HandMotion : MonoBehaviour {

    public HandSimulator handsim;

    public delegate void ThrowActivate();
    public static event ThrowActivate OnThrowActivate;

    public delegate void ThrowReload();
    public static event ThrowReload OnThrowReload;

    private bool isThrown = false;
    private bool isReloaded = false;

    // Use this for initialization
    void Awake ()
    {
        handsim = GetComponent<HandSimulator>();
	}
	
	// Update is called once per frame
	void Update () {        

        // Reload
        if (handsim.glove.Fingers[1] > 0.8 && handsim.glove.Fingers[2] > 0.8 && !isReloaded)
        {
            if (OnThrowReload != null)
            {
                OnThrowReload();
                isReloaded = true;
            }
            
        }

        // Throw
        else if (handsim.glove.Fingers[1] < 0.05 && handsim.glove.Fingers[2] < 0.05 && !isThrown)
        {
            if (OnThrowActivate != null)
            {
                OnThrowActivate();
                isThrown = true;
            }
        }
        else
        {
            isThrown = false;
            isReloaded = false;
        }
    }
}
