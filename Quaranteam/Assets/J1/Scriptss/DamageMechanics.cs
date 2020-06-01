using UnityEngine;

[ExecuteAlways]
public class DamageMechanics : MonoBehaviour
{

    [TextArea(3, 3)]
    public string ConsiderecionesDeUso = "Para que este objeto dañable (DamageMechanics) funcione se requiere al menos un objeto proyectil (Projectile) que haga el daño.";

    [Header("Object Components")]
    [Tooltip("Componente Transform del objeto que puede ser dañado.")]
    public Transform objectTransform;
    [Tooltip("Componente Rigidbody2D del objeto que puede ser dañado.")]
    public Rigidbody2D objectRigidbody2D;
    [Tooltip("Componente Collider2D del objeto que puede ser dañado.")]
    public Collider2D objectCollider2D;

    //[Header("Object Properties")]
    //[Range(0, 10)]
    //[Tooltip("Indica el radio de detección del objeto para captar el proyectil.")]
    //public float radius = 0.5f;
    //[Range(0, 500)]
    //[Tooltip("Indica el ancho del objeto (se mide en el eje 'X').")]
    //public float sizeX = 0.5f;
    //[Range(0, 500)]
    //[Tooltip("Indica el alto del objeto (se mide en el eje 'Y').")]
    //public float sizeY = 0.5f;

    [Space]
    [Range(0, 5000)]
    [Tooltip("Indica la masa del objeto.")]
    public float mass = 10f;

    [Space]
    [Range(0, 1000)]
    [Tooltip("Indica la vida del objeto")]
    public float health = 300f;

    [Space]
    [Tooltip("Indica si el objeto suma puntaje al morir. (True significa que da puntaje)")]
    public bool givePoints = false;
    [Range(0, 20)]
    [Tooltip("Indica la cantidad de puntos que el objeto da al morir. (No dara puntos si no esta habilitada la opcion 'givePoints')")]
    public int points = 1;

    [Header("Mask of the items to check")]
    public LayerMask layerMask;

    public bool killMySelf = false;
    private bool gaveAPoint = false;
    private bool rotable = false;
    private float rotationForce = 0f;
    private bool burnable = false;
    private float burningDamage = 0f;
    private bool explode = false;
    private float explotionDamage = 30f;
    private bool freezable = false;
    private float freezingTime = 0f;
    private float freezeDamagePercent = 0f;
    private float freezeDamage = 0;




    private void Start()
    {
        if(objectRigidbody2D!= null)
            objectRigidbody2D.mass = mass;
    }

    private void FixedUpdate()
    {
        checkEnemies();
        checkHealth();

        checkKillMySelf();
        detectProjectils();
    }

    private void checkEnemies()
    {
        Projectile[] proyectiles = detectProjectils();
        for (int i=0; i<proyectiles.Length; i++)
        {
            if (proyectiles[i] != null)
            {
                applyEnemysEffect(proyectiles[i]);
            }
        }

        checkPermanentEfects();
    }



    /*private Projectile[] enemiesAttacking()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(objectTransform.position, radius, layerMask);
        Projectile[] proyectiles = GameObject.FindObjectsOfType<Projectile>();
        Projectile[] atacando = new Projectile[proyectiles.Length];

        foreach (Collider2D collider in colliders)
        {
            if (!collider.Equals(objectCollider2D))
            {
                for (int i = 0; i < proyectiles.Length; i++)
                {
                    if (proyectiles[i].name == collider.name)
                    {
                        if (proyectiles[i].isActive)
                        {
                            atacando[i] = proyectiles[i];
                        }
                    }
                }
            }
        }
        return atacando;
    }*/

    private void applyEnemysEffect(Projectile enemy)
    {
        if (enemy.canBurn) { fire(enemy); }
        if (enemy.canFreeze) { ice(enemy); }
        if (enemy.canExplode) { explotion(enemy); }
        if (enemy.canElectrocute) { electri(); }
        if (enemy.canRemoveGravity) { ungravity(); }
        if (enemy.canRotate) { rotate(enemy); }
    }



