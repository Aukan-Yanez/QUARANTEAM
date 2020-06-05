using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearGravity : MonoBehaviour
{
    [TextArea(4, 4)]
    public string ConsiderecionesDeUso = "Los objetos a ser atraidos por la gravedad deben contar con el componente Rigidbody2D. Para ser detectados dentro de un rango deben contar a demás con un componente Collider2D.";

    [Header("LinearGravity Components")]
    [Tooltip("Componente Rigidbody2D del objeto con gravedad linear.")]
    public Rigidbody2D AttractorRigidbody2D;
    [Tooltip("Componente Transform del objeto con gravedad linear.")]
    public Transform AttractorTransform;
    [Tooltip("Componente Transform del halo del objeto con gravedad linear.")]
    public Transform haloTransform;

    [Header("LinearGravity Properties")]
    [Range(-200,200)]
    [Tooltip("Gravedad del objeto.")]
    public float gravity = 9.8f;
    [Range(-200, 200)]
    [Tooltip("Masa del objeto.")]
    public float mass = 1f;
    [Tooltip("Indica si en el eje 'X' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'X')")]
    public bool xAxis = false;
    [Tooltip("Indica si en el eje 'Y' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'Y')")]
    public bool yAxis = true;
    [Tooltip("Indica si en el eje 'Z' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'Z')")]
    public bool zAxis = false;

    [Space]
    [Range(-500, 500)]
    [Tooltip("Indica la proporcion del halo con respecto al objeto en el eje 'X'. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public int proportionXAxis = 20;
    [Range(-9000, 9000)]
    [Tooltip("Indica la proporcion del halo con respecto al objeto en el eje 'Y'. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public int proportionYAxis = 20;

    [Header("Specific Object Properties")]
    [Tooltip("Indica si se va a detectar o no a un objeto en específico para aplicar gravedad. (True quiere decir que detecta un objeto en específico)")]
    public bool checkSpecificObject = true;
    [Tooltip("Nombre del objeto a detectar")]
    public string specificObjectName = "no specified";

    [Header("Specific Objects Properties (List)")]
    [Tooltip("Indica si se va a detectar o no una lista de objetos en específico para aplicar gravedad. (True quiere decir que detecta una lista de objetos en específico)")]
    public bool checkObjectsList = false;
    [Tooltip("Lista de objetos a detectar. (Objetos del tipo GameObject)")]
    public GameObject[] objectList;

    [Header("Specific Range Properties")]
    [Tooltip("Indica si detecta o no objetos dentro de un rango especificado para aplicar gravedad. (True indica que detecta objetos dentro de un rango)")]
    public bool checkInRange = false;
    [Range(-50, 50)]
    [Tooltip("Rango de detección de los objetos en el eje 'X'. (Este es el rango real de deteccion de elementos en el eje 'x')")]
    public float objectDetectionRangeX = 0f;
    [Range(-50, 50)]
    [Tooltip("Rango de detección de los objetos en el eje 'Y'. (Este es el rango real de deteccion de elementos en el eje 'Y')")]
    public float objectDetectionRangeY = 0f;

    [Header("To check all")]
    [Tooltip("Indica si la gravedad lineal detecta o no a todos los objetos. (True indica que detecta todos los objetos)")]
    public bool checkAll = false;

    private float[] initGrav;
    private Rigidbody2D[] outOfrangeList;

    private void Start()
    {
        initGrav = new float[GameObject.FindObjectsOfType<GameObject>().Length];
        outOfrangeList = new Rigidbody2D[GameObject.FindObjectsOfType<GameObject>().Length];
        onBegin();
    }

    private void FixedUpdate()
    {
        applyLinearGravity();
        
    }

    private void applyLinearGravity()
    {
        if (checkSpecificObject) attractSpecific();
        if (checkObjectsList) attractObjectList();
        if (checkInRange) attractInrange();
        if (checkAll) attractAll();
    }


    private void attractSpecific()
    {
        Rigidbody2D objectRigibody = GameObject.Find(specificObjectName).GetComponent<Rigidbody2D>();
        if (objectRigibody!=null)
        {
            
            objectRigibody.gravityScale = 0;
            Attract(objectRigibody);
        }
    }

    private void attractObjectList()
    {
        for (int i=0; i<objectList.Length; i++)
        {
            Rigidbody2D objectRigidbody = objectList[i].GetComponent<Rigidbody2D>();
            if (objectRigidbody != null)
            {
                objectRigidbody.gravityScale = 0;
                Attract(objectRigidbody);
            }
        }
    }





    private void onBegin()
    {
        GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
        for (int i=0; i<outOfrangeList.Length; i++)
        {
            if (all[i].GetComponent<Rigidbody2D>() != null)
            {
                outOfrangeList[i] = (all[i].GetComponent<Rigidbody2D>());
                initGrav[i] = (all[i].GetComponent<Rigidbody2D>().gravityScale);
            }
        }
    }

    private void attractInrange()
    {
        float sizeX = AttractorTransform.localScale.x + objectDetectionRangeX;
        float sizeY = AttractorTransform.localScale.y + objectDetectionRangeY;
        Collider2D[] closeCollider = Physics2D.OverlapBoxAll(AttractorTransform.position, new Vector2(sizeX, sizeY),0);
        GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
        
        //Para cada Collider2D
        foreach (Collider2D collider in closeCollider)
        {
            for (int i=0; i<all.Length; i++)
            {
                if (all[i].GetComponent<Collider2D>() != null)
                {
                    if (collider.Equals(all[i].GetComponent<Collider2D>()))
                    {
                        if (all[i].GetComponent<Rigidbody2D>() != null)
                        {
                            if (!all[i].GetComponent<Rigidbody2D>().Equals(AttractorRigidbody2D))
                            {

                                all[i].GetComponent<Rigidbody2D>().gravityScale = 0;
                                Attract(all[i].GetComponent<Rigidbody2D>());
                            }
                        }
                    }
                    else
                    {
                        if (outOfrangeList[i]!= null)
                        {
                            outOfrangeList[i].gravityScale = initGrav[i];
                        }
                    }
                }
            }
        }
    }

    private void attractAll()
    {
        Rigidbody2D[] all = GameObject.FindObjectsOfType<Rigidbody2D>();
        for (int i=0; i<all.Length; i++)
        {
            if (all[i] != null)
            {
                if (!all[i].Equals(AttractorRigidbody2D))
                {
                    all[i].gravityScale = 0;
                    Attract(all[i]);
                }
            }
        }
    }



    private void Attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = AttractorRigidbody2D.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = gravity * (mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;
        

        rbToAttract.AddForce(ApplyInAxis(force));
    }

    private Vector3 ApplyInAxis(Vector3 force)
    {
        Vector3 retorno = force;
        if (!xAxis)
        {
            retorno.x = 0;
        }
        if (!yAxis)
        {
            retorno.y = 0;
        }
        if (!zAxis)
        {
            retorno.z = 0;
        }
        return retorno;
    }


    

    private void OnDrawGizmosSelected()
    {
        if(AttractorTransform != null)
        {
            Gizmos.color = new Color(2, 2, 2, 0.5f);
            Gizmos.DrawWireCube(AttractorTransform.position, new Vector3(AttractorTransform.localScale.x + objectDetectionRangeX, AttractorTransform.localScale.y + objectDetectionRangeY, 0));

            if (haloTransform != null)
            {
                float propX = AttractorTransform.localScale.x * proportionXAxis / 100;
                float propY = AttractorTransform.localScale.y * proportionYAxis / 100;
                haloTransform.position = AttractorTransform.position;
                haloTransform.localScale = new Vector2(propX + objectDetectionRangeX, propY + objectDetectionRangeY);
            }
        }
    }


}
