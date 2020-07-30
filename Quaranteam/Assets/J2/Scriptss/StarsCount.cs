using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarsCount : MonoBehaviour
{
    private TextMeshProUGUI starsCounterText;

    private void Awake()
    {
        starsCounterText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        starsCounterText.text = GameObject.Find("GameManager").GetComponent<GameManagerJ2>().GetPoints().ToString();
    }
}
