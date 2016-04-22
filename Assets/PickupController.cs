using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupController : MonoBehaviour
{
    public Transform gTrackedObject;
    public LayerMask pickupLayers;
    public bool mDisableRendererOnPickup;

    private Pickupable gClosestInRange = null;
    private bool gHasGrabbed = false;
    private List<Pickupable> gObjectsInRange = new List<Pickupable>();
    private HandController gHandController;
    protected bool handClosed;


    void Start()
    {
        gHandController = GetComponent<HandController>();

        HandCollider[] tColliders = GetComponentsInChildren<HandCollider>();
        for (int i = 0; i < tColliders.Length; i++)
        {
            tColliders[i].gTriggerEnter = TriggerEnter;
            tColliders[i].gTriggerExit = TriggerExit;
        }
    }

    private void TriggerEnter(Collider aCollider)
    {
        Pickupable p = aCollider.gameObject.GetComponent<Pickupable>();
        if (p != null && !gObjectsInRange.Contains(p))
        {
            gObjectsInRange.Add(p);
        }
    }

    private void TriggerExit(Collider aCollider)
    {
        Pickupable p = aCollider.gameObject.GetComponent<Pickupable>();
        if (p != null && gObjectsInRange.Contains(p))
        {
            gObjectsInRange.Remove(p);
        }
    }

    void Update()
    {
        // get the closest object
        Pickupable tClosestPickup = GetClosestPickupable();
        bool tHandIsClosed = HandIsClosed();
        bool tJustClosed = tHandIsClosed && !handClosed;
        handClosed = tHandIsClosed;

        if (!gHasGrabbed)
        {
            // if a object is in pickup range but not picked up, call the isnear function
            if (tClosestPickup != null)
            {
                if (tClosestPickup != gClosestInRange)
                {
                    // if nessecary, unhighlight the previous closest
                    if (gClosestInRange != null)
                    {
                        SetAway(gClosestInRange);
                    }
                    // highlight the new closest
                    gClosestInRange = tClosestPickup;
                }
                // to make sure the object stays highlighted even after another glove returns it to normal
                if (gClosestInRange != null)
                {
                    IsNear(gClosestInRange);
                }
            }
            else
            {
                // no device is in close range so remove the previous one if there was one
                if (gClosestInRange != null)
                {
                    SetAway(gClosestInRange);
                    gClosestInRange = null;
                }
            }
            // if the hand is closed, grab a object
            if (tJustClosed)
            {
                if (gClosestInRange != null)
                {
                    Grab(gClosestInRange);
                    gHasGrabbed = true;

                    // Disable renderers
                    if (mDisableRendererOnPickup)
                    {
                        SkinnedMeshRenderer[] meshes = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                        for (int i = 0; i < meshes.Length; i++)
                        {
                            meshes[i].enabled = false;
                        }

                        gHandController.SetVibrationPeriod(0.2f, 0.2f);
                    }
                }
            }
        }
        else
        {
            // "un"-grab a the grabbed object if the object does not exist anymore, the hand is open, the object is locked or if the joint connection is broken
            if (gClosestInRange == null || !tHandIsClosed || gClosestInRange.IsLocked || gClosestInRange.GetComponent<FixedJoint>() == null)
            {
                IsNear(gClosestInRange);
                gHasGrabbed = false;

                // Enable renderers
                if (mDisableRendererOnPickup)
                {
                    SkinnedMeshRenderer[] meshes = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                    for (int i = 0; i < meshes.Length; i++)
                    {
                        meshes[i].enabled = true;
                    }

                    gHandController.SetVibrationPeriod(0.2f, 0.2f);
                }
            }
        }
    }

    public void Grab(Pickupable pickup)
    {
        pickup.Grab(gTrackedObject.gameObject);
    }

    void IsNear(Pickupable pickup)
    {
        pickup.IsNear(gTrackedObject.gameObject);
    }

    void SetAway(Pickupable pickup)
    {
        pickup.SetAway(gTrackedObject.gameObject);
    }

    Pickupable GetClosestPickupable()
    {
        Pickupable tClosest = null;
        float tSqrDist = float.MaxValue;
        for (int i = 0; i < gObjectsInRange.Count; i++)
        {
            // do a couple of checks for the objects in the list
            if (gObjectsInRange[i].IsLocked || gObjectsInRange[i] == null || gObjectsInRange[i].GetComponent<Pickupable>().enabled == false)
            {
                gObjectsInRange.Remove(gObjectsInRange[i]);
                continue;
            }

            float tNewSqrDist = (gObjectsInRange[i].transform.position - transform.position).sqrMagnitude;
            if (tNewSqrDist < tSqrDist)
            {
                tClosest = gObjectsInRange[i];
                tSqrDist = tNewSqrDist;
            }
        }
        return tClosest;
    }

    bool HandIsClosed()
    {
        // get the glove information from the predefined hand controller 
        ManusMachina.Glove tGlove = gHandController.getGlove();

        if (tGlove != null)
        {
            const float tTreshold = 0.5f;
            /* GRABBING */
            // amount of fingers to check for grabbing
            int tFingers = 1;
            // check if at least <tFingers> fingers (NOT THUMB) are beyond a threshold ( "grabbing" gesture ) 
            for (int i = 1; i < 5; i++)
            {
                if (tGlove.Fingers[i] > tTreshold)
                {
                    tFingers--;
                }
            }
            // if the grabbing gesture is true pickup the closest if not null
            if (tFingers <= 0)
            {
                return true;
            }

            /* THUMB + FINGER PINCHING */
            /*bool tFingerBend = false;
            // check if one of the fingers (not thumb) is beyond threshold
            for (int i = 1; i < 5; i++)
            {
                if (tGlove.Fingers[i] > tTreshold - 0.2)
                {
                    tFingerBend = true;
                }
            }
            if (tGlove.Fingers[0] > tTreshold && tFingerBend)
            {
               /return true;
            }*/
        }
        return false;
    }
}
