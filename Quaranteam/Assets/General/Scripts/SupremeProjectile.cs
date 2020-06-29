using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(CircleCollider2D))]
[RequireComponent(typeof(SpringJoint2D), typeof(Animator))]
public class SupremeProjectile : MonoBehaviour
{
    #region Attributes
    #region public
    public ProjectileComponents components;
    public ProjectileProperties properties;
    public LayerMask layers = default;
    [HideInInspector]
    public ProjectileBehavior projectile;
    //[HideInInspector]
    public bool inmortal = false;
    [HideInInspector]
    public GameObject[] dotList;

    #endregion

    #region private
    private bool holdDownMouse = false;
    private bool wasDragged = false;
    private Buscar utils = new Buscar();
    private Vector2 pushingForce;
    [HideInInspector]
    public float timeInGravityField = 5;
    private float initGrav = 0;
    #endregion
    #endregion

    #region Methods
    private void Start()
    {
        //En caso de no asignar los componentes a este script
        setUpComponents();

        setUpBehavior();
        setUpProperties();
    }
    private void Update()
    {

        //int length = utils.getTouchedElements(this.gameObject, "DamagemechanicsDef").Length;//   getElemntsArround(this.gameObject, ((FirePS)projectile).burningRadius, "DamagemechanicsDef").Length;
        //Debug.Log("N° de elementos tipo DamagemechanicsDef: " + length);
        if (holdDownMouse)//Si esta presionando en el projectil:
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            if (!maxDragg(mousePosition))
            {
                components.rigidbody2D.position = mousePosition; //El proyectil sigue el movimiento del mouse.
            }
            else
            {
                components.rigidbody2D.position = fixedDistance(mousePosition);
            }
        }

