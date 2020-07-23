using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAxisX : MonoBehaviour
{
    public GameObject followTo;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTo.transform.position.x, transform.position.y, transform.position.z);
    }
}
