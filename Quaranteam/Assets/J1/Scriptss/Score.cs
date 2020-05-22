using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Enemy[] enemyList;
    public Text scoreBoard;
    public int score=0;

    public BoxCollider2D bounds;
    public Transform center;
    public float scale;

    private Rigidbody2D[] enemyListRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        //scoreBoard.text = "SCORE  0";
    }

    // Update is called once per frame
    void Update()
    {
        checkScore();
        setInvisible();
    }

    private void checkScore()
    {
        int currentScore = countDeadEnemies();
        if (currentScore > score)
        {
            score = currentScore;
            //scoreBoard.text = "SCORE  " + score.ToString();
        }
    }

    private int countDeadEnemies()
    {
        int currentScore = 0;
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (!enemyList[i].itsAlive)
            {
                currentScore++;
            }
        }
        return currentScore;
    }

    private void setInvisible()
    {

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (!enemyList[i].itsAlive)
            {
                Transform center = GameObject.Find("floor").GetComponent<Transform>();
                BoxCollider2D size = GameObject.Find("floor").GetComponent<BoxCollider2D>();
                Transform enemy = GameObject.Find(enemyList[i].name).GetComponent<Transform>();
                float bottom = enemy.position.y - (enemy.localScale.y / 2);
                float top = center.position.y + (center.localScale.y / 2);
                if (bottom <= top )
                {
                    GameObject.Find(enemyList[i].name).GetComponent<SpriteRenderer>().enabled = false;
                }

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (center == null || bounds == null)
            return;
        Gizmos.DrawCube(center.position, new Vector3(bounds.size.x + scale, bounds.size.y + scale, 0) );
    }




}
