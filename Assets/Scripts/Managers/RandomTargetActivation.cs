﻿using UnityEngine;
using System.Collections;

public class RandomTargetActivation : MonoBehaviour {

    [SerializeField]
    private GameObject[] targetObjects;

    [SerializeField]
    private float disableTargetAfterTime = 3f;

    private GameObject activeTarget;

    void Start()
    {
        InitTargets();
    }

    private void InitTargets()
    {
        if (targetObjects.Length > 1)
        {
            // Disable all targets
            foreach (GameObject target in targetObjects) target.SetActive(false);

            // Enable 1 random target
            ActivateNewTarget();
        }        
    }

    public void ActivateNewTarget()
    {
        activeTarget.SetActive(false);
        activeTarget = targetObjects[Random.Range(0, targetObjects.Length)];
        activeTarget.SetActive(false);
    }

    private IEnumerator DisableAfterTime(GameObject go)
    {
        MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
        renderer.enabled = false;

        yield return new WaitForSeconds(disableTargetAfterTime);

        go.SetActive(false);
        renderer.enabled = true;
    }

}