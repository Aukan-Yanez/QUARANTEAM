using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class LineargravityDef : MonoBehaviour
{
    public LineargravityComponents components;
    public LineargravityProperties properties;
    public LineargravityObjectives objectives;

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
        checkGravityDirection();
    }

    private void FixedUpdate()
    {
        if (objectives.checkAll) attractAll();
        if (objectives.checkOne) attractOne();
        if (objectives.checkSelectedObjects) attractSelectedObjects();
        if (objectives.checkInRange) attractInRange();
    }

    private void attractAll()
    {
        Rigidbody2D[] rigidbodies = GameObject.FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            bool isntMyself = rigidbody != components.rigidbody2D;
            if (isntMyself)
            {
                float initgravityScale = rigidbody.gravityScale;//Guarda la gravedad "fuera del blackhole"
                rigidbody.gravityScale = 0;                     //lo deja sin gravedad
                Attract(rigidbody);                             //lo atrae según la gravedad del blackhole
                rigidbody.gravityScale = initgravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }
    }

    private void attractOne()
    {
        Rigidbody2D existsPlayer = GameObject.Find(objectives.objectName).GetComponent<Rigidbody2D>();
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
        foreach (Rigidbody2D rigidbody in objectives.selectedObjects)
        {
            float initgravityScale = rigidbody.gravityScale;//Guarda la gravedad "fuera del blackhole"
            rigidbody.gravityScale = 0;                     //lo deja sin gravedad
            Attract(rigidbody);                             //lo atrae según la gravedad del blackhole
            rigidbody.gravityScale = initgravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
        }
    }

    private void attractInRange()
    {
        //float boxSizeX = components.transform.localScale.x + objectives.attractionRangeX;
        //float boxSizeY = components.transform.localScale.y + objectives.attractionRangeY;
        //Collider2D[] range = Physics2D.OverlapBoxAll(components.transform.position, new Vector2(boxSizeX, boxSizeY), 0);
        Collider2D[] top = Physics2D.OverlapCircleAll(components.top.position, properties.topRadius, components.layer);
        Collider2D[] bot = Physics2D.OverlapCircleAll(components.bot.position, properties.botRadius, components.layer);
        

        //Para cada Collider2D
        foreach (Collider2D collider in top)
        {
            Rigidbody2D haveRigidbody2D = GameObject.Find(collider.name).GetComponent<Rigidbody2D>();
            bool isntMyself = collider != components.boxCollider2D;
            SupremeProjectile isSupProjectile = collider.gameObject.GetComponent<SupremeProjectile>();
            if (haveRigidbody2D && isntMyself)
            {
                if (isSupProjectile)
                {
                    isSupProjectile.timeInGravityField = 10;
                    Debug.Log("Is sp");
                }
                float initGravityScale = haveRigidbody2D.gravityScale;//Guarda la gravedad "fuera del blackhole"
                haveRigidbody2D.gravityScale = 0;                     //lo deja sin gravedad
                Attract(haveRigidbody2D);                             //lo atrae según la gravedad del blackhole
                haveRigidbody2D.gravityScale = initGravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }
        foreach (Collider2D collider in bot)
        {
            Rigidbody2D haveRigidbody2D = GameObject.Find(collider.name).GetComponent<Rigidbody2D>();
            bool isntMyself = collider != components.boxCollider2D;
            SupremeProjectile isSupProjectile = collider.gameObject.GetComponent<SupremeProjectile>();
            if (haveRigidbody2D && isntMyself)
            {
                if (isSupProjectile)
                {
                    isSupProjectile.timeInGravityField = 10;
                    Debug.Log("Is sp");
                }
                float initGravityScale = haveRigidbody2D.gravityScale;//Guarda la gravedad "fuera del blackhole"
                haveRigidbody2D.gravityScale = 0;                     //lo deja sin gravedad
                Attract(haveRigidbody2D);                             //lo atrae según la gravedad del blackhole
                haveRigidbody2D.gravityScale = initGravityScale;      //le devuelve la gravedad inicial, en caso de ser más fuerte (la gravedad "fuera del blackhole"), el objeto saldrá del blackhole
            }
        }

    }


    private void Attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = components.rigidbody2D.position - rbToAttract.position;
        //direction = fixedDistance(direction);

        float distance = direction.magnitude;

        float forceMagnitude = properties.gravity * (properties.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(ApplyInAxis(force));
    }

    private Vector3 ApplyInAxis(Vector3 force)
    {
        
        Vector3 direction;
        if(components.top.position.y > components.bot.position.y)
        {
            direction = (components.top.position - components.bot.position).normalized;
        }
        else
        {
            direction = (components.bot.position - components.top.position).normalized;
        }

        return direction * force.magnitude;
        /*
        if (!properties.xAxis)
        {
            retorno.x = 0;
        }
        if (!properties.yAxis)
        {
            retorno.y = 0;
        }
        if (!properties.zAxis)
        {
            retorno.z = 0;
        }
        
        return retorno;*/
    }

    private Vector3 fixedDistance(Vector3 distance)
    {
        Vector3 retorno = distance;
        if (!properties.xAxis)
        {
            retorno.x = 0;
        }
        if (!properties.yAxis)
        {
            retorno.y = 0;
        }
        if (!properties.zAxis)
        {
            retorno.z = 0;
        }
        return retorno;
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


    private void OnDrawGizmos()
    {
        /*
        if (components.transform && components.haloTransform)
        {
            float boxSizeX = components.transform.localScale.x + objectives.attractionRangeX;
            float boxSizeY = components.transform.localScale.y + objectives.attractionRangeY;
            Gizmos.DrawWireCube(components.transform.position, new Vector3(boxSizeX, boxSizeY, 0));
            float fixedSizeX = 1 * properties.fixHaloXAxis;
            float fixedSizeY = 1 * properties.fixHaloYAxis;
            components.haloTransform.localScale = new Vector3(fixedSizeX, fixedSizeY, 0);
            
        }*/

        if(components.top && components.bot)
        {
            Gizmos.DrawWireSphere(components.top.position, properties.topRadius);
            Gizmos.DrawWireSphere(components.bot.position, properties.botRadius);
        }
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
    public Transform top;
    public Transform bot;
    public LayerMask layer = default;
}


[System.Serializable]
public class LineargravityProperties
{
    //[Header("LinearGravity Properties")]
    [Tooltip("Indica si el blackhole atrae o repele los cuerpos. (False indica que atrae.)")]
    public bool invertGravity = false;
    [Range(0, 1000)]
    [Tooltip("Gravedad del objeto.")]
    public float gravity = 9.8f;
    [Range(0, 1000)]
    [Tooltip("Masa del objeto.")]
    public float mass = 1f;
    [Tooltip("Indica si en el eje 'X' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'X')")]
    public bool xAxis = false;
    [Tooltip("Indica si en el eje 'Y' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'Y')")]
    public bool yAxis = true;
    [Tooltip("Indica si en el eje 'Z' se aplica o no gravedad. (True quiere decir que hay gravedad en el eje 'Z')")]
    public bool zAxis = false;

    [Space]
    [Range(0, 10)]
    [Tooltip("Indica la proporcion del halo con respecto al objeto en el eje 'X'. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public float fixHaloXAxis = 20;
    [Range(0, 100)]
    [Tooltip("Indica la proporcion del halo con respecto al objeto en el eje 'Y'. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public float fixHaloYAxis = 20;
    [Range(0, 10)]
    public float topRadius=1;
    [Range(0, 10)]
    public float botRadius = 1;
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
    public Rigidbody2D[] selectedObjects;

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