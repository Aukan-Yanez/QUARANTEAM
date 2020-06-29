using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDef : MonoBehaviour
{
    //public Transform gen;
    public int lifeTime = 100;

    [Tooltip("Necesaria si se requiere visualizar el radio afectado por algunas de las propiedades.")]
    public Transform objectTransform;
    [Tooltip("Nasd.")]
    public Rigidbody2D objectRigidbody2D;
    public Collider2D objectCollider2D;
    public CircleCollider2D objectCircleCollider2D;

    [Header("General Properties")]
    [Tooltip("Permite al proyectil ser reconocido o no por los demas objetos.")]
    public bool isActive = true;
    [Tooltip("Indica si el proyectil quita la gravedad al objeto que toca.")]
    public bool canRemoveGravity = false;

    


    public FireD fire;
    public IceD ice;
    public BombD bomb;
    public SpinD spin;
    public KirbyD kirby;
    public ExpandD expand;
    //public EnProgreso enProgreso;

    public Animator animator;
    [Range(0, 100)]
    public float AnimationTime = 0f;



    private float AnimationInitTime = 0f;
    private bool explotando = false;
    private bool congelando = false;
    private bool quemando = false;
    
    
    private void Update()
    {
        if (kirby.enable)
        {
            kirby.OnMouseDown();
            kirby.OnMouseUp();
            kirby.checkToAttract();
        }
        if (expand.enable)
        {
            expand.doit();
        }
    }

    private void Start()
    {
        AnimationInitTime = AnimationTime;
        if (kirby.enable)
        {
            kirby.kirbyRigidbody = objectRigidbody2D;
            kirby.kirbyColl = objectCollider2D;
            kirby.kirbyT = objectTransform;
            objectRigidbody2D.gravityScale = kirby.gravityScale;
        }
        if (expand.enable)
        {
            expand.cl = objectCircleCollider2D;
            expand.initRad = objectCircleCollider2D.radius;
            objectRigidbody2D.mass = expand.mass;
        }
    }

    private void FixedUpdate()
    {
        playAnimation();
        stopAnimations();
        if (!gameObject.GetComponent<SpringJoint2D>().enabled) 
        { 
            lifeTime--; 
        }
        if (lifeTime <=0)
        {
            gameObject.SetActive(false);
        }
    }


    private void playAnimation()
    {
        if (objectRigidbody2D == null)
        {
            return;
        }
        Collider2D[] impactedBodies = GameObject.FindObjectsOfType<Collider2D>();

        for (int i = 0; i < impactedBodies.Length; i++)
        {
            if (objectRigidbody2D.IsTouching(impactedBodies[i]))
            {
                if (GameObject.Find(impactedBodies[i].name).GetComponent<DamageMechanics>() != null)
                {
                    if (bomb.enable)
                    {
                        animator.SetBool("Explode", true);
                        explotando = true;
                    }
                    if (ice.enable)
                    {
                        animator.SetBool("Ice", true);
                        congelando = true;
                    }
                    if (fire.enable)
                    {
                        animator.SetBool("Fire", true);
                        quemando = true;
                    }
                }
            }
        }
    }


    private void stopAnimations()
    {
        if (explotando)
        {
            reduceAnimationTime("Explode");
        }
        if (congelando)
        {
            reduceAnimationTime("Ice");
        }
        if (quemando)
        {
            reduceAnimationTime("Fire");
        }
    }


    private void reduceAnimationTime(string condition)
    {
        AnimationTime -= 1;
        if (AnimationTime <= 0)
        {
            animator.SetBool(condition, false);
            AnimationTime = AnimationInitTime;
            explotando = false;
            congelando = false;
            quemando = false;
        }
    }




    private void OnDrawGizmos()
    {
        if (fire.enable && fire.burningRadius > 0 && objectTransform != null)
        {
            Gizmos.color = new Color(255, 0, 0);
            Gizmos.DrawWireSphere(objectTransform.position, fire.burningRadius);
        }
        if (kirby.enable)
        {
            Gizmos.color = new Color(255, 0, 128);
            Gizmos.DrawWireSphere(objectTransform.position, kirby.swallowRadius);
        }
        if (expand.enable && expand.cl != null)
        {
            Gizmos.DrawWireSphere(objectTransform.position, expand.cl.radius);
        }
    }

}

[System.Serializable]
public class FireD
{
    //[Header("Fire")]
    [Tooltip("Cuando proyectil tipo 'Fire' está activo, tiene la capacidad de quemar/incendiar en un radio determinado desde el punto de impacto. ")]
    public bool enable = false;
    [Range(0, 10)]
    [Tooltip("Indica cuanto de vida le resta al objeto afectado por el fuego en cada frame. (Impelmentado parcialmente)")]
    public float burningDamage = 0f;
    [Range(0, 20)]
    [Tooltip("Indica el rango que alcanza el fuego desde este objeto a su alrededor.")]
    public float burningRadius = 0f;
}

[System.Serializable]
public class IceD
{
    //[Header("Ice")]
    [Tooltip("Cuando proyectil tipo 'Ice' está activo, tiene la capacidad de congelar las plataformas que toca luego de un lapso de tiempo. ")]
    public bool enable = false;
    [Range(0, 100)]
    [Tooltip("Indica cuanto porcentaje de vida le resta al objeto afectado por el hielo en el primer golpe del proyectil.")]
    public float initialDamagePercentage = 0f;
    [Range(0, 100)]
    [Tooltip("Indica cuanto daño de vida le resta al objeto afectado por el hielo en cada frame.")]
    public float freezeDamage = 0f;
    [Range(0, 500)]
    [Tooltip("Indica cuanto se demorará en congelarse el objeto afectado por el hielo.")]
    public float freezingTime = 0f;
}


