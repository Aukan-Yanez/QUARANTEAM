using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CanyonDef : MonoBehaviour
{
    public CanyonComponents components;
    public CanyonProperties properties;
    public CanyonObjective objective;

    public LayerMask layers;
    private Vector2 forceDirection;
    private float initCountdown;
    private float initGravityScale;
    private bool isShooting;
    private float cooldown;
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

        isShooting = false;
        forceDirection = Vector2.zero;
        initCountdown = properties.countdown;
        initGravityScale = 0;
        cooldown = 0;

    }
    private void FixedUpdate()
    {
        reload();
        shoot();
    }
    // Update is called once per frame
    

    private void reload()
    {
        //Reload
        Collider2D[] supplies = Physics2D.OverlapCircleAll(components.transform.position, properties.detectionArea, layers);

        if (components.pointerTransform && isShooting==false)
        {
            foreach (Collider2D bullet in supplies)
            {
                if (bullet.name == objective.bullet.name)
                {
                    Vector2 canyonDirection = components.pointerTransform.position - components.transform.position;
                    forceDirection = canyonDirection * properties.shootForceMagnitude;
                    objective.bullet.velocity = Vector2.zero;
                    initGravityScale = objective.bullet.gravityScale;
                    objective.bullet.gravityScale = 0;
                    objective.bullet.position = components.rigidbody.position;
                    isShooting = true;
                }
            }
        }
    }

    private void shoot()
    {
        if (isShooting && forceDirection!=Vector2.zero)
        {
            properties.countdown = properties.countdown-1;
            Vector2 canyonDirection = components.pointerTransform.position - components.transform.position;
            forceDirection = canyonDirection * properties.shootForceMagnitude;
            //Debug.Log("Countdown(" + forceDirection + ").");
            if (properties.countdown <= 0)
            {
                components.pointerCollider2D.enabled = false;
                objective.bullet.gravityScale = initGravityScale;
                objective.bullet.AddForce(forceDirection);
                forceDirection = Vector2.zero;
                properties.countdown = initCountdown;
                cooldown = 10;
            }
        }

        if (cooldown>0)
        {
            cooldown -= 1;
            if (cooldown <= 0) 
            { 
                cooldown = 0;
                isShooting = false;
                components.pointerCollider2D.enabled = true;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (components.transform)
        {
            Vector2 adjustedCenter = new Vector2(components.transform.position.x + properties.areaOffsetXAxis, components.transform.position.y + properties.areaOffsetYAxis);
            Gizmos.DrawWireSphere(adjustedCenter, properties.detectionArea);
        }
    }



}

[System.Serializable]
public class CanyonComponents
{
    public Transform transform;
    public Rigidbody2D rigidbody;
    public Transform pointerTransform;
    public Collider2D pointerCollider2D;
}

[System.Serializable]
public class CanyonProperties
{
    [Tooltip("Rango de deteccion del proyectil")]
    [Range(0, 3)]
    public float detectionArea = 1f;

    [Tooltip("Magnitud de la fuerza para disparar el proyectil")]
    [Range(0, 1000)]
    public float shootForceMagnitude = 100f;

    [Tooltip("Cuenta regresiva para disparar")]
    [Range(0, 200)]
    public float countdown = 5f;

    [Tooltip("Desplazamiento del area en el eje 'X'")]
    [Range(-20, 20)]
    public float areaOffsetXAxis = 0f;

    [Tooltip("Desplazamiento del area en el eje 'Y'")]
    [Range(-20, 20)]
    public float areaOffsetYAxis = 0f;
}

[System.Serializable]
public class CanyonObjective
{
    public Rigidbody2D bullet;
    public Collider2D Collider2D;
}