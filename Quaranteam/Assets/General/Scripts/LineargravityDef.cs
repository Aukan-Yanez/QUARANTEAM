using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class LineargravityDef : MonoBehaviour
{
    public LineargravityComponents components;

    // Start is called before the first frame update
    void Start()
    {
        if (components.rigidbody2D == null)
        {
            components.rigidbody2D = GameObject.Find(this.name).GetComponent<Rigidbody2D>();
        }
        if (components.boxCollider2D == null)
        {
            components.boxCollider2D = GameObject.Find(this.name).GetComponent<BoxCollider2D>();
        }
        if (components.transform == null)
        {
            components.transform = GameObject.Find(this.name).GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}





[System.Serializable]
public class LineargravityComponents
{
    //[Header("LinearGravity Components")]
    [Tooltip("Componente Rigidbody2D del objeto con gravedad linear.")]
    public Rigidbody2D rigidbody2D;
    [Tooltip("Componente BoxCollider2D del objeto con gravedad linear.")]
    public BoxCollider2D boxCollider2D;
    [Tooltip("Componente Transform del objeto con gravedad linear.")]
    public Transform transform;
    [Tooltip("Componente Transform del halo del objeto con gravedad linear.")]
    public Transform haloTransform;
}


[System.Serializable]
public class LineargravityProperties
{
    [Header("LinearGravity Properties")]
    [Range(-200, 200)]
    [Tooltip("Gravedad del objeto.")]
    public float gravity = 9.8f;
    [Range(-200, 200)]
    [Tooltip("Masa del objeto.")]
    public float mass = 1f;
    [Tooltip("Indica si en el eje 'X' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'X')")]
    public bool xAxis = false;
    [Tooltip("Indica si en el eje 'Y' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'Y')")]
    public bool yAxis = true;
    //[Tooltip("Indica si en el eje 'Z' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'Z')")]
    //public bool zAxis = false;

    [Space]
    [Range(-500, 500)]
    [Tooltip("Indica la proporcion del halo con respecto al objeto en el eje 'X'. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public int fixHaloXAxis = 20;
    [Range(-9000, 9000)]
    [Tooltip("Indica la proporcion del halo con respecto al objeto en el eje 'Y'. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public int fixHaloYAxis = 20;
}


[System.Serializable]
public class LineargravityObjectives
{
    [Header("Check All Objects")]
    [Tooltip("Indica si la gravedad lineal detecta o no a todos los objetos. (True indica que detecta todos los objetos)")]
    public bool checkAll = false;

    [Header("Check One Object")]
    [Tooltip("Indica si se va a detectar o no a un objeto en específico para aplicar gravedad. (True quiere decir que detecta un objeto en específico)")]
    public bool checkOne = true;
    [Tooltip("Nombre del objeto a detectar")]
    public string objectName = "no specified";

    [Header("Check Selected Objects")]
    [Tooltip("Indica si se va a detectar o no una lista de objetos en específico para aplicar gravedad. (True quiere decir que detecta una lista de objetos en específico)")]
    public bool checkSelectedObjects = false;
    [Tooltip("Lista de objetos a detectar. (Objetos del tipo GameObject)")]
    public GameObject[] selectedObjects;

    [Header("Check Objects In Range")]
    [Tooltip("Indica si detecta o no objetos dentro de un rango especificado para aplicar gravedad. (True indica que detecta objetos dentro de un rango)")]
    public bool checkInRange = false;
    [Range(-50, 50)]
    [Tooltip("Rango de detección de los objetos en el eje 'X'. (Este es el rango real de deteccion de elementos en el eje 'x')")]
    public float attractionRangeX = 0f;
    [Range(-50, 50)]
    [Tooltip("Rango de detección de los objetos en el eje 'Y'. (Este es el rango real de deteccion de elementos en el eje 'Y')")]
    public float attractionRangeY = 0f;
}