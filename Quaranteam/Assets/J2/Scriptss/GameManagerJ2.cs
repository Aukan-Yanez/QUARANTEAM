using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerJ2 : MonoBehaviour
{
    [Header("Level Settings")]
    public string level;
    public float time = 60f;
    public GameObject playerrr;
    public int nextLevelIndex;
    [Header("Music")]
    private AudioSource dropStar;
    [HideInInspector]
    AudioSource lossMusic;
    [Header("Text End Level")]
    public float secondsTextEnd = 3.0f;
    public GameObject winnerText;
    public GameObject loserText;

    [HideInInspector] public int cantPoints;

    [HideInInspector] private bool timer = true;

    private void Awake()
    {
        dropStar = GetComponent<AudioSource>();
        
    }

    public float GetTime()
    {
        return time;
    }


    public void CapturedCoins(int points)
    {
        dropStar.Play();
        cantPoints += points;
    }
    
    public int GetPoints()
    {
        return cantPoints;
    }

    IEnumerator winGame()
    {
        winnerText.SetActive(true);
        yield return new WaitForSeconds(secondsTextEnd);
        
        SceneManager.LoadScene(nextLevelIndex);
    }

    IEnumerator lostGame()
    {
        loserText.SetActive(true);
        yield return new WaitForSeconds(secondsTextEnd);
        loserText.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void updateScore()
    {
        string nameKey = "lvl" + level;
        Debug.Log("GM "+ PlayerPrefs.HasKey(nameKey));
        if (PlayerPrefs.HasKey(nameKey))
        {
            if (PlayerPrefs.GetInt(nameKey) < cantPoints)
            {
                PlayerPrefs.SetInt(nameKey, cantPoints);
            }
        }
        else
        {
            PlayerPrefs.SetInt(nameKey, cantPoints);
        }
        Debug.Log(nameKey + " " + PlayerPrefs.GetInt(nameKey).ToString());
    }

    public void Win()
    {
        updateScore();
        StartCoroutine("winGame");
    }

    public void Lost()
    {
        StartCoroutine("lostGame");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        cantPoints = 0;
        if (time == 0f)
        {
            timer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer)
        {
            time -= Time.deltaTime;
        }
        if(playerrr.activeInHierarchy == false)
        {
            Lost();
        }

        if (time < 0.0f)
        {
            playerrr.SetActive(false);
        }
    }
}
