using System.Collections;
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
    public Rigidbody2D[] slots;
    
    private int indiceObjective = 0;
    void Start()
    {
        setUp();
        if (objectsCounter != null)
        {
            objectsCounter.text = message + slots.Length.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        iterativeMode();
    }

    private void mode()
    {
        if(disposeMode == Mode.Iterative)
        {
            iterativeMode();
        }
        else
        {
            linearMode();
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
                    slots[indiceObjective].gameObject.SetActive(true);
                }
                if (objectsCounter != null)
                {
                    objectsCounter.text = message + (slots.Length - indiceObjective).ToString();
                }
            }
        }
    }

    private void linearMode()
    {
        indiceObjective = 0;
        foreach (Rigidbody2D rb in slots )
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


}
