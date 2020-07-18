using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionVector : MonoBehaviour
{
    public Rigidbody2D directionVectorRigidbody;
    [Range(0, 10)]
    public float vectorSize = 3f;
    public Transform directionVectorTip;
    [HideInInspector]
    public Vector2 direction;


    public Transform forceBar;
    public Slider barSlider;
    public Image barFill;
    public Gradient barGradient;
    [Range(0, 100)]
    public float maxForce = 30f;
    [Range(0, 2)]
    public float forceChangeScale = 2f;
    //public LineRenderer vectorToDraw;


    private bool isPressing = false;
    private bool selected = false;
    private float currentForceIncrease;

    [Header("Vector ahora si xd")]
    public Rigidbody2D vectorToMove;
    public Rigidbody2D vectorToDrag;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //moveVectorDirection();
        changeForceMagnitude();
        resetPos();
    }

    private void resetPos()
    {
        if (isPressing == false)
        {
            vectorToDrag.transform.localPosition = Vector3.zero;
            //vectorToDrag.transform.localRotation = vectorToMove.transform.rotation;
        }
            
        //vectorToDrag.rotation = vectorToMove.rotation;
    }

    private void moveVectorDirection()
    {
        direction = (directionVectorTip.position - new Vector3(directionVectorRigidbody.position.x, directionVectorRigidbody.position.y, 0)).normalized;
        
    }

    private void changeForceMagnitude()
    {
        
        {
            //components.forceBar.localScale = new Vector2(components.forceBar.localScale.x + currentForceIncrease, components.forceBar.localScale.y);

            barSlider.value += currentForceIncrease;

            if (barSlider.value >= maxForce)
            {
                currentForceIncrease = -forceChangeScale;
            }
            if (barSlider.value <= 0)
            {
                currentForceIncrease = forceChangeScale;
            }
            barFill.color = barGradient.Evaluate(barSlider.normalizedValue);
        }
    }

    private void OnMouseDown()
    {
        isPressing = true;
    }

    private void OnMouseUp()
    {
        isPressing = false;
    }
}
