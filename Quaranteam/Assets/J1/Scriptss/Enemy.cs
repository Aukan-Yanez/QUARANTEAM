using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public int lifePoints = 1;
    public Rigidbody2D enemy;
    private float initPosX;
    private float initPosY;
    public bool itsAlive = true;
    public string namee;
    // Start is called before the first frame update
    void Start()
    {
        initPosX = enemy.position.x;
        initPosY = enemy.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if ( itsMoved() )
        {
            itsAlive = false;
            //Debug.Log(namee + ", he muerto");
        }
    }

    private bool itsMoved()
    {
        float xPositiveRange = initPosX + 1;
        float xNegativeRange = initPosX - 1;
        float yPositiveRange = initPosY + 1;
        float yNegativeRange = initPosY - 1;

        float currentEnemyX = enemy.position.x;
        float currentEnemyY = enemy.position.y;

        if (currentEnemyX < xNegativeRange || currentEnemyX > xPositiveRange || currentEnemyY > yPositiveRange || currentEnemyY < yNegativeRange)
        {
            return true;
        }
        return false;
    }
}
