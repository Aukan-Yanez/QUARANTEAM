using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneradorManager : MonoBehaviour
{
    public GameObject[] generatorsPointsList;
    public int maximumCantOfObjects = 7;
    private Vector3[] lastGeneratodPointObject;
    private int _objectCounter;
    private Vector3 _initialPosition;

    private void GenerateObject()
    {
        int index = Random.Range(0, generatorsPointsList.Length - 1);
        generatorsPointsList[index].gameObject.GetComponent<GenInteractives>().GeneratorObj();
        _objectCounter++;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _objectCounter = 0;
        _initialPosition = transform.position;
        foreach (var generatorPoint in generatorsPointsList)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_objectCounter <= maximumCantOfObjects-2)
        {
            Invoke("GenerateObject", Random.Range(3,8));
        }
        if (_objectCounter <= maximumCantOfObjects)
        {
            GenerateObject();
        }
    }
}
