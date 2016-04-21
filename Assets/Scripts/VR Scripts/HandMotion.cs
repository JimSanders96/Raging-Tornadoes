using UnityEngine;
using System.Collections;
using ManusMachina;

public class HandMotion : MonoBehaviour {

    public HandSimulator handsim;

    public delegate void ThrowAction();
    public delegate void WristAction();

    public static event ThrowAction OnThrowActivate;
    public static event ThrowAction OnThrowReload;
    public static event WristAction OnHandUp;
    public static event WristAction OnHandReturn;

    [SerializeField]
    [Range(.7f,.95f)]
    private float reloadFingerCurveThreshold = 0.8f;
    [SerializeField]
    [Range(.01f, .1f)]
    private float throwFingerCurveThreshold = 0.05f;

    [SerializeField]
    [Range(45f,90f)]
    private float blockWristXAxisThreshold = 0.8f;
    
    private bool isThrown = false;
    private bool isReloaded = false;
    private bool isHandUp = false;

    // Use this for initialization
    void Awake ()
    {
        handsim = GetComponent<HandSimulator>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckThrowActions();
        CheckBlockActions();
    }

    /// <summary>
    /// Trigger the ThrowAction events when the finger curve thresholds are crossed.
    /// </summary>
    private void CheckThrowActions()
    {
        // Reload
        if (handsim.glove.Fingers[1] > reloadFingerCurveThreshold && handsim.glove.Fingers[2] > reloadFingerCurveThreshold && !isReloaded)
        {
            if (OnThrowReload != null)
            {
                OnThrowReload();
                isReloaded = true;
            }

        }

        // Throw
        else if (handsim.glove.Fingers[1] < throwFingerCurveThreshold && handsim.glove.Fingers[2] < throwFingerCurveThreshold && !isThrown)
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

    /// <summary>
    /// Trigger the WristAction events when the wrist curve threshold on the X axis has been crossed.
    /// </summary>
    private void CheckBlockActions()
    {
        if (handsim.glove.Quaternion.eulerAngles.x > blockWristXAxisThreshold && !isHandUp)
        {
            if (OnHandUp != null)
            {
                OnHandUp();
                isHandUp = true;
            }
        }
        else if (handsim.glove.Quaternion.eulerAngles.x < blockWristXAxisThreshold && isHandUp)
        {
            if (OnHandReturn != null)
            {
                OnHandReturn();
                isHandUp = false;
            }
        }
        else
        {
            isHandUp = false;
        }
    }
}
