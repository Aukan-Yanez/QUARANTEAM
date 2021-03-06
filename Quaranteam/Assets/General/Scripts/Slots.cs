﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public Text objectsCounter;
    public string message = "COUNTER: ";
    public enum Mode { Iterative, Linear }
    [Tooltip("Iterative: inicia solo con el primer elemento de la lista activado. Luego va descontando el objeto actual si este muere y activando el siguiente según el orden en la lista." +
             "Linear: inicia con todos los elementos de la lista activados. Luego va descontando todos los objetos que esten muertos")]
    public Mode disposeMode = Mode.Iterative;
    private GameObject[] slots = new GameObject[10];

    public GameObject vida1;
    public GameObject vida2;
    public GameObject vida3;

    public Sprite charizard;
    public Sprite iceClimber;
    public Sprite pikachu;
    

    private int indiceObjective = 0;

    public GameObject[] projectilePrefabs;
    public Transform initPos;
 
    private int lifeCount = 3;

    public bool followCam = true;

    void Start()
    {
        //setUp();
        /*if (objectsCounter != null)
        {
            objectsCounter.text = message + slots.Length.ToString();
        }*/

        
        slots = new GameObject[lifeCount];

        int actCharacter = PlayerPrefs.GetInt("character");
        if(actCharacter == 0)
        {
            vida1.GetComponent<Image>().sprite = charizard;
            vida2.GetComponent<Image>().sprite = charizard;
            vida3.GetComponent<Image>().sprite = charizard;
        }
        if (actCharacter == 1)
        {
            vida1.GetComponent<Image>().sprite = iceClimber;
            vida2.GetComponent<Image>().sprite = iceClimber;
            vida3.GetComponent<Image>().sprite = iceClimber;
        }
        if (actCharacter == 2)
        {
            vida1.GetComponent<Image>().sprite = pikachu;
            vida2.GetComponent<Image>().sprite = pikachu;
            vida3.GetComponent<Image>().sprite = pikachu;
        }

        initiateCharacter();

    }

    // Update is called once per frame
    void Update()
    {
        iterativeMode();
    }


    private void initiateCharacter()
    {
        if (projectilePrefabs.Length > 0 && initPos != null)
        {
            if (PlayerPrefs.HasKey("character") == false)
            {
                PlayerPrefs.SetInt("character", 0);
            }
            else
            {
                GameObject instancia = projectilePrefabs[PlayerPrefs.GetInt("character")];
                
                for(int i=0; i<lifeCount; i++)
                {
                    Debug.Log("Guardando instancias");
                    GameObject i1 = Instantiate(instancia, initPos.position, initPos.rotation);
                    if (i!=0)
                    {
                        i1.SetActive(false);
                        slots[i] = i1;
                        if (followCam)
                        {
                            GameObject.FindObjectOfType<CameraManager>().focusOnList[i+1] = i1.transform;
                        }
                    }
                    else
                    {
                        slots[i] = i1;
                        if (followCam)
                        {
                            GameObject.FindObjectOfType<CameraManager>().focusOnList[i+1] = i1.transform;
                        }
                    }
                }
            }
        }
    }

   

    private void iterativeMode()
    {
        if (indiceObjective < slots.Length && slots.Length>0)
        {
            //Si el objeto actual esta desactivado
            if (!slots[indiceObjective].gameObject.activeSelf)
            {
                indiceObjective += 1;
                if (indiceObjective < slots.Length)//si el indice esta en el rango de la lista
                {
                    //Activa el siguiente proyectil
                    Debug.Log("Activando el siguiente");
                    slots[indiceObjective].gameObject.SetActive(true);
                }
                if (objectsCounter != null)
                {
                    //objectsCounter.text = message + (slots.Length - indiceObjective).ToString();
                    int vida = slots.Length - indiceObjective;
                    if(vida == 2)
                    {
                        Debug.Log("Le quedan 2 Vidas");
                        vida3.SetActive(false);
                    }
                    if(vida == 1)
                    {
                        Debug.Log("Le quedan 1 Vidas");
                        vida2.SetActive(false);
                    }
                    if (vida == 0)
                    {
                        Debug.Log("Le quedan 0 Vidas");
                        vida1.SetActive(false);
                    }
                }
            }
        }
    }

    public int getVidas()
    {
        return slots.Length - indiceObjective;
    }
    public GameObject[] getSlots()
    {
        return this.slots;
    }


    /*
    #region Old
    private void linearMode()
    {
        indiceObjective = 0;
        foreach (GameObject rb in slots )
        {
            //Si el objeto actual esta desactivado
            if (!rb.gameObject.activeSelf)
            {
                //Debug.LogWarning("Destruyendo objeto (" + rb.name + ").");
                //Destroy(rb, 200);
                indiceObjective += 1;
            }
        }
        if (objectsCounter != null)
        {
            objectsCounter.text = message + (slots.Length - indiceObjective).ToString();
        }
    }


    private void setUp()
    {
        if (disposeMode == Mode.Iterative)
        {
            iterativeSetUp();
        }
        else
        {
            linearSetUp();
        }
    }

    private void iterativeSetUp()
    {
        //Por cada proyectil
        for (int i = 0; i < slots.Length; i++)
        {
            //Desactiva al proyectil
            slots[i].gameObject.SetActive(false);
        }
        //Se deja activado el primer proyectil
        slots[0].gameObject.SetActive(true);
    }

    private void linearSetUp()
    {

    }

    private void mode()
    {
        if (disposeMode == Mode.Iterative)
        {
            iterativeMode();
        }
        else
        {
            linearMode();
        }
    }
    #endregion

    ¨*/
}
