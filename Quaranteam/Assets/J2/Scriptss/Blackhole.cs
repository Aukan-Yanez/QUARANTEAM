using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    private float initG = 0.05f;
    private float prevG = 0.5f;
    private bool decreasedRadiusOnce=false;
    private float prevRadius=0;
    private bool entroAlBlackhole = false;

    //[TextArea(4, 4)]
    //public string ConsiderecionesDeUso = "Los objetos a ser atraidos por la gravedad deben contar con el componente Rigidbody2D. Para ser detectados dentro de un rango deben contar a demás con un componente Collider2D.";

    [Header("Blackhole Components")]
    [Tooltip("Componente Rigidbody2D del Blackhole.")]
    public Rigidbody2D blackholeRigidbody2D;
    [Tooltip("Componente CircleCollider2D del Blackhole.")]
    public CircleCollider2D blackholeCircleCollider2D;
    [Tooltip("Componente Transform del Blackhole.")]
    public Transform blackholeTransform;
    [Tooltip("Componente Transform del halo asociado al Blackhole.")]
    public Transform haloTransform;
    

    [Header("Blackhole Properties")]
    [Tooltip("Indica si el blackhole atrae o repele los cuerpos. (False indica que atrae.)")]
    public bool invertBlackhole = false;
    [Range(-800, 800)]
    [Tooltip("Indica el radio del blackhole.")]
    public float blackholeRadius = 1.8f;//radio del blackhole
    [Range(-800, 800)]
    [Tooltip("Indica la fuerza de gravedad del blackhole.")]
    public float blackholeGravity = 0.5f;//gravedad del bh
    [Range(0, 5000)]
    [Tooltip("Indica la masa del blackhole.")]
    public float blackholeMass = 5.0f;//masa del bh
    [Range(-800, 800)]
    [Tooltip("Indica el radio del halo. (Modifica el borde del halo) (Este es el rango real de deteccion de elementos) (Variaciones en la gravedad tambien cambian el halo)")]
    public float eventHorizon = 0.5f;//horizonte de eventos del bh
    [Range(-500, 500)]
    [Tooltip("Indica la proporcion del halo con respecto al blackhole. (Modifica el relleno del halo) (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'eventHorizon') (Variaciones en la gravedad tambien cambian el halo)")]
    public int proportion = 20;//halo
    [Tooltip("Indica si despues de escapar del rango del blackhole, el objeto recupera su gravedad previa a entrar en el blackhole.")]
    public bool recoverGravity = true;//para devolver la gravedad al player en caso de salir del bh


    [Header("Specific Object Components")]
    [Tooltip("True quiere decir que solo detectara a un objeto especifico.")]
    public bool checkSpecificObject = true;//para checkear solo al objeto
    [Tooltip("Escriba aquí el nombre del objecto específico a detectar.")]
    public string specificObjectName = "no specified";//nombre del objeto
    [Range(0, 100)]
    [Tooltip("Indica la escala de reduccion del radio del componente CircleCollider2D del objeto específico a detectar.")]
    public int radiusDecreaseScale = 20;//radio de la imagen del objeto
    
    

    [Header("Specific Objects Components (List)")]
    [Tooltip("True indica que el blackhole detectará una lista específica de objetos.")]
    public bool checkListOfObjects = false;//para chequear una lista de objetos
    [Tooltip("Lista de objetos específicos a detectar. (Objetos del tipo GameObject)")]
    public GameObject[] objectsList;//lista de objetos a chequear


    [Header("To check all")]
    [Tooltip("True indica que el blackhole detectará y atraera todo objeto que caiga en su rango. (Para caer en el rango del blackhole el objeto debe poseer los componentes de Rigidbody2D y Collider2D)")]
    public bool checkAll = false;//para chequear a todos los elementos


    [Header("Layer of items to check")]
    [Tooltip("El blackhole solo detectara objetos asociados a este Layer.")]
    public LayerMask layers;

    

    private void Start()
    {
        eventHorizon = blackholeGravity;
        prevRadius = blackholeRadius;
        prevG = blackholeGravity;
        initG = blackholeGravity;
    }

    private void Update()
    {
        checkIfInvert();
    }
    void FixedUpdate()
    {
        eventHorizonExceeded();
        ifItEscape();
        
        updateEventHorizon();
        updateRadius();

        checkIfInvert();
    }

    private void eventHorizonExceeded()
    {
        if (checkSpecificObject)
        {
            eventHorizonExceededByPlayer();
        }
        if (checkAll)
        {
            eventHorizonExceededByAll();
        }
        if (checkListOfObjects)
        {
            eventHorizonExceededByList();
        }
    }


    private void eventHorizonExceededByPlayer()
    {
        if (GameObject.Find(specificObjectName) == null)
        {
            Debug.LogWarning("El nombre especificado no se encuentra.");
            return;
        }
        //Se obtienen todos los Collider2D
        Collider2D[] closeCollider = Physics2D.OverlapCircleAll(blackholeTransform.position, eventHorizon, layers);
        CircleCollider2D playerCollider = GameObject.Find(specificObjectName).GetComponent<CircleCollider2D>();
        Rigidbody2D playerRigidbody = GameObject.Find(specificObjectName).GetComponent<Rigidbody2D>();

        //Para cada Collider2D
        foreach (Collider2D collider in closeCollider)
        {
            if (playerCollider != null)
            {
                //Si el Collider es el Collider del player
                if (collider.Equals(playerCollider))
                {
                    if (playerRigidbody != null)
                    {
                        //Se desactiva la gravedad hacia el suelo
                        entroAlBlackhole = true;
                        playerRigidbody.gravityScale = 0;
                        //Decrece el radio del collider del player (esto genera un movimiento más erratico del player)
                        if (decreasedRadiusOnce == false)
                        {
                            playerCollider.radius = playerCollider.radius - ((playerCollider.radius * radiusDecreaseScale) / 100);
                            decreasedRadiusOnce = true;
                        }
                        //Aplica atraccion al player
                        Attract(playerRigidbody);
                    }
                }
            }
        }
    }


    private void eventHorizonExceededByAll()
    {
        //Se obtienen todos los Collider2D
        Collider2D[] closeCollider = Physics2D.OverlapCircleAll(blackholeTransform.position, eventHorizon, layers);
        GameObject[] allGameObject = GameObject.FindObjectsOfType<GameObject>();
        //Para cada Collider2D
        foreach (Collider2D collider in closeCollider)
        {
            for(int i=0; i< allGameObject.Length; i++)
            {
                Rigidbody2D currentRigidbody = allGameObject[i].GetComponent<Rigidbody2D>();
                CircleCollider2D currentCollider = allGameObject[i].GetComponent<CircleCollider2D>();

                if (currentCollider != null)
                {
                    //Si el Collider es el Collider del player
                    if (collider.Equals(currentCollider))
                    {
                        if (currentRigidbody != null)
                        {
                            if (currentRigidbody != blackholeRigidbody2D)
                            {
                                //Se desactiva la gravedad hacia el suelo
                                currentRigidbody.gravityScale = 0;
                                Attract(currentRigidbody);
                            }
                        }                    
                    }
                }
            }
        }
    }


    private void eventHorizonExceededByList()
    {
        //Se obtienen todos los Collider2D
        Collider2D[] closeCollider = Physics2D.OverlapCircleAll(blackholeTransform.position, eventHorizon, layers);
       
        //Para cada Collider2D
        foreach (Collider2D collider in closeCollider)
        {
            for (int i = 0; i < objectsList.Length; i++)
            {
                Rigidbody2D currentRigidbody = objectsList[i].GetComponent<Rigidbody2D>();
                CircleCollider2D currentCollider = objectsList[i].GetComponent<CircleCollider2D>();

                if (currentCollider != null)
                {
                    //Si el Collider es el Collider del player
                    if (collider.Equals(currentCollider))
                    {
                        if (currentRigidbody != null)
                        {
                            if (currentRigidbody != blackholeRigidbody2D)
                            {
                                //Se desactiva la gravedad hacia el suelo
                                currentRigidbody.gravityScale = 0;
                                Attract(currentRigidbody);
                            }
                        }
                    }
                }
            }
        }
    }


    private void ifItEscape()
    {
        if (!recoverGravity) { return; }
        if (GameObject.Find(specificObjectName) == null) { return; }

        bool escape = true;
        Rigidbody2D playerRigidbody = GameObject.Find(specificObjectName).GetComponent<Rigidbody2D>();
        CircleCollider2D playerCollider = GameObject.Find(specificObjectName).GetComponent<CircleCollider2D>();
        
        //Se obtienen todos los Collider2D
        Collider2D[] closeCollider = Physics2D.OverlapCircleAll(blackholeTransform.position, eventHorizon, layers);

        //Para cada Collider2D
        foreach (Collider2D collider in closeCollider)
        {
            //Si el Collider es el Collider del player
            if (collider.Equals(playerCollider))
            {
                //No escapa del blackhole
                escape = false;
            }
        }

        if (escape)
        {
            if (entroAlBlackhole)
            {
                playerRigidbody.gravityScale = initG;
                entroAlBlackhole = false;
            }
        }
    }


    void Attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = blackholeRigidbody2D.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = blackholeGravity * (blackholeMass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }


    private void updateEventHorizon()
    {
        if (blackholeGravity != prevG)
        {
            prevG = blackholeGravity;
            eventHorizon = blackholeGravity;
        }
    }

    private void updateRadius()
    {
        //blackHoleCc.radius = radius;
        float factor = 0.01f;
        if (prevRadius < blackholeRadius)
        {
            prevRadius = blackholeRadius;
            blackholeTransform.localScale = new Vector2(blackholeTransform.localScale.x + factor, blackholeTransform.localScale.y + factor);
            blackholeGravity += factor;
        }
        if (prevRadius > blackholeRadius)
        {
            prevRadius = blackholeRadius;
            blackholeTransform.localScale = new Vector2(blackholeTransform.localScale.x - factor, blackholeTransform.localScale.y - factor);
            blackholeGravity -= factor;
        }
    }

    private void checkIfInvert()
    {
        if (invertBlackhole)
        {
            if (blackholeGravity>0)
            {
                blackholeGravity = -blackholeGravity;
                prevG = -prevG;
            }
        }
        else
        {
            blackholeGravity = Mathf.Sqrt(Mathf.Pow(blackholeGravity, 2));
            prevG = Mathf.Sqrt(Mathf.Pow(prevG, 2));
        }
    }


    


    private void OnDrawGizmosSelected()
    {
        if (blackholeTransform == null)
            return;

        float prop = blackholeTransform.localScale.x * proportion / 100;

        Gizmos.DrawWireSphere(blackholeTransform.position, eventHorizon);
        haloTransform.localScale = new Vector2(eventHorizon + prop, eventHorizon + prop);
    }




}
