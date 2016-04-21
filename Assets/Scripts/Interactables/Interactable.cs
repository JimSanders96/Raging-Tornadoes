using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource), typeof(Collider), typeof(Animator))]
public class Interactable : MonoBehaviour
{

    [SerializeField]
    protected AudioClip interactSound;

    protected AudioSource audioSource;
    protected Collider myCollider;

    protected delegate void ColissionEvent();
    protected ColissionEvent onColission;
    protected Animator animator;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myCollider = (Collider)GetComponent(typeof(Collider));
        animator = GetComponent<Animator>();
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
        audioSource.PlayOneShot(interactSound);
    }       

}
