using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera maincam;
    public Cinemachine.CinemachineVirtualCamera cam;
    public bool followDefaultObject = true;
    public Transform defaultObjectTransform;
    public Transform background;
    [Range(0,100)]
    public float backgroundVelocityScale = 1f;
    public Transform[] focusOnList;
    public Transform[] enemyList;
    [Range(0,20)]
    public float minVelToApplyZoom = 5;
    [Range(0, 200)]
    public float smoothedOut = 5;
    [Range(0, 15)]
    public float maxZoom = 8;
    [Range(0, 15)]
    public float minZoom = 5;
    [Range(0, 2)]
    public float zoomSmoothedOut = 0.07f;
    [Range(0, 2)]
    public float zoomSmoothedIn = 0.1f;

    
    private int i = 0;
    private bool once = false;
    private float prevPosition = 0;
    private float currPosition = 0;
    private float touchPrevDist = 0;
    private float touchCurrDist = 0;
    void Start()
    {
        prevPosition = cam.transform.position.x;
        currPosition = cam.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (followDefaultObject || focusOnList.Length==0 )
        {
            focus();
        }
        else
        {
            checkFocus();
        }

        adaptToVelocity();
        moveBackgorundToo();
    }

    private void checkFocus()
    {
        if (focusOnList[i].gameObject.activeSelf)
        {
            cam.Follow = focusOnList[i];
        }
        else
        {
            if (i < focusOnList.Length - 1)
            {
                i += 1;
                cam.Follow = focusOnList[i];
            }
            if(i==focusOnList.Length-1 && focusOnList[i].gameObject.activeSelf==false)
            {
                cam.Follow = defaultObjectTransform;
            }
        }
    }

    private void focus()
    {
        if (!once)
        {
            cam.Follow = defaultObjectTransform;
            once = true;
        }
    }

    private void adaptToVelocity()
    {
        if(cam.Follow.gameObject.GetComponent<Rigidbody2D>() != null)
        {

            int velocAbs = Mathf.Abs(Mathf.RoundToInt(cam.Follow.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude));
            

            if (velocAbs > minVelToApplyZoom)
            {
                if (cam.m_Lens.OrthographicSize < maxZoom)
                {
                    cam.m_Lens.OrthographicSize += zoomSmoothedOut;
                }
            }
            
            if (velocAbs < minVelToApplyZoom)
            {
                if (cam.m_Lens.OrthographicSize > minZoom)
                {
                    cam.m_Lens.OrthographicSize -= zoomSmoothedIn;
                }
            }
        }
    }


    private void moveBackgorundToo()
    {
        if (background != null)
        {
            currPosition = cam.transform.position.x;

            if (currPosition > prevPosition)
            {
                background.position = new Vector2(background.position.x + (prevPosition - currPosition) * backgroundVelocityScale / 100, background.position.y);
            }
            if (currPosition < prevPosition)
            {
                background.position = new Vector2(background.position.x + (prevPosition - currPosition) * backgroundVelocityScale / 100, background.position.y);
            }

            prevPosition = currPosition;
        }
    }


    private void scroll()
    {
        Touch[] touches = Input.touches;
        if (Input.touchCount == 2)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (touchCurrDist == 0 && touchPrevDist == 0)
                {
                    touchCurrDist = (touches[0].position - touches[1].position).magnitude;
                    touchPrevDist = (touches[0].position - touches[1].position).magnitude;
                }
                else
                {
                    touchCurrDist = (touches[0].position - touches[1].position).magnitude;
                    if (touchCurrDist < touchPrevDist)
                    {
                        //menos zoom (se aleja la camara)
                        if (cam.m_Lens.OrthographicSize < maxZoom)
                        {
                            cam.m_Lens.OrthographicSize += zoomSmoothedOut;
                        }
                        touchPrevDist = touchCurrDist;
                    }
                    if (touchCurrDist > touchPrevDist)
                    {
                        //más zoom (se acerca la camara)
                        if (cam.m_Lens.OrthographicSize > minZoom)
                        {
                            cam.m_Lens.OrthographicSize -= zoomSmoothedIn;
                        }
                        touchPrevDist = touchCurrDist;
                    }
                }
            }
        }
        
    }

}
