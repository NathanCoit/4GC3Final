using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Simple camera script, slightly modified from management mode camera script
 */
public class CombatCam : MonoBehaviour
{
    private bool start; // really awful way of avoiding organising awake/start stuff (for now!!)
    private int xBoundary;
    private int zBoundary;

    int theScreenWidth;
    int theScreenHeight;

    public bool rightHeld;
    public bool leftHeld;
    public bool downHeld;
    public bool upHeld;

    //Stuff for moving camera to units
    private Vector3 camSmoothDampV;
    public float targetTolerance;
    private Vector3 cameraTarget;
    public bool cameraMoving;

    float maxFov = 60.0f;
    float minFov = 10.0f;
    public float sensitivity = 5.0f;
    public bool CameraMovementEnabled = true;

    public float fov;

    // i tried putting this here and the boundaries in start - didn't work :(
    private void Awake()
    {
    }

    void Start()
    {
        fov = Camera.main.fieldOfView;
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
    }

    void Update()
    {

        if (cameraMoving)
        {
            if (!(System.Math.Abs(Camera.main.transform.position.x - cameraTarget.x) < targetTolerance) || !(System.Math.Abs(Camera.main.transform.position.z - cameraTarget.z) < targetTolerance))
            {
                Camera.main.transform.position = Vector3.SmoothDamp(
                    Camera.main.transform.position, cameraTarget, ref camSmoothDampV, 0.1f);
            }
            else
                cameraMoving = false;
        }
    }


    public void resetCamera()
    {
        lookAt(new Vector3(0, 8, -11));
    }

    //Function for setting camera target to unit when its selected
    public void lookAt(Vector3 target)
    {
        //Did some trig. This adjacent places the target in the middle of the camera
        float adjacent = 5;
        cameraTarget = new Vector3(target.x, target.y, target.z - adjacent);
        cameraMoving = true;
    }

}
