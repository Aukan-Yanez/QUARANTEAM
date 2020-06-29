using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LevelVisualizer : MonoBehaviour
{
    public Rigidbody2D initialPoint;
    public Transform finalPoint;
    public enum Direction { Horizontal, Vertical}
    public Direction direction = Direction.Horizontal;
    [Range(0,2)]
    public float speed = 0.05f;
    private float pSpeed = 0;
    private float sentido;
    void Start()
    {
        initialPoint = gameObject.GetComponent<Rigidbody2D>();

        if (direction == Direction.Horizontal)
        {
            if (initialPoint.position.x > finalPoint.position.x)
            {
                sentido = -1;
            }
            else
            {
                sentido = 1;
            }
        }
        if (direction == Direction.Vertical)
        {
            if (initialPoint.position.y > finalPoint.position.y)
            {
                sentido = -1;
            }
            else
            {
                sentido = 1;
            }
        }
        pSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpDown();
        MoveRightleft();
    }

    private void MoveUpDown()
    {
        if (direction == Direction.Vertical)
        {
            if (sentido < 0)
            {
                if (initialPoint.position.y > finalPoint.position.y)
                {
                    pSpeed = Mathf.Abs(pSpeed) * sentido;
                    initialPoint.position = new Vector2(initialPoint.position.x, initialPoint.position.y + pSpeed);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if (initialPoint.position.y < finalPoint.position.y)
                {
                    pSpeed = Mathf.Abs(pSpeed) * sentido;
                    initialPoint.position = new Vector2(initialPoint.position.x, initialPoint.position.y + pSpeed);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    private void MoveRightleft()
    {
        if (direction == Direction.Horizontal)
        {
            if (sentido<0)
            {
                if (initialPoint.position.x > finalPoint.position.x)
                {
                    pSpeed = Mathf.Abs(pSpeed) * sentido;
                    initialPoint.position = new Vector2(initialPoint.position.x + pSpeed, initialPoint.position.y);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if (initialPoint.position.x < finalPoint.position.x)
                {
                    pSpeed = Mathf.Abs(pSpeed) * sentido;
                    initialPoint.position = new Vector2(initialPoint.position.x + pSpeed, initialPoint.position.y);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

}