    private void explotion(Projectile enemy)
    {
        float magnitud = Random.Range(enemy.explotionForceMin, enemy.explotionForceMax);
        float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);

        Vector2 direction = new Vector2(x, y);

        objectRigidbody2D.AddForce(direction * magnitud);
        objectRigidbody2D.AddTorque(magnitud / 2);

        explode = true;
        explotionDamage = enemy.explotionDamage;
        health -= explotionDamage;
        Debug.Log("Explóto!..");
    }
    
    //El elemento quemado debe arder y quemar el area a su alrededor
    private void fire(Projectile enemy) 
    {
        burnable = true;
        burningDamage = enemy.burningDamage;
        health -= 1;
        Debug.Log("Me Quemoo!..");
    }

    //El elemento congelado debe congelar a los elementos que este tocando volviendolos "más fragiles"
    private void ice(Projectile enemy) 
    {
        freezable = true;
        freezeDamagePercent = enemy.freezeDamagePercent;
        freezeDamage = enemy.freezeDamage;
        freezingTime = enemy.freezingTime;
        ungravity();
        freezingTime -= 1;
        health -= freezeDamagePercent*health/100;
    }


    private void electri()
    {

    }



    private void ungravity()
    {
        objectRigidbody2D.gravityScale = 0;
    }



    private void rotate(Projectile enemy)
    {
        objectRigidbody2D.AddTorque(enemy.rotationForce);
        rotable = true;
        rotationForce = enemy.rotationForce;
        health -= 1;
        Debug.Log("Me Girann!..");
    }




    private void checkPermanentEfects()
    {
        if (rotable && health>-1) 
        { 
            objectRigidbody2D.AddTorque(rotationForce);
            health -= 1;
        }
        if(burnable && health > -1) 
        {
            health -= burningDamage; 
        }
        if (explode && health > -1)
        {
            health -= explotionDamage;
            GameObject.Find(objectRigidbody2D.name).GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (freezable && health > -1)
        {
            if (freezingTime > 0)
            {
                if(objectRigidbody2D.rotation > 0)
                {
                    objectRigidbody2D.AddTorque(2);
                }
                else
                {
                    objectRigidbody2D.AddTorque(-2);
                }
            }
            else
            {
                objectRigidbody2D.freezeRotation = true;
                objectRigidbody2D.velocity = Vector3.zero;
                objectRigidbody2D.gravityScale = 0;
                GameObject.Find(objectRigidbody2D.name).GetComponent<SpriteRenderer>().color = Color.cyan;
                //objectRigidbody2D.isKinematic = true;
            }
            freezingTime -= 1;
            health -= freezeDamage;
        }
    }


    private void checkHealth()
    {
        if(health <= 0)
        {
            GameObject.Find(objectRigidbody2D.name).GetComponent<SpriteRenderer>().enabled = false;
            objectCollider2D.enabled = false;
            giveScore();
        }
    }

    private void giveScore()
    {
        if (givePoints && !gaveAPoint)
        {
            if(GameObject.FindObjectOfType<ScoreD>() != null)
            {
                GameObject.FindObjectOfType<ScoreD>().addPoints(points);
            }
            gaveAPoint = true;
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(objectTransform.position, radius);
    }*/

    private Projectile[] detectProjectils()
    {
        Projectile[] listapro = GameObject.FindObjectsOfType<Projectile>();
        Projectile[] ret = new Projectile[listapro.Length];
        int i = 0;
        foreach (Projectile projectile in listapro)
        {
            Collider2D colliderPro = projectile.GetComponent<Collider2D>();
            if (colliderPro != null)
            {
                if (objectRigidbody2D.IsTouching(colliderPro))
                {
                    ret[i] = projectile;
                    //Debug.Log("Madera " + objectRigidbody2D.name + ": proyectil (" + projectile + ") está atacando.");
                }
            }
            i++;
        }
        return ret;
    }


    private void checkKillMySelf()
    {
        if (killMySelf)
        {
            health = -1;
        }
    }
}
