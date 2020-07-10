using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileTwo : MonoBehaviour
{
    public ComponentsPT components;
    public PropertiesPT properties;
    private float currentForceIncrease;
    private bool isPressing;
    private bool isClickingOnPlayer;
    private bool isClickingOnDirection;
    private bool wasLaunched;
    private Vector2 playerInitPos;
    // Start is called before the first frame update
    void Start()
    {
        isPressing = false;
        currentForceIncrease = -properties.forceChangeScale;
        isClickingOnPlayer = false;
        isClickingOnDirection = false;
        wasLaunched = false;
        components.forceSlider.maxValue = properties.maxForce;
        playerInitPos = components.thisObject.GetComponent<Rigidbody2D>().position;
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseUpLeft();
        OnMouseDownLeft();

        alignVector();
        //resetPlayerPosition();
        varyTheForceMagnitud();
        varyTheForceDirection();
        whoIsClicked();
        checkingForTrigger();
    }

    private void OnMouseDownLeft()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressing = true;
        }
    }
    private void OnMouseUpLeft()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isPressing = false;
        }
    }
    private void alignVector()
    {
        components.directionDraggable.transform.localPosition = Vector3.zero;
        components.directionDraggable.transform.rotation = components.direction.transform.rotation;
        components.directionTip.transform.localPosition = Vector3.zero;
    }
    private void resetPlayerPosition()
    {
        components.thisObject.GetComponent<Rigidbody2D>().position = playerInitPos;
    }
    private void varyTheForceMagnitud()
    {
        if (!isClickingOnPlayer)
        {
            components.forceSlider.value += currentForceIncrease;

            if (components.forceSlider.value >= properties.maxForce)
            {
                currentForceIncrease = -properties.forceChangeScale;
            }
            if (components.forceSlider.value <= 0)
            {
                currentForceIncrease = properties.forceChangeScale;
            }
            components.forceFill.color = properties.forceGradient.Evaluate(components.forceSlider.normalizedValue);
        }
    }
    private void varyTheForceDirection()
    {
        if (isClickingOnDirection)
        {
            components.directionDraggable.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isClickingOnDirection = false;
        }
        components.directionDraggable.gameObject.GetComponent<SpriteRenderer>().color = properties.forceGradient.Evaluate(components.forceSlider.normalizedValue);
        components.direction.gameObject.GetComponent<SpriteRenderer>().color = properties.forceGradient.Evaluate(components.forceSlider.normalizedValue);
    }
    private void whoIsClicked()
    {
        Collider2D[] clickedColliders = Physics2D.OverlapCircleAll(components.center.position, components.radius, components.layerMask);
        foreach(Collider2D collider in clickedColliders)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(collider.name);
            if (collider.OverlapPoint(mousePosition) && isPressing)
            {
                Debug.Log("Player: " + components.thisObject.name);
                Debug.Log("Vector: " + components.directionDraggable.name);
                if (collider.name == components.thisObject.name)
                {
                    isClickingOnPlayer = true;
                }
                if (collider.name == components.directionDraggable.name)
                {
                    isClickingOnDirection = true;
                }
            }
        }
    }
    private Vector2 getLaunchDirection()
    {
        Vector2 direccion = (components.directionTip.transform.position - components.thisObject.transform.position).normalized;
        //print(direccion);
        return direccion;
    }
    private void checkingForTrigger()
    {
        if (isClickingOnPlayer && !wasLaunched)
        {
            components.direction.gameObject.SetActive(false);
            components.thisObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            components.thisObject.GetComponent<Rigidbody2D>().gravityScale = properties.gravityScale;
            //getLaunchDirection() retorna la direccion de la flecha y components.forceSlider.value es la magnitud de la fuerza.
            Vector2 force = getLaunchDirection()*components.forceSlider.value*100;
            components.thisObject.GetComponent<Rigidbody2D>().AddForce(force);
            wasLaunched = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(components.center.position.x, components.center.position.y, 0), components.radius);
        if (components.directionDraggable != null && components.direction != null)
        {
            components.direction.transform.localScale = new Vector3(properties.directionHeight, properties.directionWidth, 0);
            //components.directionDraggable.transform.localScale = new Vector3(properties.directionHeight, properties.directionWidth, 0);
        }
    }




}

[System.Serializable]
public class ComponentsPT
{
    [Header("This Object:")]
    public GameObject thisObject;

    [Header("Force")]
    public Image forceFill;
    public Slider forceSlider;

    [Header("Direction")]
    public Rigidbody2D direction;
    public Rigidbody2D directionDraggable;
    public Rigidbody2D directionTip;

    [Header("Otros")]
    [Tooltip("Punto en torno al cual se detectaran clicks o toques en la pantalla dentro de un radio dado.")]
    public Rigidbody2D center;
    [Tooltip("Radio dentro del cual se detectan clicks o toques de pantalla.")]
    [Range(0, 5)]
    public float radius = 2f;
    public LayerMask layerMask;
}
[System.Serializable]
public class PropertiesPT
{
    [Range (0,100)]
    public float maxForce = 30f;
    [Range(0, 2)]
    public float forceChangeScale = 0.2f;
    

    [Range(0, 5)]
    public float directionHeight = 1.2f;
    [Range(0, 2)]
    public float directionWidth = 1f;
    public Gradient forceGradient;

    [Range(0, 500)]
    public float gravityScale = 1;
}