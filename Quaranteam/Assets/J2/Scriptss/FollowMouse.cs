using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowMouse : MonoBehaviour
{
    //Variables
    private bool itsGrabbed = false;
    public Rigidbody2D playerRigidBody2D;
    public string namee = "nosetyet";
    public string playername = "nosetyet";
    public string playername2 = "nosetyet";
    private int chance = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (itsGrabbed)
        {
            playerRigidBody2D.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Con esto la pelota sigue el movimiento del mouse.
        }

        if (namee == "tryagain" && itsGrabbed)
        {
            if (chance == 2)
            {
                GameObject.Find("player3").GetComponent<PlayerMovement>().reset(GameObject.Find("player").GetComponent<PlayerMovement>().getInitPos());
                GameObject.Find("player2").GetComponent<PlayerMovement>().reset(new Vector3(-200, -200, 0));
                
            }

            if (chance==3)
            {
                GameObject.Find("player2").GetComponent<PlayerMovement>().reset(GameObject.Find("player").GetComponent<PlayerMovement>().getInitPos());
                GameObject.Find("player").GetComponent<PlayerMovement>().reset(new Vector3(-200, -200, 0));
                
            }
        }

    }

    private void OnMouseDown()
    {
        itsGrabbed = true;
        playerRigidBody2D.isKinematic = true;
    }

    private void OnMouseUp()
    {
        itsGrabbed = false;
        playerRigidBody2D.isKinematic = false;
        chance--;
    }
}
