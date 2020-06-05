using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class GravityDef : MonoBehaviour
{
    public GravityComponents components;
    public GravityProperties properties;
    public GravityObjectives objective;

    [Header("Layer of items to check")]
    [Tooltip("El blackhole solo detectara objetos asociados a este Layer.")]
    public LayerMask layers;

    // Start is called before the first frame update
    void Start()
    {
        if (components.rigidbody2D == null)
        {
            components.rigidbody2D = GameObject.Find(this.name).GetComponent<Rigidbody2D>();
        }
        if (components.circleCollider2D == null)
        {
            components.circleCollider2D = GameObject.Find(this.name).GetComponent<CircleCollider2D>();
        }
        if (components.transform == null)
        {
            components.transform = GameObject.Find(this.name).GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkGravityDirection();
    }

    private void FixedUpdate()
    {
        checkWhoToAttract();
    }


    private void checkWhoToAttract()
    {
        if (objective.checkOne) attractOne();
        if (objective.checkSelectedObjects) attractSelectedObjects();
        if (objective.checkAll) attractAll();
        if (objective.checkInRange) attractInrange();
    }

    private void attractAll()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D haveRigidbody2D = GameObject.Find(collider.name).GetComponent<Rigidbody2D>();
            bool isntMyself = collider != components.circleCollider2D;
            if (haveRigidbody2D && isntMyself)
            {
                float initgravityScale = haveRigidbody2D.gravityScale;//Guarda la gravedad "fuera del blackhole"
                haveRigidbody2D.gravityScale = 0;                     //lo deja sin gravedad
                Attract(haveRigidbody2D);                             //lo atrae según la gravedad del blackhole
                haveRigidbody2D.gravityScale = initgravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }

    }

    private void attractOne()
    {
        Rigidbody2D existsPlayer = GameObject.Find(objective.objectName).GetComponent<Rigidbody2D>();
        if (existsPlayer)
        {
            float initGravityScale = existsPlayer.gravityScale;//Guarda la gravedad "fuera del blackhole"
            existsPlayer.gravityScale = 0;                     //lo deja sin gravedad
            Attract(existsPlayer);                             //lo atrae según la gravedad del blackhole
            existsPlayer.gravityScale = initGravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
        }
    }

    private void attractSelectedObjects()
    {
        foreach (Rigidbody2D rigidbody in objective.selectedObjects)
        {
            float initgravityScale = rigidbody.gravityScale;//Guarda la gravedad "fuera del blackhole"
            rigidbody.gravityScale = 0;                     //lo deja sin gravedad
            Attract(rigidbody);                             //lo atrae según la gravedad del blackhole
            rigidbody.gravityScale = initgravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
        }
    }

    private void attractInrange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(components.transform.position, objective.attractionRange, layers);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D haveRigidbody2D = GameObject.Find(collider.name).GetComponent<Rigidbody2D>();
            bool isntMyself = collider != components.circleCollider2D;
            if (haveRigidbody2D && isntMyself)
            {
                float initgravityScale = haveRigidbody2D.gravityScale;//Guarda la gravedad "fuera del blackhole"
                haveRigidbody2D.gravityScale = 0;                     //lo deja sin gravedad
                Attract(haveRigidbody2D);                             //lo atrae según la gravedad del blackhole
                haveRigidbody2D.gravityScale = initgravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }
    }



    void Attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = components.rigidbody2D.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = properties.gravity * (properties.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    private void checkGravityDirection()
    {
        if (properties.invertGravity)
        {
            if (properties.gravity >= 0)
            {
                properties.gravity = -1 * Mathf.Abs(properties.gravity);
            }
        }
        else
        {
            if (properties.gravity < 0)
            {
                properties.gravity = Mathf.Abs(properties.gravity);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (components.transform == null)
            return;
        if (objective.checkInRange)
        {
            Gizmos.DrawWireSphere(components.transform.position, objective.attractionRange);
            float fixedSize = components.transform.localScale.x * properties.fixHaloSize;
            components.haloTransform.localScale = new Vector3(fixedSize, fixedSize, 0);
        }
        else
        {
            float fixedSize = 0.5f;
            components.haloTransform.localScale = new Vector3(fixedSize, fixedSize, 0);
        }
        
    }


}




[System.Serializable]
public class GravityComponents
{
    //[Header("Blackhole Components")]
    [Tooltip("Componente Rigidbody2D del Blackhole.")]
    public Rigidbody2D rigidbody2D;
    [Tooltip("Componente CircleCollider2D del Blackhole.")]
    public CircleCollider2D circleCollider2D;
    [Tooltip("Componente Transform del Blackhole.")]
    public Transform transform;
    [Tooltip("Componente Transform del halo asociado al Blackhole.")]
    public Transform haloTransform;
}



[System.Serializable]
public class GravityProperties
{
    //[Header("Blackhole Properties")]
    [Tooltip("Indica si el blackhole atrae o repele los cuerpos. (False indica que atrae.)")]
    public bool invertGravity = false;
    [Range(-1000, 1000)]
    [Tooltip("Indica la fuerza de gravedad del blackhole.")]
    public float gravity = 0.5f;//gravedad del bh
    [Range(0, 5000)]
    [Tooltip("Indica la masa del blackhole.")]
    public float mass = 5.0f;//masa del bh
    [Range(0, 34)]
    [Tooltip("Permite ajustar el tamaño del halo para. (Modifica el relleno del halo) (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que esté en concordancia con 'eventHorizon')")]
    public float fixHaloSize = 1;//halo
    
}


[System.Serializable]
public class GravityObjectives
{
    [Header("Check All Objects")]
    [Tooltip("True indica que el blackhole detectará y atraera todo objeto que caiga en su rango. (Para caer en el rango del blackhole el objeto debe poseer los componentes de Rigidbody2D y Collider2D)")]
    public bool checkAll = false;//para chequear a todos los elementos


    [Header("Check One Object")]
    [Tooltip("True quiere decir que solo detectara a un objeto especifico.")]
    public bool checkOne = true;//para checkear solo al objeto
    [Tooltip("Escriba aquí el nombre del objecto específico a detectar.")]
    public string objectName = "no specified";//nombre del objeto


    [Header("Check Selected Objects")]
    [Tooltip("True indica que el blackhole detectará una lista específica de objetos.")]
    public bool checkSelectedObjects = false;//para chequear una lista de objetos
    [Tooltip("Lista de objetos específicos a detectar. (Objetos del tipo GameObject)")]
    public Rigidbody2D[] selectedObjects;//lista de objetos a chequear


    [Header("Check Objects In Range")]
    [Tooltip("True indica que el blackhole detectará una lista específica de objetos.")]
    public bool checkInRange = false;//para chequear una lista de objetos
    [Range(0, 20)]
    [Tooltip("Indica el radio del halo. (Modifica el borde del halo) (Este es el rango real de deteccion de elementos) (Variaciones en la gravedad tambien cambian el halo)")]
    public float attractionRange = 0.5f;//horizonte de eventos del bh
}