        projectile.doPersonalizedBehavior(this.gameObject); //cada proyectil puede sobreescribir este metodo "a su modo"  
    }
    private void FixedUpdate()
    {
        checkIfWasDraggedEnough();
        reduceLifeTime();
        animateIfDoesDamage();
        checkOwnGravity();
    }

    private void checkOwnGravity()
    {
        if (timeInGravityField > 0)
        {
            timeInGravityField -= 1;
            components.rigidbody2D.gravityScale = 0;
        }

        if (timeInGravityField == 0)
        {
            components.rigidbody2D.gravityScale = initGrav;
        }
    }


    #region animation
    private void animateIfDoesDamage()
    {
        if (projectile.animationState() != "default")
        {
            if (utils.getTouchedElements("DamagemechanicsDef", this.gameObject).Length > 0)
            {
                components.animator.SetBool(projectile.animationState(), true);
            }
            else
            {
                components.animator.SetBool(projectile.animationState(), false);
            }
        }
    }
    #endregion

    #region set up
    private void setUpComponents()
    {
                                                                                                
        if (components.spriteRenderer == null)                                                  //Asigna el sprite renderer
        {
            components.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();              //Debug.Log("Assigning SpriteRenderer");
                                                                                                
            if (components.spriteRenderer.sprite == null)                                       //Si no tiene imagen asociada, carga una desde Resource/Sprites
            {
                components.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/ABSCircle2");//Debug.Log("     Assigning Sprite to the SpriteRenderer");
                components.spriteRenderer.color = Color.red;                                    //Debug.Log("     Assigning Color.red to the SpriteRenderer");
            }
        }
                                                                                                
        if (components.rigidbody2D == null)                                                     //Asigna el rigid body 2D
        {
            components.rigidbody2D = gameObject.GetComponent<Rigidbody2D>();                    //Debug.Log("Assigning Rigidbody2D");            
            if (components.rigidbody2D.sharedMaterial == null)                                  //Si no tine material asignado, cargar uno desde Resources/material
            {
                components.rigidbody2D.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/ball");
            }
        }
                                                                                                
        if (components.collider2D == null)                                                      //Asigna el collider 2D
        {
            components.collider2D = gameObject.GetComponent<Collider2D>();                      //Debug.Log("Assigning Collider2D");
        }                                                                                       
        
        if (components.elasticCord == null)                                                     //Asigna el Spring joint 2D (la cuerda elastica que une el proyectil con el gancho)
        {
            components.elasticCord = gameObject.GetComponent<SpringJoint2D>();                  //Debug.Log("Connecting cord to projectile");
            if (components.hookRigidbody2D != null)                                             //Si el gancho esta asignado
            {
                if (components.elasticCord.connectedBody == null)                               //Si no tiene conectado el elástico, lo conecta
                {
                    components.elasticCord.connectedBody = components.hookRigidbody2D;          //Debug.Log("Connecting cord to hook");
                }
            }
        }
        

        if (components.animator == null)                                                                    //Asigna el animator
        {
            components.animator = gameObject.GetComponent<Animator>();                                      //Debug.Log("Assigning Animator");
        }

        if (gameObject.GetComponent<TrailRenderer>())
        {
            gameObject.GetComponent<TrailRenderer>().materials[0].color = gameObject.GetComponent<SpriteRenderer>().color;
        }


        if (properties.dot != null)
        {
            dotList = new GameObject[properties.projectionPointsNumber];
            for (int i = 0; i < properties.projectionPointsNumber; i++)
            {
                dotList[i] = Instantiate(properties.dot, components.transform, false);
            }
        }
        

    }
    private void setUpProperties()
    {
        components.rigidbody2D.mass = properties.mass;
        components.rigidbody2D.gravityScale = properties.gravityScale;
        components.elasticCord.frequency = properties.hookForce;
        initGrav = components.rigidbody2D.gravityScale;
    }
    private void setUpBehavior()
    {
        if (properties.projectileType == ProjectileProperties.ProjectileType.Fire)
        {
            projectile = properties.fire;
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Ice)
        {
            projectile = properties.ice;
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Kirby)
        {
            projectile = properties.kirby;
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Spin)
        {
            projectile = properties.spin;
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Bomb)
        {
            projectile = properties.bomb;
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Expand)
        {
            projectile = properties.expand;
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Pegaloco)
        {
            projectile = properties.pegaloco;
        }
    }
    #endregion

    #region projectile launch
    private void checkIfWasDraggedEnough()
    {
        if (!ifItsDisconnected() && !nearTheHook()) //Si a+un está conectado el proyectil y lo arrastro lo suficiente
        {
            showTrajectory();
            wasDragged = true;
        }
    }
    private bool nearTheHook()//Devuelve true si el proyectil está cerca del gancho.
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(components.hookRigidbody2D.position, properties.minRadiusToLaunch, layers);
        foreach (Collider2D collider in colliders)//Por cada collider cerca del hook
        {
            if (collider.Equals(components.collider2D))//Si el collider actual es el collider del proyectil, retorna true
            {
                return true;
            }
        }
        return false;
    }
    private bool maxDragg(Vector3 mousePos)
    {
        Vector2 center = components.hookRigidbody2D.position;
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        float distance = (mousePos2D - center).magnitude;
        if (distance>=properties.maxRadiusToLaunch)
        {
            return true;
        }
        return false;
    }
    private Vector2 fixedDistance(Vector3 mousePosition)
    {
        Vector2 center = components.hookRigidbody2D.position;
        Vector2 mousePos2D = new Vector2(mousePosition.x, mousePosition.y);
        Vector2 direction = (mousePos2D - center).normalized;
        Vector2 posicion = direction * properties.maxRadiusToLaunch;
        return posicion + center;
    }
    #endregion

    #region life and directional line
    private void reduceLifeTime()
    {
        if (ifItsDisconnected() && !inmortal)
        {
            properties.lifeTime--;
            if (properties.lifeTime <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private bool ifItsDisconnected()
    {
        if (components.elasticCord.enabled)
        {
            return false;
        }
        return true;
    }

    private void showTrajectory()
    {
        Vector2 direccion = (components.hookRigidbody2D.position - components.rigidbody2D.position).normalized;
        Vector2 velocidadInicial = direccion * properties.initialVelocityMagnitude;

        Vector2 posicionInicial = components.rigidbody2D.position;
        Vector2 gravedad = Physics2D.gravity;

        if (dotList != null)
        {
            if (dotList.Length > 0)
            {
                for (int i = 0; i < dotList.Length; i++)
                {
                    dotList[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    float currentCurveTime = properties.spaceBetweenPoints * i;
                    Vector2 posicionActual = posicionInicial + velocidadInicial * currentCurveTime + 0.5f * gravedad * Mathf.Pow(currentCurveTime, 2);
                    dotList[i].transform.position = posicionActual;
                }
            }
        }
        
        pushingForce = velocidadInicial;
    }
    private void hideTrajectory()
    {
        for (int i = 0; i < dotList.Length; i++)
        {
            dotList[i].gameObject.GetComponent<SpriteRenderer>().enabled=false;
        }
    }
    private void deleteTrajectory()
    {
        for (int i = 0; i < dotList.Length; i++)
        {
            Destroy(dotList[i]);
        }
    }
    #endregion

    #region mouse detection
    private void OnMouseDown()
    {
        if (components.elasticCord.enabled)//Solo si aún esta conectado al elástico
        {
            holdDownMouse = true;
            components.rigidbody2D.isKinematic = true;
        }
    }
    private void OnMouseUp()
    {
        holdDownMouse = false;
        components.rigidbody2D.isKinematic = false;

        if (wasDragged && !nearTheHook())//Si el proyectil fue arrastrado y no esta cerca del gancho (es decir fue arrastrado lo suficiente)
        {
            components.rigidbody2D.velocity = pushingForce;
            components.elasticCord.enabled = false;
            deleteTrajectory();
            wasDragged = false;
        }
        if (wasDragged && nearTheHook())//Si el proyectil fue arrastrado pero esta cerca del gancho
        {
            hideTrajectory();
            wasDragged = false;
        }
    }
    #endregion
    
    #endregion


    private void OnDrawGizmos()
    {
        if (components.hookRigidbody2D)//Dibuja una circunferencia que indica si el proyectil aún está cerca del gancho (es decir dentro de la circunferencia) o si esta lejos del gancho (por fuera de la circunferencia). (el dibujo es solo visible en la ventana de Scene).
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(components.hookRigidbody2D.position, properties.minRadiusToLaunch);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(components.hookRigidbody2D.position, properties.maxRadiusToLaunch);
        }

        if (properties.projectileType == ProjectileProperties.ProjectileType.Kirby)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(components.rigidbody2D.position, properties.kirby.swallowRadius);
        }

        if (properties.projectileType == ProjectileProperties.ProjectileType.Spin)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(components.rigidbody2D.position, properties.spin.radiusEffect);
        }

        if (properties.projectileType == ProjectileProperties.ProjectileType.Bomb)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(components.rigidbody2D.position, properties.bomb.massChangeRadius);
        }
        if (properties.projectileType == ProjectileProperties.ProjectileType.Expand)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(components.rigidbody2D.position, properties.expand.currentRadius);
        }

    }
}


