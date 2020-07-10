using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMecanics : MonoBehaviour
{
    public int points = 1;
    public GameObject[] balls;

    private void Start()
    {
        if(balls.Length == 0)
        {
            balls = GameObject.FindGameObjectsWithTag("Player");
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (GameObject obj in balls)
        {
            if (col.gameObject.name == obj.name)
            {
                GameObject.Find("GameManager").GetComponent<GameManagerJ2>().CapturedCoins(points);
                Destroy(gameObject);
            }
        }

    }
}
