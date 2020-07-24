using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class DamagemechanicsDef : MonoBehaviour
{
    //[TextArea(3, 3)]
    //public string ConsiderecionesDeUso = "Para que este objeto dañable (DamageMechanics) funcione se requiere al menos un objeto proyectil (Projectile) que haga el daño.";

    #region Atributos públicos
    public DmComponents components;
    public DmProperties properties;
    
    [Header("Mask of the items to check")]
    public LayerMask layerMask;
    #endregion

    #region Atributos privados
    private bool animated = false;
    private FirePS firep;
    private IcePS icep;
    private BombPS bomb;
    [HideInInspector]
    private SpinPS spin;
    private KirbyPS kirby;
    private bool gaveAPoint = false;
    [HideInInspector]
    private string typeOfDamageReceived="";
    private Buscar utils = new Buscar();
    private bool didItOnce = false;
    #endregion


    private void Start()
    {
        properties.seconds = 1;
        if (components.deathAnimator==null)
        {
            components.deathAnimator = gameObject.GetComponent<Animator>();
        }
        if (components.objectRigidbody2D != null)
        {
            components.objectRigidbody2D.mass = properties.mass;
        } 
    }

    private void FixedUpdate()
    {
        checkIfIamBeingAttacked();
        checkAttackEffects();
        checkIfIamDead();
    }


    #region Related to If I am being attacked
    private void checkIfIamBeingAttacked()
    {
        SupremeProjectile[] listapro = GameObject.FindObjectsOfType<SupremeProjectile>();
        foreach (SupremeProjectile projectile in listapro)
        {
            if (components.objectRigidbody2D.IsTouching(projectile.gameObject.GetComponent<Collider2D>()))
            {
                assignIdentity(projectile);//Debug.Log("Found a " + projectile.name);
            }
        }
    }
    private void assignIdentity(SupremeProjectile projectile)
    {
        if (projectile.properties.canRemoveGravity) { ungravity(); }
        if (projectile.properties.projectileType == ProjectileProperties.ProjectileType.Fire) { this.firep = ((FirePS)projectile.projectile); typeOfDamageReceived = "Burned"; }
        if (projectile.properties.projectileType == ProjectileProperties.ProjectileType.Ice) { this.icep = (IcePS)projectile.projectile; typeOfDamageReceived = "Frozen";  }
        if (projectile.properties.projectileType == ProjectileProperties.ProjectileType.Bomb) { this.bomb = (BombPS)projectile.projectile; typeOfDamageReceived = "Electric";  }
        if (projectile.properties.projectileType == ProjectileProperties.ProjectileType.Spin) { this.spin = (SpinPS)projectile.projectile; typeOfDamageReceived = "Spin";  }
        if (projectile.properties.projectileType == ProjectileProperties.ProjectileType.Kirby) { this.kirby = (KirbyPS)projectile.projectile; typeOfDamageReceived = "OtherDeath";  }
    }
    private void ungravity()
    {
        components.objectRigidbody2D.gravityScale = 0;
    }
    #endregion

    #region Related to check attack effects
    private void checkAttackEffects()
    {
        if (firep!=null) { Burn(); }
        if (icep != null) { Freeze(); }
        if (bomb != null) { Explode(); }
        if (spin != null) { Rotate(); }
        if (kirby != null) { Swallow(); }
    }
    private void Burn()
    {
        if (properties.health > -1)
        {
            //Debug.Log("Me Quemo!! :c");
            properties.health -= firep.damage;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            burnsAreaAround();
        }
    }
    private void burnsAreaAround()
    {
        Object[] boxes = utils.getElemntsArround(this.gameObject,properties.fireExpandRadius, "DamagemechanicsDef");
        foreach(Object element in boxes)
        {
            if(GameObject.Find(element.name) != this.gameObject)
            {
                GameObject.Find(element.name).GetComponent<DamagemechanicsDef>().properties.health -= firep.damage * 0.9f;
                GameObject.Find(element.name).GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
    private void Freeze()
    {
        //Falta el daño por impacto
        if (properties.health > -1)
        {
            if (icep.freezingTime > 0)
            {
                if (components.objectRigidbody2D.rotation > 0)
                {
                    components.objectRigidbody2D.AddTorque(properties.rotationForce);
                }
                else
                {
                    components.objectRigidbody2D.AddTorque(-properties.rotationForce);
                }
            }
            else
            {
                components.objectRigidbody2D.freezeRotation = true;
                components.objectRigidbody2D.velocity = Vector3.zero;
                components.objectRigidbody2D.gravityScale = 0;
                GameObject.Find(components.objectRigidbody2D.name).GetComponent<SpriteRenderer>().color = Color.cyan;
                //objectRigidbody2D.isKinematic = true;
            }
            icep.freezingTime -= 1;
            properties.health -= icep.damage;
            freezeAreaArround(icep.freezingTime);
        }
    }
    private void freezeAreaArround(float freezingTime)
    {
        if (icep.freezingTime < -5)
        {
            Object[] boxes = utils.getTouchedElements("DamagemechanicsDef", this.gameObject);
            foreach (Object element in boxes)
            {
                if (GameObject.Find(element.name) != this.gameObject)
                {
                    GameObject.Find(element.name).GetComponent<DamagemechanicsDef>().properties.health -= icep.damage * 0.9f;
                    GameObject.Find(element.name).GetComponent<SpriteRenderer>().color = Color.cyan;
                }
            }
        }
    }
    private void Explode()
    {
        if (didItOnce == false)
        {
            components.objectRigidbody2D.mass = 1f;
            //Debug.Log("Bumm!");
            float magnitud = Random.Range(bomb.explotionForceMin, bomb.explotionForceMax);
            float torque = Random.Range(-bomb.explotionForceMin, bomb.explotionForceMax);
            float x = Random.Range(-10, 10);
            float y = Random.Range(0.2f, 5);

            Vector2 direction = new Vector2(x, y).normalized;

            components.objectRigidbody2D.AddForce(direction * magnitud *100);
            components.objectRigidbody2D.AddTorque(torque / 2);
            didItOnce = true;
        }

        if (properties.health > -1)
        {
            properties.health -= bomb.damage;
            //GameObject.Find(objectRigidbody2D.name).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    private void Rotate()
    {
        if (didItOnce == false)
        {
            float initTorque = components.objectRigidbody2D.rotation;
            if (initTorque<0)
            {
                properties.rotationForce = -spin.rotationForce;
            }
            else
            {
                properties.rotationForce = spin.rotationForce;
            }
            didItOnce = true;
        }

        if (properties.health > -1)
        {
            components.objectRigidbody2D.AddTorque(properties.rotationForce);
            properties.health -= spin.damage;
        }
    }
    private void Swallow()
    {
        if (kirby.getRigidbody().IsTouching(components.objectCollider2D))
        {
            if (properties.health > -1)
            {
                properties.health -= kirby.damage;
            }
        }
    }
    #endregion

    #region related to check if I am dead
    private void checkIfIamDead()
    {
        if (properties.health <= 0)
        {
            giveScore();
            if (!animated)
            {
                deathAnimation();
                animated = true;
                StartCoroutine(WaitForDeath(properties.seconds));
            }
        }
    }
    private void deathAnimation()
    {
        components.deathAnimator.SetTrigger(typeOfDamageReceived);
    }
    IEnumerator WaitForDeath(int pSeconds)
    {
        yield return new WaitForSeconds(pSeconds);
        gameObject.SetActive(false);
    }
    private void giveScore()
    {
        if (properties.givePoints && !gaveAPoint)
        {
            if (GameObject.FindObjectOfType<ScoreD>() != null)
            {
                GameObject.FindObjectOfType<ScoreD>().addPoints(properties.points);
            }
            gaveAPoint = true;
        }
    }
    #endregion


    private void OnDrawGizmos()
    {
        if (properties.iceeRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(this.transform.position, properties.iceExpandRadius);
        }
        if (properties.fireRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, properties.fireExpandRadius);
        }

       
    }


    public void setTypeOfDamageReceived(string newDamage)
    {
        this.typeOfDamageReceived = newDamage;
    }
    public void setSpin(SpinPS newSpin)
    {
        this.spin = newSpin;
    }

}



[System.Serializable]
public class DmComponents
{
    //[Header("Object Components")]
    [Tooltip("Componente Transform del objeto que puede ser dañado.")]
    public Transform objectTransform;
    [Tooltip("Componente Rigidbody2D del objeto que puede ser dañado.")]
    public Rigidbody2D objectRigidbody2D;
    [Tooltip("Componente Collider2D del objeto que puede ser dañado.")]
    public Collider2D objectCollider2D;
    public Animator deathAnimator;


}


[System.Serializable]
public class DmProperties
{
    [Header("Object Properties")]
    [Space]
    [Range(0, 5000)]
    [Tooltip("Indica la masa del objeto.")]
    public float mass = 10f;

    [Space]
    [Range(0, 1000)]
    [Tooltip("Indica la vida del objeto")]
    public float health = 300f;

    [Range(0, 1000)]
    public float rotationForce = 0f;

    [Space]
    [Tooltip("Indica si el objeto suma puntaje al morir. (True significa que da puntaje)")]
    public bool givePoints = false;
    [Range(0, 20)]
    [Tooltip("Indica la cantidad de puntos que el objeto da al morir. (No dara puntos si no esta habilitada la opcion 'givePoints')")]
    public int points = 1;
    [Tooltip("segundos para morir")]
    public int seconds;

    [Header("to display the effect ranges")]
    public bool fireRadius = false;
    [Range(0, 10)]
    public float fireExpandRadius = 1f;
    public bool iceeRadius = false;
    [Range(0, 10)]
    public float iceExpandRadius = 1f;

}