using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    // Start is called before the first frame update
    public Rigidbody2D ayuda;
    public Transform attackPoint;
    public Transform metaPoint;
    public float attackRange = 0.5f;
    public Transform halo;
    public LayerMask playerLayers;
    public Rigidbody2D[] playerList;

    public float tork = 2f;

    private GameObject uno;
    private float blackHoleForce;
    private float angle = 360f;
    //private bool angleDefined = false;
    void Start()
    {
        blackHoleForce = attackRange;
        //uno = GameObject.Find("player");
        //Debug.Log(uno.name);
    }

    // Update is called once per frame
    void Update()
    {
        checkcheckAround();
        halo.localScale = new Vector2(attackRange, attackRange);

    }

   
    void addTorke()
    {
        float turn = 1f;
        ayuda.AddTorque(tork * turn);
    }

    private void FixedUpdate()
    {
        
    }

    private void checkcheckAround()
    {
        Collider2D[] closePlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        //(Para cada collider en el rango)
        foreach(Collider2D player in closePlayers)
        {
            attract();
            
        }
    }

    private void attract()
    {
        float positiveRangeX = attackPoint.position.x + attackRange;
        float negativeRangeX = attackPoint.position.x - attackRange;
        float positiveRangeY = attackPoint.position.y - attackRange;
        float negativeRangeY = attackPoint.position.y + attackRange;

        //(Para cada RigidBody en el rango)
        for (int i=0; i<playerList.Length; i++)
        {
            float playerPosX = playerList[i].position.x;
            float playerPosY = playerList[i].position.y;
            if (playerPosX > negativeRangeX && playerPosX < positiveRangeX && playerPosY > positiveRangeY && playerPosY < negativeRangeY)
            {
                //playerList[i].velocity = new Vector3(0, 0, 0);
                playerList[i].gravityScale = 0;
                
                /*
                if (angleDefined == false)
                {
                    angleDefined = true;
                    defineAngle(playerList[i]);
                    
                }

                changeDirection(playerList[i]);*/
            }
        }
    }

    private void changeDirection(Rigidbody2D player)
    {
        
        float radians = angle * (Mathf.PI / 180);
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        Debug.Log("Nuevo angulo2: " + angle);
        Debug.Log("Pos2(" + player.position.x + ", " + player.position.y + ")");
        //player.velocity = new Vector2(x * blackHoleForce, y * blackHoleForce);
        //Debug.Log("Pos(" + x + ", " + y + ")");
        //Debug.Log("Angle: " + angle);


        player.position = new Vector3(metaPoint.position.x + (x * blackHoleForce), metaPoint.position.y + (y * blackHoleForce),0);
        //player.angularVelocity = angle;

        angle = angle - 1f;
        blackHoleForce = blackHoleForce - 0.005f;

        if (angle <= 0)
        {
            angle = 360f;
        }

        if(blackHoleForce <= 0)
        {
            blackHoleForce = 0.005f;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void defineAngle(Rigidbody2D player)
    {
        float Ax = player.position.x;
        float Ay = player.position.y;

        float Bx = metaPoint.position.x;
        float By = metaPoint.position.y;

        float co = Ax-Bx;
        float ca = Ay-By;

        float hip = Mathf.Pow( (Mathf.Pow(co,2) + Mathf.Pow(ca, 2)), 0.5f);

        if (Ay > By)
        {
            float mycalcInRadians = Mathf.Pow(Mathf.Cos((ca / hip)), -1);
            angle = mycalcInRadians * (180 / Mathf.PI);
        }
        else
        {
            float mycalcInRadians = Mathf.Pow(Mathf.Cos((ca / hip)), -1);
            angle = 180f + (mycalcInRadians * (180 / Mathf.PI));
        }

        Debug.Log("Nuevo angulo: " + angle);
        Debug.Log("Pos(" + Ax + ", " + Ay + ")");

    }

}