#region Caracteristicas y componentes del movimiento del proyectil
[System.Serializable]
public class ProjectileComponents
{
    [Header("Proyectil")]
    public Transform transform;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2D;
    public Collider2D collider2D;
    [Header("Hook")]
    public Rigidbody2D hookRigidbody2D;
    public SpringJoint2D elasticCord;
    public LineRenderer lineRenderer;
    [Header("Animation")]
    public Animator animator;
}

[System.Serializable]
public class ProjectileProperties
{
    [Header("Life")]
    //[Tooltip("Permite al proyectil ser reconocido o no por los demas objetos.")]
    //public bool isActive = true;
    [Tooltip("Duración del proyectil luego de cortar el elástico y ser lanzado.")]
    [Range(0, 1000)]
    public int lifeTime = 100;
    [Tooltip("Define el area de detección para cortar el elástico que une al proyectil con el gancho")]
    [Range(0, 4)]
    public float minRadiusToLaunch = 1;
    [Tooltip("Define el estiramiento máximo que el elástico puede alzanzar")]
    [Range(0, 5)]
    public float maxRadiusToLaunch = 2.5f;
    [Tooltip("Modulo de la velocidad inicial (velocidad inicial de lanzamiento)")]
    [Range(0, 100)]
    public float initialVelocityMagnitude = 1f;
    [Tooltip("Cantidad de puntos para dibujar la trayectoria")]
    [Range(0, 30)]
    public int projectionPointsNumber = 3;
    [Tooltip("Espacio entre los puntos")]
    [Range(0, 2)]
    public float spaceBetweenPoints = 0.1f;
    [Tooltip("GameObject de referencia para ser dibujado")]
    public GameObject dot;
    

