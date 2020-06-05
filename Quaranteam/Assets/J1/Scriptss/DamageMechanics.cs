using System.Collections;
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
    private float spinDamage = 0f;
    private bool burnable = false;
    private float burningDamage = 0f;
    private float burningRadius = 0f;
    private bool explode = false;
    private float explotionDamage = 30f;
    private bool freezable = false;
    private float freezingTime = 0f;
    private float initialDamagePercentage = 0f;
    private float freezeDamage = 0;
    private Animator deathAnimator;

    [Header("segundos para morir")]
    public int seconds;

    private Fire firep;
    private Ice icep;
    private Bomb bomb;
    private Spin spin;
    private Kirby kirby;

    private bool animated = false;

    private void Start()
    {
        seconds = 1;
        deathAnimator = GetComponent<Animator>();
        if (objectRigidbody2D!= null)
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


    

    private void applyEnemysEffect(Projectile enemy)
    {
        if (enemy.canRemoveGravity) { ungravity(); }
        if (enemy.fire.enable) { fire(enemy); }
        if (enemy.ice.enable) { ice(enemy); }
        if (enemy.bomb.enable) { explotion(enemy); }
        if (enemy.spin.enable) { rotate(enemy); }
        if (enemy.kirby.enable) { swallow(enemy); }
    }

    private void swallow(Projectile enemy)
    {
        kirby = enemy.kirby;
        //attract(objectRigidbody2D, kirby.kirbyRigidbody, kirby.kirbySwallowPower, kirby.kirbyMass);
        if (kirby.kirbyRigidbody.IsTouching(objectCollider2D))
        {
            health -= kirby.damage;
        }
        
    }



    private void explotion(Projectile enemy)
    {
        float magnitud = Random.Range(enemy.bomb.explotionForceMin, enemy.bomb.explotionForceMax);
        float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);

        Vector2 direction = new Vector2(x, y);

        objectRigidbody2D.AddForce(direction * magnitud);
        objectRigidbody2D.AddTorque(magnitud / 2);

        explode = true;
        explotionDamage = enemy.bomb.explotionDamage;
        health -= explotionDamage;
        Debug.Log("Explóto!..");
    }
    
    //El elemento quemado debe arder y quemar el area a su alrededor
    private void fire(Projectile enemy) 
    {
        burnable = true;
        burningDamage = enemy.fire.burningDamage;
        burningRadius = enemy.fire.burningRadius;
        health -= 1;
        Debug.Log(this.name + ": Me Quemoo!..");
    }

    //El elemento congelado debe congelar a los elementos que este tocando volviendolos "más fragiles"
    private void ice(Projectile enemy) 
    {
        if (freezable==false)
        {
            freezable = true;
            initialDamagePercentage = enemy.ice.initialDamagePercentage;
            freezeDamage = enemy.ice.freezeDamage;
            freezingTime = enemy.ice.freezingTime;
            ungravity();
            freezingTime -= 1;
            health -= initialDamagePercentage * health / 100;
        }
    }





    private void ungravity()
    {
        objectRigidbody2D.gravityScale = 0;
    }



    private void rotate(Projectile enemy)
    {
        objectRigidbody2D.AddTorque(enemy.spin.rotationForce);
        rotable = true;
        rotationForce = enemy.spin.rotationForce;
        spinDamage = enemy.spin.spinDamage;
        if (enemy.spin.randomSpin)
        {
            int side = Random.Range(-5, 6);
            if (side > 0)
            {
                rotationForce = -rotationForce;
            }
        }
        else
        {
            if (enemy.spin.rightSpin)
            {
                rotationForce = -rotationForce;
            }
        }
        health -= spinDamage;
        Debug.Log("Me Girann!..");
    }




    private void checkPermanentEfects()
    {
        if (rotable && health>-1) 
        { 
            objectRigidbody2D.AddTorque(rotationForce);
            health -= spinDamage;
        }
        if(burnable && health > -1) 
        {
            health -= burningDamage;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255,100,0);
            burnsAreaAround();
        }
        if (explode && health > -1)
        {
            health -= explotionDamage;
            //GameObject.Find(objectRigidbody2D.name).GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (freezable && health > -1)
        {
            if (freezingTime > 0)
            {
                if(objectRigidbody2D.rotation > 0)
                {
                    objectRigidbody2D.AddTorque(rotationForce);
                }
                else
                {
                    objectRigidbody2D.AddTorque(-rotationForce);
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
        if (kirby!=null && kirby.enable)
        {
            if (kirby.kirbyRigidbody.IsTouching(objectCollider2D))
            {
                health -= kirby.damage;
            }
        }
    }


    private void checkHealth()
    {
        if(health <= 0)
        {
            if (!animated)
            {
                deathAnimation();
                animated = true;
                StartCoroutine(WaitForDeath(seconds));
                
            }

            //gameObject.SetActive(false);
            //GameObject.Find(objectRigidbody2D.name).GetComponent<SpriteRenderer>().enabled = false;
            //objectCollider2D.enabled = false;

            giveScore();
        }
    }

    IEnumerator WaitForDeath(int pSeconds)
    {
        yield return new WaitForSeconds(pSeconds);
        gameObject.SetActive(false);
    }



        private void deathAnimation()
    {
        if (burnable)
        {
            deathAnimator.SetTrigger("Burned");
        }

        if (freezable)
        {
            deathAnimator.SetTrigger("Frozen");
        }

        if (rotable)
        {
            deathAnimator.SetTrigger("Rotated");
        }

        if (explode)
        {
            deathAnimator.SetTrigger("Electric");
        }
        else
        {
            deathAnimator.SetTrigger("OtherDeath");
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



    private void attract(Rigidbody2D objToAttract, Rigidbody2D own, float swall, float mass)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = own.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = swall * (mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }







    


    private void OnDrawGizmos()
    {
        if (objectTransform == null)
        {
            return;
        }
        if (burnable)
        {
            //Gizmos.DrawWireSphere(objectTransform.position, burningRadius);
            //Gizmos.color = new Color(255, 0,0, 0.3f);
            //Gizmos.DrawSphere(objectTransform.position, burningRadius);
        }
        if (explode)
        {
            //Se debe dibujar un angulo, arco o semicircunferencia que representen los angulos de salida 
            //de los proyectiles al explotar.
        }

    }


    private void burnsAreaAround()
    {
        Collider2D[] arround = Physics2D.OverlapCircleAll(objectTransform.position, burningRadius, layerMask);
        GameObject[] someone = GameObject.FindObjectsOfType<GameObject>();
        


        foreach(Collider2D enemy in arround)
        {
            if (enemy != objectCollider2D)
            {
                for (int i = 0; i < someone.Length; i++)
                {
                    if (someone[i].GetComponent<DamageMechanics>() != null)
                    {
                        if (someone[i].GetComponent<DamageMechanics>().name == enemy.name)
                        {
                            //Debug.Log("Quemando tambien a: " + someone[i].GetComponent<DamageMechanics>().name);
                            someone[i].GetComponent<DamageMechanics>().health -= burningDamage * 0.9f;
                            someone[i].GetComponent<SpriteRenderer>().color = new Color(255, 128, 0);
                        }
                    }
                }
            }
        }
    }




}
