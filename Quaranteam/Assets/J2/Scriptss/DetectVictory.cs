using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectVictory : MonoBehaviour
{
    public GameObject player;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == player.gameObject.name)
        {
            _audioSource.Play();
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("GameManager").GetComponent<GameManagerJ2>().Win();
        }
    }
}
