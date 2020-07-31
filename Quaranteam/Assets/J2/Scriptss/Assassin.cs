using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    public GameObject[] whiteList;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (whiteList.Length != 0)
        {
            foreach (var i in whiteList)
            {
                if (i.gameObject.name != collision.gameObject.name)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
