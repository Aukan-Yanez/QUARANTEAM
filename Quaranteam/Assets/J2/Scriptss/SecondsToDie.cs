using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondsToDie : MonoBehaviour
{
    public bool Destroy = true;
    public float secondsToDie = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        //Wait seconds
        yield return StartCoroutine(Wait(secondsToDie));
        //Do process stuff
        if (Destroy)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
