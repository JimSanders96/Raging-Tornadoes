using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource), typeof(Collider))]
public class Interactable : MonoBehaviour
{

    [SerializeField]
    protected AudioClip interactSound;

    protected AudioSource audioSource;
    protected Collider myCollider;

    protected delegate void ColissionEvent();
    protected ColissionEvent onColission;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myCollider = (Collider)GetComponent(typeof(Collider));
    }

    /// <summary>
    /// Play a sound.
    /// Execute whatever methods are attached to the onColission delegate;
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        PlayInteractSound();

        if (onColission != null)
            onColission();
    }

    private void PlayInteractSound()
    {
        if (interactSound != null)
            audioSource.PlayOneShot(interactSound);
    }

}
