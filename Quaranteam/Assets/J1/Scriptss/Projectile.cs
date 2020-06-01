using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    public bool isActive = true;
    public bool canBurn = false;
    public bool canFreeze = false;
    public bool canExplode = true;
    [Tooltip("Propiedad aún no implementada")]
    public bool canElectrocute = false;
    public bool canRemoveGravity = false;
    public bool canRotate = false;
    [Tooltip("Propiedad aún no implementada")]
    public bool canEnlarge = false;
    public bool disappear = false;


    [Header("Properties Values")]
    [Range(0,10)] 
    [Tooltip("Indica cuanto de vida le resta al objeto afectado por el fuego en cada frame. (Impelmentado parcialmente)")]
    public float burningDamage = 2f;

    [Space]
    [Range(0, 100)] 
    [Tooltip("Indica cuanto porcentaje de vida le resta al objeto afectado por el hielo en el primer golpe del proyectil.")]
    public float freezeDamagePercent = 0f;
    [Range(0, 100)]
    [Tooltip("Indica cuanto daño de vida le resta al objeto afectado por el hielo en cada frame.")]
    public float freezeDamage = 0f;
    [Range(0, 500)]
    [Tooltip("Indica cuanto se demorará en congelarse el objeto afectado por el hielo.")]
    public float freezingTime = 0f;

    [Space]
    [Range(0, 3000)]
    [Tooltip("Rango mínimo de fuerza explosiva.")]
    public float explotionForceMin = 100f;
    [Range(0, 3000)]
    [Tooltip("Rango máximo de fuerza explosiva")]
    public float explotionForceMax = 200f;
    [Range(0, 100)]
    [Tooltip("Indica cuanto de vida le resta al objeto afectado por la explosión en cada frame.")]
    public float explotionDamage = 30f;

    [Space]
    [Range(0, 1000)]
    [Tooltip("Indica cuanto rotará el objeto afectado por la rotación.")]
    public float rotationForce = 10f;

    [Space]
    [Range(0, 1000)]
    [Tooltip("Indica la proporcion de crecimiento del ¿proyectil?. (Aún no implementado)")]
    public float enlargeSize = 0f;

    


}
