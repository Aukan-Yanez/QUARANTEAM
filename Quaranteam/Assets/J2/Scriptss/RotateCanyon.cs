using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCanyon : MonoBehaviour
{
    [Header("Canyon Components")]
    [Tooltip("Canyon script. (Debe arrastrar el objeto 'X' específico que contiene el Script para poder rotarlo.) (NO debe arrastrar el script original ya que no esta asociado a algún objeto en particular.)")]
    public Canyon canyon;
    [Tooltip("Componente Transform del cañon.")]
    public Transform canyonTipTransform;
    [Tooltip("Componente Rigidbody2D del cañon.")]
    public Rigidbody2D canyonRigidbody2D;
    [Tooltip("Componente Rigidbody2D del boton.")]
    public Rigidbody2D buttonRigidbody2D;


    [Header("Canyon Properties")]
    [Range(-9, 9)]
    [Tooltip("Indica lo rápido que gira el cañon. (Positivo o negativo indica el sentido de giro)")]
    public int rotationValue=1;

    [Space]
    [Range(0, 3)]
    [Tooltip("Indica el desplazamiento en 'X' del botón.")]
    public float offsetX = 1;
    [Range(0, 3)]
    [Tooltip("Indica el desplazamiento en 'Y' del botón.")]
    public float offsetY = 1;

    [Header("Button Properties")]
    [Range(-9, 9)]
    [Tooltip("Indica lo rápido que gira la punta del cañon.(Positivo o negativo indica el sentido de giro). (se debe adecuar para que concuerde con el giro del cañon)")]
    public float tipRotationValue = 1f;
    [Range(0, 9)]
    [Tooltip("Indica el radio de giro con respecto al centro del cañon.")]
    public float tipRadius = 1.8f;

    private bool itsGrabbed = false;
    void Update()
    {
        if (itsGrabbed)
        {
            canyonRigidbody2D.rotation += rotationValue;
            rotate();
        }

        buttonRigidbody2D.position = canyonRigidbody2D.position - new Vector2(offsetX, offsetY);
    }


    private void OnMouseDown()
    {
        itsGrabbed = true;
        canyonRigidbody2D.isKinematic = true;
    }

    private void OnMouseUp()
    {
        itsGrabbed = false;
        canyonRigidbody2D.isKinematic = false;
    }


    public void rotate()
    {
        float radians = canyon.getAngle() * (Mathf.PI / 180);
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);
        canyonTipTransform.position = new Vector3(canyonRigidbody2D.position.x + (x * tipRadius), canyonRigidbody2D.position.y + (y * tipRadius), 0);

        
        canyon.setAngle(canyon.getAngle() + tipRotationValue);

        if (canyon.getAngle() > 360) { canyon.setAngle(0); }
    }

}
