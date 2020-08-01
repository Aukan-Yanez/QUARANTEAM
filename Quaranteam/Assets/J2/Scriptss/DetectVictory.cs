using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectVictory : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("GameManager").GetComponent<GameManagerJ2>().Win();
        }
    }
}