    [Header("Physics")]
    [Tooltip("Masa del proyectil.")]
    [Range(0, 1000)]
    public float mass = 1;
    [Tooltip("Gravedad del proyectil.")]
    [Range(0, 1000)]
    public float gravityScale = 1;
    [Tooltip("Frecuancia con la que se expande y contrae el elástico. (mientras esta conectado con el proyectil).")]
    [Range(0, 1000)]
    public float hookForce = 1;

    
    [Header("Behavior")]
    [Tooltip("Indica si el proyectil quita la gravedad al objeto que toca.")]
    public bool canRemoveGravity = false;
    public enum ProjectileType {Fire, Ice, Bomb, Spin, Kirby, Expand, Pegaloco}
    public ProjectileType projectileType = ProjectileType.Spin;
    public FirePS fire;
    public IcePS ice;
    public BombPS bomb;
    public SpinPS spin;
    public KirbyPS kirby;
    public ExpandPS expand;
    public Pegaloco pegaloco;
    //public EnProgresoPS enProgreso;

    [Header("Directional line color")]
    [Range(0, 255)]
    public int R = 0;
    [Range(0, 255)]
    public int G = 0;
    [Range(0, 255)]
    public int B = 0;
    [Range(0, 255)]
    public int A = 0;
}
#endregion


#region Comportamiento del proyectil

public abstract class ProjectileBehavior
{
    [Range(0,100)]
    public float damage = 1;

    public abstract string animationState();
    public abstract void doPersonalizedBehavior(GameObject go);
}


[System.Serializable]
public class FirePS : ProjectileBehavior
{
    //[Range(0, 20)]
    //[Tooltip("Indica el rango que alcanza el fuego desde este objeto a su alrededor.")]
    //public float burningRadius = 0f;

    public override string animationState()
    {
        return "Burning";
    }

    public override void doPersonalizedBehavior(GameObject go)
    {
        return;
    }
}

[System.Serializable]
public class IcePS : ProjectileBehavior
{
    [Range(0, 100)]
    [Tooltip("Indica cuanto porcentaje de vida le resta al objeto afectado por el hielo en el primer golpe del proyectil.")]
    public float initialDamagePercentage = 0f;
    [Range(0, 500)]
    [Tooltip("Indica cuanto se demorará en congelarse el objeto afectado por el hielo.")]
    public float freezingTime = 0f;
    public override string animationState()
    {
        return "Freezing";
    }
    public override void doPersonalizedBehavior(GameObject go)
    {
        return;
    }
}

[System.Serializable]
public class BombPS : ProjectileBehavior
{
    [Range(0, 3000)]
    [Tooltip("Rango mínimo de fuerza explosiva.")]
    public float explotionForceMin = 0f;
    [Range(0, 3000)]
    [Tooltip("Rango máximo de fuerza explosiva")]
    public float explotionForceMax = 0f;
    [Range(0,5)]
    [Tooltip("Rango para aumentar la masa")]
    public float massChangeRadius = 0f;
    [Range(0, 500)]
    [Tooltip("Masa a la que aumenta")]
    public float finalMass = 0f;

    public override string animationState()
    {
        return "Exploding";
    }
    public override void doPersonalizedBehavior(GameObject go)
    {
        Collider2D[] boxes = Physics2D.OverlapCircleAll(go.transform.position, massChangeRadius, go.GetComponent<SupremeProjectile>().layers);
        if (boxes.Length > 0)
        {
            go.GetComponent<Rigidbody2D>().mass = finalMass;
        }
    }
}

[System.Serializable]
public class SpinPS : ProjectileBehavior
{
    [Range(0, 1000)]
    [Tooltip("Indica cuanto rotará el objeto afectado por la rotación.")]
    public float rotationForce = 0f;
    //[Tooltip("Giro hacia la izquierda (antihorario)")]
    //public bool leftSpin = false;
    //[Tooltip("Giro hacia la derecha (horario)")]
    //public bool rightSpin = false;
    //[Tooltip("Giro al azar.")]
    //public bool randomSpin = true;
    [Range(0, 5)]
    public float radiusEffect = 0;

    public override string animationState()
    {
        return "default";
    }
    public override void doPersonalizedBehavior(GameObject go)
    {
        Collider2D[] boxes = Physics2D.OverlapCircleAll(go.transform.position, radiusEffect, go.GetComponent<SupremeProjectile>().layers);
        foreach (Collider2D box in boxes)
        {
            DamagemechanicsDef hasDamageMechanics = box.gameObject.GetComponent<DamagemechanicsDef>();
            if (hasDamageMechanics)
            {
                hasDamageMechanics.setSpin(go.gameObject.GetComponent<SupremeProjectile>().properties.spin);
                hasDamageMechanics.setTypeOfDamageReceived("OtherDeath");
            }
        }
        return;
    }
}

