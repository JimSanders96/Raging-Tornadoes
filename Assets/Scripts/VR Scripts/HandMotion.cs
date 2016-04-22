using UnityEngine;
using System.Collections;
using ManusMachina;
using System.Timers;

public class HandMotion : MonoBehaviour
{

    public HandSimulator handsim;

    public delegate void ThrowAction();
    public delegate void WristAction();

    public static event ThrowAction OnThrowActivate;
    public static event ThrowAction OnThrowReload;
    public static event WristAction OnHandUp;
    public static event WristAction OnHandReturn;

    [SerializeField]
    [Range(1, 500)]
    private int jitterCheckMilliseconds = 100;

    [SerializeField]
    [Range(.7f, .95f)]
    private float reloadFingerCurveThreshold = 0.8f;
    [SerializeField]
    [Range(.01f, .1f)]
    private float throwFingerCurveThreshold = 0.05f;

    [SerializeField]
    [Range(45f, 90f)]
    private float blockWristXAxisThreshold = 0.8f;

    private bool isCheckingJitter = false;
    private bool isThrown = false;
    private bool isReloaded = false;
    private bool isHandUp = false;

    private Timer jitterCheckTimer;


    // Use this for initialization
    void Awake()
    {
        handsim = GetComponent<HandSimulator>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if (CheckJitter(ref handsim.glove.Fingers[1], reloadFingerCurveThreshold, true))
            {
                if (OnThrowReload != null)
                {
                    OnThrowReload();
                    isReloaded = true;
                }
            }          
        }

        // Throw
        else if (handsim.glove.Fingers[1] < throwFingerCurveThreshold && handsim.glove.Fingers[2] < throwFingerCurveThreshold && !isThrown)
        {
            // This if statement will hold the script in a while loop for jitterTimeMilliseconds.
            if (CheckJitter(ref handsim.glove.Fingers[1], throwFingerCurveThreshold, false))
            {
                if (OnThrowActivate != null)
                {
                    OnThrowActivate();
                    isThrown = true;
                }
            }            
        }
        else
        {
            isThrown = false;
            isReloaded = false;
        }
    }

    #region Jitter checking
    /// <summary>
    /// This function will hold the thread for jitterTimeMilliseconds in a while loop.
    /// After the while loop has finished, the current input value will be compared against the threshhold.
    /// If goOverThreshold is true, the input value has to be equal to or greater than the threshold to return true.
    /// </summary>
    /// <param name="liveInputValue"></param>
    /// <param name="threshold"></param>
    /// <param name="goOverThreshold"></param>
    /// <returns></returns>
    private bool CheckJitter(ref float liveInputValue, float threshold, bool goOverThreshold)
    {
        float inputEnd;
        int i = 0;

        while (i < jitterCheckMilliseconds) i++;

        inputEnd = liveInputValue;

        // liveInputValue has still crossed the threshold after x time
        if (goOverThreshold)
        {
            if (inputEnd >= threshold)
            {
                return true;
            }
        }
        else
        {
            if (inputEnd <= threshold)
            {
                return true;
            }
        }       

        return false;        
    }

    #endregion

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
