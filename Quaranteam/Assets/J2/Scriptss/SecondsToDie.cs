using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondsToDie : MonoBehaviour
{
    public bool activateDeath = true;
    public float secondsToDie = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (activateDeath)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        //Wait 1 second
        yield return StartCoroutine(Wait(secondsToDie));
        //Do process stuff
        Destroy(this.gameObject);
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