[System.Serializable]
public class KirbyPS : ProjectileBehavior
{
    [Range(0, 10)]
    public float maximumJumpsNumber = 4f;
    [Range(0, 30)]
    public float jumpForce = 10f;
    [Range(0, 30)]
    public float sideForce = 3f;
    [Range(0, 15)]
    public float kirbySwallowPower = 30;
    [Range(0, 15)]
    public float swallowRadius = 1.2f;
    [Range(0, 1000)]
    public float kirbyMass = 10;
    [Range(0, 5)]
    public float gravityScale = 4;
    [Range(0, 5)]
    public float rockModeFallingForce = 10;

    //private bool press = false;

    private string animatorState = "default";
    private bool once = false;
    private Rigidbody2D meRigidbody;
    private Transform meTransform;
    private LayerMask meLayer;
    private bool rock = false;
    private SpringJoint2D elastic;

    public override string animationState()
    {
        return animatorState;
    }
    public override void doPersonalizedBehavior(GameObject go)
    {
        if (!once)
        {
            meRigidbody = go.gameObject.GetComponent<Rigidbody2D>();
            meTransform = go.transform;
            meLayer = go.gameObject.GetComponent<SupremeProjectile>().layers;
            elastic = go.GetComponent<SupremeProjectile>().components.elasticCord;
            once = true;
        }
        jump();
        transformIntoRock();
    }

    private void jump()
    {
        if (Input.GetMouseButtonDown(0) && !elastic.enabled)
        {
            if (maximumJumpsNumber >= 1)
            {
                if (meRigidbody.velocity.x >= 0)
                {
                    meRigidbody.velocity = new Vector3(meRigidbody.velocity.x + sideForce, jumpForce, 0);
                }
                else
                {
                    meRigidbody.velocity = new Vector3(meRigidbody.velocity.x - sideForce, jumpForce, 0);
                }
                maximumJumpsNumber -= 1;
                //animatorState = "kirbyJumping";
                //return;
            }
            animatorState = "default";
        }
        checkToAttract();
    }
    private void checkToAttract()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(meTransform.position, swallowRadius, meLayer);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D hasRigidbody = collider.gameObject.GetComponent<Rigidbody2D>();
            bool isKirby = hasRigidbody == meRigidbody;
            if (hasRigidbody && !isKirby)
            {
                hasRigidbody.mass = 0.1f;
                attract(hasRigidbody);
            }
        }
    }
    private void attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = meRigidbody.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = kirbySwallowPower * (kirbyMass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    private void transformIntoRock()
    {
        if (maximumJumpsNumber <= 0)
        {
            meRigidbody.velocity = new Vector2(0, meRigidbody.velocity.y);
            meRigidbody.rotation = 0;
            meRigidbody.gravityScale = rockModeFallingForce;
            kirbyMass = 200;
            meRigidbody.sharedMaterial.bounciness = 0;
            //animatorState = "kirbyRock";
            //return;
        }
        animatorState = "default";
    }

    public Rigidbody2D getRigidbody()
    {
        return this.meRigidbody;
    }

}

[System.Serializable]
public class ExpandPS : ProjectileBehavior
{
    [Range(0, 2)]
    public float disableCollDetectionRadius=1;
    [Range(0, 8)]
    public float radio = 2;
    [Range(0, 100)]
    public float mass = 1;
    [Range(0, 500)]
    public float pushingForce = 4;
    private bool getRigigbody = false;
    private bool expanded = false;
    private Rigidbody2D rigidbody;
    private LayerMask layer;
    [HideInInspector]
    public float currentRadius=0f;
    private SpringJoint2D elasticCorde;
    private Collider2D collider;
    public Transform botLimit;
    public Transform topLimit;
    public Transform leftLimit;
    public Transform rightLimit;

    private SupremeProjectile realCorp;
    private List<Collider2D> disabledColliders = new List<Collider2D>();

    public override string animationState()
    {
        return "default";
    }

