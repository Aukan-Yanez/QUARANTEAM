using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desactivator : MonoBehaviour
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
                    collision.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            collision.gameObject.SetActive(false);
        }
    }
}
