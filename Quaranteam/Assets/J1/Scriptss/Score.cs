using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Enemy[] enemyList;
    public Text scoreBoard;
    public int score=0;

    // Start is called before the first frame update
    void Start()
    {
        scoreBoard.text = "SCORE  0";
    }

    // Update is called once per frame
    void Update()
    {
        checkScore();
    }

    private void checkScore()
    {
        int currentScore = countDeadEnemies();
        if (currentScore > score)
        {
            score = currentScore;
            scoreBoard.text = "SCORE  " + score.ToString();
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

}
