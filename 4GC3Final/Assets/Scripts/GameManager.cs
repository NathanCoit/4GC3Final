using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    private int Player1Score = 0;
    private int Player2Score = 0;

    //Sound man has finally blessed your scripts. Praise him. Love him;
    private SoundManager SoundMan;

    private MenuManager MenuMan;

    private bool Resetting = false;
    // Start is called before the first frame update
    void Start()
    {
        //Setting up ya bois
        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        MenuMan = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();

        //Do this for inital round start sounds and ui and stuff
        StartCoroutine(DelayedResetCoroutine(0.1f));
        Player1.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.TimedOut;
        Player2.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.TimedOut;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerLost(string Player)
    {
        if(!Resetting)
        {
            Resetting = true;
            if (Player == "P1")
            {
                Player2Score++;
            }
            else
            {
                Player1Score++;
            }
            DelayedGameReset();
        }   
    }

    public void DelayedGameReset()
    {
        StartCoroutine(DelayedResetCoroutine(3));
    }

    private IEnumerator DelayedResetCoroutine(float seconds)
    {
        //STOP
        Player1.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.TimedOut;
        Player2.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.TimedOut;

        yield return new WaitForSeconds(seconds);
        Player1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Player2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Player1.transform.position = new Vector3(-4, 0.5f, 0);
        Player2.transform.position = new Vector3(4, 0.5f, 0);

        Camera.main.GetComponent<CombatCam>().resetCamera();

        //Announcer and text anim
        SoundMan.playReady();
        MenuMan.ReadyAnimation();
        yield return new WaitForSeconds(1);
        SoundMan.playBegin();
        MenuMan.BeginAnimation();

        yield return new WaitForSeconds(0.5f);

        //Give control after begin has been said
        Player1.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.None;
        Player2.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.None;

        Resetting = false;
    }
}