    public override void doPersonalizedBehavior(GameObject go)
    {
        if (!getRigigbody)
        {
            rigidbody = go.GetComponent<Rigidbody2D>();
            layer = go.GetComponent<SupremeProjectile>().layers;
            elasticCorde = go.GetComponent<SupremeProjectile>().components.elasticCord;
            collider = go.GetComponent<Collider2D>();
            realCorp = go.GetComponent<SupremeProjectile>();
        }
        checkToExpand();
        expand();
        checkBorders();
    }
    private void switchCollider2D(bool kinematic, bool collider)
    {
        if (!elasticCorde.enabled)
        {
            Collider2D[] near = Physics2D.OverlapCircleAll(rigidbody.position, disableCollDetectionRadius, layer);
            foreach (Collider2D coll in near)
            {
                DamagemechanicsDef hasDamageMechanics = coll.gameObject.GetComponent<DamagemechanicsDef>();
                if (hasDamageMechanics)
                {
                    coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = kinematic;
                    coll.gameObject.GetComponent<Collider2D>().enabled = collider;
                    if (collider)
                    {
                        foreach(Collider2D c in disabledColliders)
                        {
                            c.enabled = true;
                            c.gameObject.GetComponent<Rigidbody2D>().isKinematic = kinematic;
                        }
                    }
                    else
                    {
                        if (!disabledColliders.Contains(coll))
                        {
                            disabledColliders.Add(coll);
                        }
                    }
                }
            }
        }
    }

    private void checkToExpand()
    {
        if (Input.GetMouseButtonDown(0) && !elasticCorde.enabled && !expanded)
        {
            //switchCollider2D(false, true);
            expanded = true;
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            rigidbody.isKinematic = true;
        }
        if(!elasticCorde.enabled && !expanded)
        {
            collider.enabled = false;
        }
    }

