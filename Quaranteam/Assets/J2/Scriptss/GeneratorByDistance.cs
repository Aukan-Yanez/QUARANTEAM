using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneratorByDistance : MonoBehaviour
{
    public GameObject[] objectsList;
    public float rotationMin;
    public float rotationMax;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    
    private Vector3 distanceToGenerate;
    
    void SetDistance()
    {
        float axisX = transform.parent.position.x;
        Vector3 myPosition = this.transform.position;
        distanceToGenerate = new Vector3(axisX + Random.Range(minDistance, maxDistance), myPosition.y, myPosition.z);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = GameObject.Find("FollowAxisX").transform;
        SetDistance();
    }

    // Update is called once per frame
    void Update()
    {
        float axisX = transform.parent.position.x;
        print((axisX).ToString());
        if (axisX >= distanceToGenerate.x)
        {
            Instantiate(objectsList[Random.Range(0, objectsList.Length)], transform.position, Quaternion.Euler(0,0,Random.Range(rotationMin,rotationMax)));
            SetDistance();
        }
    }
}
