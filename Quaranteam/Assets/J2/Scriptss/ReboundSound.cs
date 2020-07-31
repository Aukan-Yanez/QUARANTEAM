using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        audioSource.Play();
    }
}
