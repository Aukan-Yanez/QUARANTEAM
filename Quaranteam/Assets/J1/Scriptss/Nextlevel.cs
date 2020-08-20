using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Nextlevel : MonoBehaviour
{
    private ScoreD score;
    private bool existsLevel = false;
    private bool canChangeLevel = false;
    [Range(0,10)]
    public int pointsToWin = 1;
    public int nextSceneIndex = 2;
    public GameObject victoria;
    public GameObject derrota;
    public ProjectileTwo projectile;

    private bool perder = false;

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
        loadScene();
        
        //checkForVictory();
    }

    private void checkForVictory()
    {
        if(victoria && derrota && projectile && score.getScore() >= pointsToWin)
        {
            if (projectile.isDead == 1)
            {
                StartCoroutine("waitForWinner");
            }
            if (projectile.isDead == 2)
            {
                StartCoroutine("waitForLoser");
                
            }
        }
        if (!victoria && !derrota && !projectile && score.getScore() >= pointsToWin)
        {
            canChangeLevel = true;
        }
    }


    IEnumerator waitForWinner()
    {
        victoria.SetActive(true);
        yield return new WaitForSeconds(2);
        canChangeLevel = true;
    }

    IEnumerator waitForLoser()
    {
        derrota.SetActive(true);
        yield return new WaitForSeconds(2);
        nextSceneIndex = 0;
        canChangeLevel = true;
    }


    private void checkScore()
    {
        if(score.getScore() >= pointsToWin)
        {
            Scene nextScene = SceneManager.GetSceneByBuildIndex(nextSceneIndex);
            if (nextScene != null)
            {
                existsLevel = true;
            }
            else
            {
                Debug.LogWarning("El nombre de la escena especificado no corresponde a alguna escena en el 'Build'.");
            }
        }
    }


    private void loadScene()
    {
        existsLevel = false;
        LW();
    }

    private void LW()
    {
        Slots s = GameObject.FindObjectOfType<Slots>();
        if(s.getVidas() == 0 && score.getScore() < pointsToWin)
        {
            StartCoroutine("animNextLevelLose");
        }
        if (score.getScore() >= pointsToWin)
        {
            StartCoroutine("animNextLevelWin");
        }
        
    }

    IEnumerator animNextLevelLose()
    {
        derrota.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
        derrota.SetActive(false);
    }
    IEnumerator animNextLevelWin()
    {
        victoria.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(nextSceneIndex);
        victoria.SetActive(false);
    }



}
