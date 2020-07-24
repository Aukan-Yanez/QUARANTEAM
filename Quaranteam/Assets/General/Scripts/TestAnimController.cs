using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class TestAnimController : MonoBehaviour
{
    //public Animations animations;
    private Rigidbody2D rigidbody2D;
    private Vector2 force;
    private bool isJumping = false;
    private Animator animator;
    public GameObject[] ground;

    [Range(0, 100)]
    public float gravityScale = 2;
    [Range(0, 100)]
    public float mass = 5;
    [Range(0, 1000)]
    public float forceMagnitude = 100;
    public Vector2 forceDirection = new Vector2(0, 1);

    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.mass = mass;
        rigidbody2D.gravityScale = gravityScale;
        force = forceDirection.normalized * forceMagnitude;
        animator = gameObject.GetComponent<Animator>();
    }

    
    
    void Update()
    {
        checkGround();
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            rigidbody2D.position = new Vector2(rigidbody2D.position.x, rigidbody2D.position.y+0.5f);
            rigidbody2D.AddForce(force);
            isJumping = true;
        }

    }

    private void checkGround()
    {
        if (isJumping)
        {
            foreach (GameObject place in ground)
            {
                Collider2D hasCollider = place.GetComponent<Collider2D>();
                if (hasCollider)
                {
                    if (rigidbody2D.IsTouching(hasCollider))
                    {
                        isJumping = false;
                        animator.SetBool("isJumping", false);
                    }
                }
            }
        }
        else
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("Normal", true);
        }
    }


}
