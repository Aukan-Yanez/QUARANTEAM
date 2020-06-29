using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(CircleCollider2D))]
public class BlackholeDef : MonoBehaviour
{
    //[TextArea(4, 4)]
    //public string ConsiderecionesDeUso = "Los objetos a ser atraidos por la gravedad deben contar con el componente Rigidbody2D. Para ser detectados dentro de un rango deben contar a demás con un componente Collider2D.";
    
    
    public BlackholeComponents components;
    public BlackholeProperties properties;
    public BlackholeObjectives objective;

    [Header("Layer of items to check")]
    [Tooltip("El blackhole solo detectara objetos asociados a este Layer.")]
    public LayerMask layers;



    private void Start()
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

    


    private void Update()
    {
        checkGravityDirection();
    }
    void FixedUpdate()
    {
        checkWhoToAttract();
    }

    private void checkWhoToAttract()
    {
        if (objective.checkOne) attractOne();
        if (objective.checkSelectedObjects) attractSelectedObjects();
        if (objective.checkAll) attractAll();
    }

    private void attractOne()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(components.transform.position, properties.eventHorizon, layers);
        Rigidbody2D player = GameObject.Find(objective.objectName).GetComponent<Rigidbody2D>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.name== objective.objectName && player!=null)
            {
                float initGravityScale = player.gravityScale;//Guarda la gravedad "fuera del blackhole"
                player.gravityScale = 0;                     //lo deja sin gravedad
                Attract(player);                             //lo atrae según la gravedad del blackhole
                player.gravityScale = initGravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }
    }

    private void attractSelectedObjects()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(components.transform.position, properties.eventHorizon,layers);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D objectExists = isASelectedObject(collider.name);
            if (objectExists)
            {
                float initgravityScale = objectExists.gravityScale;//Guarda la gravedad "fuera del blackhole"
                objectExists.gravityScale = 0;                     //lo deja sin gravedad
                Attract(objectExists);                             //lo atrae según la gravedad del blackhole
                objectExists.gravityScale = initgravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }
    }

    private Rigidbody2D isASelectedObject(string name)
    {
        for (int i = 0; i < objective.selectedObjects.Length; i++)
        {
            if(objective.selectedObjects[i].name == name)
            {
                return objective.selectedObjects[i];
            }
        }
        return null;
    }

    private void attractAll()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(components.transform.position, properties.eventHorizon, layers);

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

        float forceMagnitude = properties.blackholeGravity * (properties.blackholeMass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }



    private void checkGravityDirection()
    {
        if (properties.invertBlackhole)
        {
            if (properties.blackholeGravity >= 0)
            {
                properties.blackholeGravity = -1 * Mathf.Abs(properties.blackholeGravity);
            }
        }
        else
        {
            if (properties.blackholeGravity < 0)
            {
                properties.blackholeGravity = Mathf.Abs(properties.blackholeGravity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (components.transform == null)
            return;
        Gizmos.DrawWireSphere(components.transform.position, properties.eventHorizon);
        float fixedSize = components.transform.localScale.x * properties.fixHaloSize;
        components.haloTransform.localScale = new Vector3(fixedSize, fixedSize, 0);
    }



}


[System.Serializable]
public class BlackholeComponents
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
public class BlackholeProperties
{
    //[Header("Blackhole Properties")]
    [Tooltip("Indica si el blackhole atrae o repele los cuerpos. (False indica que atrae.)")]
    public bool invertBlackhole = false;
    //[Range(-800, 800)]
    //[Tooltip("Indica el radio del blackhole.")]
    //public float blackholeRadius = 1.8f;//radio del blackhole
    [Range(-1000, 1000)]
    [Tooltip("Indica la fuerza de gravedad del blackhole.")]
    public float blackholeGravity = 0.5f;//gravedad del bh
    [Range(0, 5000)]
    [Tooltip("Indica la masa del blackhole.")]
    public float blackholeMass = 5.0f;//masa del bh
    [Range(0, 20)]
    [Tooltip("Indica el radio del halo. (Modifica el borde del halo) (Este es el rango real de deteccion de elementos) (Variaciones en la gravedad tambien cambian el halo)")]
    public float eventHorizon = 0.5f;//horizonte de eventos del bh
    [Range(0, 34)]
    [Tooltip("Permite ajustar el tamaño del halo para. (Modifica el relleno del halo) (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que esté en concordancia con 'eventHorizon')")]
    public float fixHaloSize = 1;//halo
    //[Tooltip("Indica si despues de escapar del rango del blackhole, el objeto recupera su gravedad previa a entrar en el blackhole.")]
    //public bool recoverGravity = true;//para devolver la gravedad al player en caso de salir del bh
    //===============================

    //[Range(-800, 800)]
    //[Tooltip("Indica el radio del blackhole.")]
    //public float blackholeSize = 1f;//radio del blackhole
    
}


[System.Serializable]
public class BlackholeObjectives
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

}