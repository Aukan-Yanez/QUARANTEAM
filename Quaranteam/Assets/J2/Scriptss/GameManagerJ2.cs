using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerJ2 : MonoBehaviour
{
    public int cantPoints;
    // Start is called before the first frame update
    void Start()
    {
        cantPoints = 0;
    }


    public void CapturedCoins(int points)
    {
        cantPoints += points;
    }
    
    public int GetPoints()
    {
        return cantPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
