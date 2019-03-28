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

    private bool follow;

    private GameObject targetObject;

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
        follow = false;
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
        if(follow)
        {
            transform.LookAt(targetObject.transform);

            //Should rotate around target but does not =(
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }


    public void resetCamera()
    {
        follow = false;
        lookAt(new Vector3(0, 8, -11));
        transform.LookAt(GameObject.FindGameObjectWithTag("Arena").transform);
        transform.eulerAngles = new Vector3(30, 0, 0);
    }

    //Function for setting camera target to unit when its selected
    public void lookAt(Vector3 target)
    {
        //Did some trig. This adjacent places the target in the middle of the camera
        float adjacent = 5;
        cameraTarget = new Vector3(target.x, target.y, target.z - adjacent);
        cameraMoving = true;
    }

    public void setTarget(GameObject target)
    {
        follow = true;
        targetObject = target;
    }

}
