using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float time;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        time = GameObject.Find("GameManager").GetComponent<GameManagerJ2>().GetTime();
    }
    // Update is called once per frame
    void Update()
    {
        if (time > 0.0f)
        {
            time -= Time.deltaTime;
        }
        timerText.text = "" + time.ToString("f0");
    }
}
