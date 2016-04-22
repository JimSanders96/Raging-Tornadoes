using UnityEngine;
using System.Collections;
using HighlightingSystem;
using System.Timers;

public class Pickupable : MonoBehaviour
{
    public Material mHighlightMaterial;
    public Material mPickupMaterial;

    public bool constantRumble;
    public Color mFlashColor;

    private Renderer pRenderer = null;
    private Timer timer;

    void Start()
    {
        timer = new Timer();
        timer.AutoReset = false;
        timer.Elapsed += Timer_Elapsed;
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        mHighlighter.FlashingOff();
    }

    private Material mStartMaterial;
    private Highlighter pHighlighter = null;
    private Highlighter mHighlighter
    {
        get
        {
            if (pHighlighter == null)
                pHighlighter = gameObject.GetComponentInChildren<Highlighter>();

            return pHighlighter;
        }
        set
        {
            pHighlighter = value;
        }
    }
    public Color mNearColor;
    public Color mGrabColor;

    // following variables
    FixedJoint mJoint = null;
    GameObject mToFollow = null;
    private bool mIsFollowing = false;
    public bool IsGrabbed
    {
        get
        {
            return mIsFollowing;
        }
    }
    // when this pickupable is "finished", this is set to true (e.g. firefly is in lantarn) so noone can interact anymore
    private bool mLocked;
    public bool IsLocked
    {
        get
        {
            return mLocked;
        }
    }

    Vector3 gFollowPosOffset = new Vector3();
    Quaternion gFollowRotOffset = new Quaternion();


    // Update is called once per frame
    void Update()
    {
        if (mLocked) return;
        // if the object is following but the joint is destroyed, reset all the settings
        if (mIsFollowing && mJoint == null)
        {
            // turn off the tracking
            mIsFollowing = false;
            mToFollow = null;
            // turn off the highlight
            mHighlighter.ConstantOff();
        }
    }

    public void IsNear(GameObject aToFollow)
    {
        if (mLocked) return;
        // if this object is following someone, make sure that the caller is also the owner
        // if not following something execute anyway
        if (!(mIsFollowing ^ (mToFollow == aToFollow)))
        {
            // turn off the tracking
            mIsFollowing = false;
            mToFollow = null;
            // remove fixed joint
            Destroy(mJoint);
            // set the highlight to the "near" color
            mHighlighter.ConstantOn(mNearColor);
        }
    }

    public void Grab(GameObject aToFollow)
    {
        if (mLocked) return;
        if (!mIsFollowing)
        {
            // add a fixed joint to follow the object
            mJoint = gameObject.AddComponent<FixedJoint>();
            mJoint.connectedBody = aToFollow.GetComponent<Rigidbody>();
            mJoint.breakForce = 100000.0f;
            // set the tracking to true
            mIsFollowing = true;
            mToFollow = aToFollow;
            // set the highlight to the defined grabbing color
            mHighlighter.ConstantOn(mGrabColor);
        }
    }

    public void SetAway(GameObject aToFollow)
    {
        if (mLocked) return;
        // if this object is following someone, make sure that the caller is also the owner
        // if not following something execute anyway
        if (!(mIsFollowing ^ (mToFollow == aToFollow)))
        {
            // reset all the settings
            Reset();
        }
    }


    // Reset the object from whatever it is currently tracking, USE WITH CARE
    public void Reset()
    {
        // turn off the tracking
        mIsFollowing = false;
        mToFollow = null;
        // remove fixed joint
        Destroy(mJoint);
        // turn off the highlight
        mHighlighter.ConstantOff();
    }

    public void Lock()
    {
        mLocked = true;
        Reset();
    }

    public void Unlock()
    {
        mLocked = false;
    }

    public void OnJointBreak(float aBreakForce)
    {
        if (timer == null || timer.Enabled)
            return;

        timer.Interval = 3.0f * 1000.0;
        timer.Start();

        mHighlighter.FlashingOn(Color.black, mFlashColor, 1.0f);
        return;
    }

}