    private void expand()
    {
        if (expanded)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(rigidbody.position, radio, layer);
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D hasRigidbody = collider.gameObject.GetComponent<Rigidbody2D>();
                bool isExpand = hasRigidbody == rigidbody;
                if (hasRigidbody && !isExpand)
                {
                    repel(hasRigidbody);
                }
            }
            if (currentRadius < radio)
            {
                currentRadius += 1;
            }
        }
    }

    private void repel(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = rigidbody.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = -pushingForce * (mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    private void checkBorders()
    {
        if(rigidbody.position.y <= botLimit.position.y)
        {
            //realCorp.properties.lifeTime = 1;
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            rigidbody.isKinematic = true;
        }
        if (rigidbody.position.y >= topLimit.position.y)
        {
            //realCorp.properties.lifeTime = 1;
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            rigidbody.isKinematic = true;
        }
        if (rigidbody.position.x <= leftLimit.position.x)
        {
            //realCorp.properties.lifeTime = 1;
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            rigidbody.isKinematic = true;
        }
        if (rigidbody.position.x >= rightLimit.position.x)
        { 
            //realCorp.properties.lifeTime = 1;
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            rigidbody.isKinematic = true;
        }
    }

}

[System.Serializable]
public class Pegaloco : ProjectileBehavior
{
    [Range(0, 5)]
    public float frequency = 2f;
    [Range(0, 1000)]
    public float finalMass = 50f;
    [Tooltip("LineRendere para dibujar el elástico")]
    public LineRenderer lineRenderer;
    private SpringJoint2D ownElastic;
    private Rigidbody2D ownHook;
    private bool start = false;
    private Rigidbody2D projectileRb;
    private bool fijado = false;
    private float lifeTime = 0;
    GameObject thisGameObject;

    public override string animationState()
    {
        return "default";
    }

    public override void doPersonalizedBehavior(GameObject go)
    {
        setUp(go);
        rebota();
        return;
    }

    private void setUp(GameObject go)
    {
        if (!start)
        {
            projectileRb = go.GetComponent<Rigidbody2D>();
            ownElastic = go.GetComponent<SpringJoint2D>();
            ownHook = go.GetComponent<SupremeProjectile>().components.hookRigidbody2D;
            start = true;
            lifeTime = go.GetComponent<SupremeProjectile>().properties.lifeTime;
            thisGameObject = go;
            if (lineRenderer)
            {
                lineRenderer.positionCount = 2;
            }
        }
    }

    private void rebota()
    {
        if (Input.GetMouseButtonDown(0) && !fijado && !ownElastic.enabled)
        {
            thisGameObject.GetComponent<TrailRenderer>().enabled = false;
            ownElastic.enabled = true;
            ownElastic.frequency = frequency;
            projectileRb.mass = finalMass;
            lineRenderer.SetPosition(0, ownHook.position);
            lineRenderer.materials[0].color = thisGameObject.GetComponent<SpriteRenderer>().color;
            lineRenderer.startWidth = 0.25f;
            lineRenderer.endWidth = 0.25f;
            ownHook.bodyType = RigidbodyType2D.Static;
            fijado = true;
        }
        if(!fijado && !ownElastic.enabled)
        {
            ownHook.bodyType=RigidbodyType2D.Dynamic;
            ownHook.position = projectileRb.position;
            ownElastic.anchor = Vector2.zero;
            ownElastic.connectedAnchor = Vector2.zero;
        }
        if (fijado)
        {
            lifeTime -= 1.5f;
            if (lifeTime <= 0)
            {
                thisGameObject.SetActive(false);
            }
            lineRenderer.SetPosition(1, projectileRb.position);
        }
    }
}

[System.Serializable]
public class Multibomb : ProjectileBehavior
{
    private bool setUp = false;
    public override string animationState()
    {
        return "default";
    }

    public override void doPersonalizedBehavior(GameObject go)
    {
        return;
    }

    private void start()
    {
        if (!setUp)
        {
            setUp = true;
        }
    }

}



[System.Serializable]
public class EnProgresoPS
{
    //[Header("En progreso")]
    [Tooltip("Propiedad aún no implementada.")]
    public bool canElectrocute = false;
    [Tooltip("Proposito: expandir el proyectil en un momento dado moviendo los objetos a su alrededor.")]
    public bool canEnlarge = false;
    //[Range(0, 1000)]
    //[Tooltip("Indica la proporcion de crecimiento del ¿proyectil?. (Aún no implementado)")]
    //public float enlargeSize = 0f;
    [Tooltip("Proposito: desaparecer luego del lanzamiento y aparecer en un momento dado generando una explosión.")]
    public bool disappear = false;
    [Tooltip("Proposito: rebotar una cierta cantidad de veces y luego desaparecer (rebote tipo pong).")]
    public bool cristal = false;
    [Tooltip("Proposito: al hacer click en ella puede dar saltos adicionales (segundo, tercer y cuarto salto).")]
    public bool kirby = false;
    [Tooltip("Proposito: dormir a un objeto, desactiva funcionalidades, poderes, o cualquer atributo extra por un tiempo determinado.")]
    public bool jigglypuff = false;
    [Tooltip("Proposito: se mueve como burbuja, cuando toca una superficie la revienta como si fuera una burbuja. (sin causar movimientos ni reaccion en cadena)")]
    public bool bubble = false;
}

#endregion


/*
        int touchedObjetcs = 0;
        DamagemechanicsDef[] damageableObjects = GameObject.FindObjectsOfType<DamagemechanicsDef>();                    //Se obtienen los objetos dañables
        foreach (DamagemechanicsDef objectTouched in damageableObjects)                                                 //Por cada objeto dañable
        {
            if (components.rigidbody2D.IsTouching(objectTouched.gameObject.GetComponent<Collider2D>()))                 //Si este proyectil esta tocando a un objeto dañable, IamDoingDamage queda en true
            {
                touchedObjetcs += 1;
            }    
        }
        //Si IamDoingDamage queda true, se inicia la animacion, si queda falso la detiene. (según el tipo de proyectil que sea (depende de typeOfProjectile) hara una u otra animacion).
        */

/*
if (components.lineRenderer == null)                                                    //Asigna el line renderer
    {
        components.lineRenderer = gameObject.GetComponent<LineRenderer>();                  //Debug.Log("Assigning LineRenderer");
        components.lineRenderer.material.color = new Color(properties.R, properties.G, properties.B);
        Debug.Log("     If you can't see the line color, be sure to assign a material to the linerenderer component. (Material with a sprite shader).");

        components.lineRenderer.SetPosition(0, components.hookRigidbody2D.position);
        components.
    }
    else
    {
        components.lineRenderer.material.color = new Color(properties.R, properties.G, properties.B);
        components.lineRenderer.SetPosition(0, components.hookRigidbody2D.position);
        components.lineRenderer.startWidth = 0.1f;
        components.lineRenderer.endWidth = 0.1f;
    }
*/
