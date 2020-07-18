using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DkbarrelDef : MonoBehaviour
{
    public DkComponents components;
    public DkProperties properties;
    public string exception;
    public string exception2;
    //public DkObjective objective;

    public LayerMask layers;
    private List<Collider2D> currentBullets;
    private bool isShooting = false;
    private List<float> initGravityScale;
    private float cooldown;
    private bool charging;

    private List<(Collider2D, float)> CurrentBullets;
    // Start is called before the first frame update
    void Start()
    {
        if (components.transform == null)
        {
            components.transform = gameObject.GetComponent<Transform>();
        }
        if (components.rigidbody == null)
        {
            components.rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        cooldown = 0;
        isShooting = false;
        charging = false;
        currentBullets = new List<Collider2D>();
        initGravityScale = new List<float>();
        CurrentBullets = new List<(Collider2D, float)>();
    }

    // Update is called once per frame
    void Update()
    {
        reloading();
        shoot();
    }

    private void reloading()
    {
        if (!isShooting)
        {
            Collider2D[] currentColliders = Physics2D.OverlapCircleAll(components.rigidbody.position, properties.detectionArea, layers);
            
            foreach (Collider2D collider in currentColliders)
            {
                if (collider.name != exception && collider.name != exception2)
                {
                    print("Agregando: " + collider.name);
                    if (!CurrentBullets.Contains((collider, collider.gameObject.GetComponent<Rigidbody2D>().gravityScale)))// && currentCollider != components.exitSideCollider)
                    {
                        CurrentBullets.Add((collider, collider.gameObject.GetComponent<Rigidbody2D>().gravityScale));
                        collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                        collider.gameObject.GetComponent<Rigidbody2D>().position = components.rigidbody.position;
                        collider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                }            
            }

            if (Input.GetMouseButtonDown(0) && currentColliders.Length>0)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float bordeDerecho = components.transform.position.x - components.transform.localScale.x;//.....
                if (mousePosition.x < bordeDerecho )
                {

                }
                //Falta comprobar que haga click dentro del barril, ya que se activan otras cosas por ahí xdxdxdx
                isShooting = true;
            }
        }
    }

    private void shoot()
    {
        Vector2 canyonDirection = components.exitSideTransform.position - components.transform.position;
        Vector2 forceDirection = canyonDirection * properties.shootForceMagnitude;

        if (isShooting && !charging)
        {
            if (CurrentBullets.Count > 0)
            {
                charging = true;
                foreach ((Collider2D,float) projectile in CurrentBullets)
                {
                    projectile.Item1.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    projectile.Item1.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection);
                }
                CurrentBullets.Clear();
                cooldown = 100;
            }
        }

        if (cooldown > 0)
        {
            
            cooldown -= 1;
            if (cooldown <= 0)
            {
                //Debug.Log("Cooldown:" + cooldown);
                cooldown = 0;
                isShooting = false;
                charging = false;
            }
        }

    }



    private void OnDrawGizmos()
    {
        if (components.rigidbody)
        {
            Gizmos.DrawWireSphere(components.rigidbody.position, properties.detectionArea);
        }
        
    }

}



[System.Serializable]
public class DkComponents
{
    public Transform transform;
    public Rigidbody2D rigidbody;
    [Tooltip("trnasform que indica por cual lado del barril saldra el/los proyectil/proyectiles;")]
    public Transform exitSideTransform;
    //public Collider2D exitSideCollider;
}

[System.Serializable]
public class DkProperties
{
    [Tooltip("Rango de deteccion del proyectil")]
    [Range(0, 3)]
    public float detectionArea = 1f;

    [Tooltip("Magnitud de la fuerza para disparar el proyectil")]
    [Range(0, 1000)]
    public float shootForceMagnitude = 100f;

    //[Tooltip("Cuenta regresiva para disparar")]
    //[Range(0, 200)]
    //public float countdown = 5f;

    //[Tooltip("Desplazamiento del area en el eje 'X'")]
    //[Range(-20, 20)]
    //public float areaOffsetXAxis = 0f;

    //[Tooltip("Desplazamiento del area en el eje 'Y'")]
    //[Range(-20, 20)]
    //public float areaOffsetYAxis = 0f;
}

[System.Serializable]
public class DkObjective
{
    public Rigidbody2D bullet;
    public Collider2D Collider2D;
}