[System.Serializable]
public class BombD
{
    //[Header("Bomb")]
    [Tooltip("Cuando proyectil tipo 'Bomb' está activo, tiene la capacidad de explotar las plataformas que toca instantaneamente. ")]
    public bool enable = false;
    [Range(0, 3000)]
    [Tooltip("Rango mínimo de fuerza explosiva.")]
    public float explotionForceMin = 0f;
    [Range(0, 3000)]
    [Tooltip("Rango máximo de fuerza explosiva")]
    public float explotionForceMax = 0f;
    [Range(0, 100)]
    [Tooltip("Indica cuanto de vida le resta al objeto afectado por la explosión en cada frame.")]
    public float explotionDamage = 0f;



}

[System.Serializable]
public class SpinD
{
    //[Header("Spin")]
    [Tooltip("Cuando proyectil tipo 'Spin' está activo, tiene la capacidad de rotar las plataformas que toca hasta desaparecerlas a demás de quitarles la gravedad. ")]
    public bool enable = false;
    [Range(0, 1000)]
    [Tooltip("Indica cuanto rotará el objeto afectado por la rotación.")]
    public float rotationForce = 0f;
    [Range(0, 500)]
    [Tooltip("Daño del spin por tiempo transcurrido")]
    public float spinDamage = 0f;
    [Tooltip("Giro hacia la izquierda (antihorario)")]
    public bool leftSpin = false;
    [Tooltip("Giro hacia la derecha (horario)")]
    public bool rightSpin = false;
    [Tooltip("Giro al azar.")]
    public bool randomSpin = true;
}

[System.Serializable]
public class KirbyD
{
    public bool enable = false;
    [Range(0, 10)]
    public float maximumJumpsNumber = 0f;
    [Range(0, 30)]
    public float jumpForce = 0f;
    [Range(0, 30)]
    public float sideForce = 0f;
    [Range(0, 15)]
    public float kirbySwallowPower = 0;
    [Range(0, 15)]
    public float swallowRadius = 0;
    [Range(0, 1000)]
    public float kirbyMass = 0;
    [Range(0, 100)]
    public float damage = 0;
    [Range(0, 5)]
    public float gravityScale = 0;
    private bool press = false;

    [HideInInspector]
    public Rigidbody2D kirbyRigidbody;
    [HideInInspector]
    public Collider2D kirbyColl;
    [HideInInspector]
    public Transform kirbyT;
    [HideInInspector]
    public LayerMask lm;



    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) == true && maximumJumpsNumber >= 1)
        {
            if (!press)// && GameObject.Find(kirbyRigidbody.name).GetComponent<SpringJoint2D>().enabled == false)
            {
                if (kirbyRigidbody.velocity.x >= 0)
                {
                    kirbyRigidbody.velocity = new Vector3(kirbyRigidbody.velocity.x + sideForce, jumpForce, 0);
                }
                else
                {
                    kirbyRigidbody.velocity = new Vector3(kirbyRigidbody.velocity.x - sideForce, jumpForce, 0);
                }
                maximumJumpsNumber -= 1;
                press = true;
            }
        }
    }

    public void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) == true)
        {
            press = false;
        }
    }

    public void checkToAttract()
    {
        Rigidbody2D[] rigidbodies = GameObject.FindObjectsOfType<Rigidbody2D>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(kirbyT.position, swallowRadius);
        foreach (Collider2D collider in colliders)
        {
            foreach (Rigidbody2D rigidbody in rigidbodies)
            {
                if (rigidbody.name == collider.name && rigidbody != kirbyRigidbody)
                {
                    attract(rigidbody);
                }
            }
        }
    }

    private void attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = kirbyRigidbody.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = kirbySwallowPower * (kirbyMass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

}



[System.Serializable]
public class ExpandD
{
    [Range(0, 8)]
    public float radio = 0;
    public float radiusScale = 0;
    public float mass = 0;
    public bool enable = false;
    private bool exp = false;
    private bool expanding = false;
    private bool back = false;
    [HideInInspector]
    public float initRad = 0;
    [HideInInspector]
    public CircleCollider2D cl;
    public Blackhole bl;

    public void act()
    {
        if (!exp)// && GameObject.Find(cl.name).GetComponent<SpringJoint2D>().enabled == false)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                //expanding = true;
                bl.blackholeMass = 300;
                exp = true;
            }
        }
    }
    public void doit()
    {

        if (!exp && GameObject.Find(cl.name).GetComponent<SpringJoint2D>().enabled == false)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                expanding = true;
                exp = true;
            }
        }

        if (expanding)
        {
            if (cl.radius < radio)
            {
                cl.radius += radiusScale;
                Debug.Log("DJKHSKDJHSKJ");
            }
            if (cl.radius >= radio)
            {
                expanding = false;
                back = true;
            }
        }
        if (back)
        {
            if (cl.radius > initRad)
            {
                cl.radius -= radiusScale / 2;
            }
            if (cl.radius <= initRad)
            {
                back = false;
            }
        }

    }

}




[System.Serializable]
public class EnProgresoD
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
