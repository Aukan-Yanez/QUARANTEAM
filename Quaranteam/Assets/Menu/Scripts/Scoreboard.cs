using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public GameObject[] scoreTexts;

    void updateScoreboard()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            string nameKey = "lvl" + (i + 1).ToString();
            Debug.Log(nameKey);
            Debug.Log(PlayerPrefs.HasKey(nameKey).ToString());
            if (PlayerPrefs.HasKey(nameKey))
            {
                scoreTexts[i].GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt(nameKey);
                Debug.Log(PlayerPrefs.GetInt(nameKey).ToString());
            }
            else
            {
                break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        updateScoreboard();
    }

    private void OnEnable()
    {
        updateScoreboard();
    }
}
