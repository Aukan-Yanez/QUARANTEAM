using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canyon : MonoBehaviour
{
    [Header("Canyon Components")]
    [Tooltip("Componente Rigidbody2D del cañon.")]
    public Rigidbody2D canyonRigidbody2D;
    [Tooltip("Componente Transform del cañon.")]
    public Transform canyonTransform;
    [Tooltip("Componente Transform de la punta del cañon.")]
    public Transform canyonTipTransform;

    [Header("Canyon Properties")]
    [Range(0, 10)]
    [Tooltip("Indica el radio de deteccion del cañon para captar el proyectil.")]
    public float detectionRadius = 0.5f;
    [Range(-10, 10)]
    [Tooltip("Indica el desface en 'Y' del radio de detección del cañon.")]
    public float offsetY = 1f;
    [Range(-10, 10)]
    [Tooltip("Indica el desface en 'X' del radio de detección del cañon.")]
    public float offsetX = 0f;

    [Space]
    [Range(0, 5000)]
    [Tooltip("Indica la fuerza con la que disparará el cañon.")]
    public float shootForce = 1000f;

    [Space]
    [Range(0, 200)]
    [Tooltip("Indica el tiempo de espera antes de disparar. (Depende de la cantidad de actualizaciones, no es el tiempo en segundos)")]
    public int triggerTime = 100;

    [Space]
    [Tooltip("El cañon detectara objetos solo presentes en este layer.")]
    public LayerMask layerMask;

    private bool canShoot = false;
    private string nombreBala = "";
    private int initCont;
    private float initGrav=0;
    private bool itsGrabbed = false;
    private float angle = 0f;
    
    private void Start()
    {
        initCont = triggerTime;
    }
    private void FixedUpdate()
    {
        checkArround();
        if (canShoot)
        {
            shoot();
        }
        
    }

    private void checkArround()
    {
        if (!canShoot)
        {
            if (canyonRigidbody2D == null) { return; }
            Vector3 centro = new Vector3(canyonTransform.position.x + (canyonTransform.localScale.x / offsetX), canyonTransform.position.y + (canyonTransform.localScale.y / offsetY), canyonTransform.position.z);
            Collider2D bala = Physics2D.OverlapCircle(centro, detectionRadius, layerMask);

            if (bala != null)
            {
                initGrav = GameObject.Find(bala.name).GetComponent<Rigidbody2D>().gravityScale;
                
                GameObject.Find(bala.name).GetComponent<Rigidbody2D>().gravityScale = 0;
                GameObject.Find(bala.name).GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                canShoot = true;
                nombreBala = bala.name;
            }
        }
    }

    private void shoot()
    {
        if (triggerTime > 0)
        {
            triggerTime -= 1;
        }

        if (triggerTime <= 50)
        {
            Vector2 centro = new Vector2(canyonRigidbody2D.position.x, canyonRigidbody2D.position.y);
            Vector2 puntaCañ = new Vector2(canyonTipTransform.position.x, canyonTipTransform.position.y);
            
            Vector3 direction = puntaCañ - centro;
            GameObject.Find(nombreBala).GetComponent<Rigidbody2D>().AddForce(direction.normalized* shootForce);
            GameObject.Find(nombreBala).GetComponent<Rigidbody2D>().gravityScale = initGrav;
        }

        if (triggerTime <= 0)
        {
            triggerTime = initCont;
            canShoot = false;
        }

    }

    

    private void OnDrawGizmosSelected()
    {
        if (canyonTransform==null) { return; }
        Vector3 centro = new Vector3(canyonTransform.position.x + (canyonTransform.localScale.x / offsetX), canyonTransform.position.y + (canyonTransform.localScale.y / offsetY), canyonTransform.position.z);
        Gizmos.DrawWireSphere(centro, detectionRadius);
    }

    

    private void OnMouseDown()
    {
        itsGrabbed = true;
    }

    private void OnMouseUp()
    {
        itsGrabbed = false;
    }


    public float getAngle() 
    { 
        return this.angle; 
    }

    public void setAngle(float angle)
    {
        this.angle = angle;
    }


}
