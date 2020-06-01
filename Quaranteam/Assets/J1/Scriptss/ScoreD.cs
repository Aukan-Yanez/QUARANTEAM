using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreD : MonoBehaviour
{
    public Text scoreBoard;
    private int score = 0;

    private void Start()
    {
        if (scoreBoard != null)
        {
            scoreBoard.text = "Score: " + score.ToString();
        }
    }

    public void addPoint()
    {
        score++;
        if (scoreBoard != null)
        {
            scoreBoard.text = "Score: " + score.ToString();
        }
    }

    public void addPoints(int points)
    {
        score = score + points;
        if (scoreBoard != null)
        {
            scoreBoard.text = "Score: " + score.ToString();
        }
    }


    public int getScore()
    {
        return score;
    }

}
