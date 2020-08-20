using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            string nameKey = "lvl" + (i + 1).ToString();
            if (PlayerPrefs.HasKey(nameKey))
            {
                PlayerPrefs.SetInt(nameKey, 0);
            }
            else
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
