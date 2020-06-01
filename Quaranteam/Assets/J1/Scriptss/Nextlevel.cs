using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextlevel : MonoBehaviour
{
    private ScoreD score;
    private bool canGoNextLevel = false;
    [Range(0,10)]
    public int pointsToWin = 1;
    public int nextSceneIndex = 2;
    //public string nextSceneName = "J1.2";
    
    [Range(0, 200)]
    public int delayTime = 20;
    // Start is called before the first frame update
    void Start()
    {
        ScoreD sd = GameObject.FindObjectOfType<ScoreD>();
        if(sd != null)
        {
            score = sd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkScore();
        if (canGoNextLevel)
        {
            loadScene();
        }
    }

    private void checkScore()
    {
        if(score.getScore() >= pointsToWin)
        {
            Scene nextScene = SceneManager.GetSceneByBuildIndex(nextSceneIndex);
            if (nextScene != null)
            {
                canGoNextLevel = true;
            }
            else
            {
                Debug.LogWarning("El nombre de la escena especificado no corresponde a alguna escena en el 'Build'.");
            }
        }
    }


    private void loadScene()
    {
        delayTime -= 1;
        if (delayTime<=0)
        {
            canGoNextLevel = false;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}
