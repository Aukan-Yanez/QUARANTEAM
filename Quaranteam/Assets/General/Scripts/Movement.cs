using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    [Range(0,100)]
    public float velocidadHorizontal = 0f;
    [Range(0, 100)]
    public float velocidadVertical = 0f;

    private float ha=0;
    private float va = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ha = Input.GetAxisRaw("Horizontal") * velocidadHorizontal;
        va = Input.GetAxisRaw("Vertical") * velocidadVertical;
    }

    private void FixedUpdate()
    {
        ha = ha * Time.fixedDeltaTime;
        va = va * Time.fixedDeltaTime;
        playerRigidbody.position = new Vector2(playerRigidbody.position.x + ha, playerRigidbody.position.y + va);
    }
}
