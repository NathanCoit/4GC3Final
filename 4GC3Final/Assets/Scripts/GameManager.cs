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

    /// <summary>
    /// Awake called before start. Create players and link scripts
    /// </summary>
    void Awake()
    {
        // Get chosen character names from magical place.
        string Player1Character = "Red";
        string Player2Character = "Harry";
        CreatePlayers(Player1Character, Player2Character);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Setting up ya bois
        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        MenuMan = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();

        //Do this for inital round start sounds and ui and stuff
        StartCoroutine(DelayedResetCoroutine(0.1f));
        Player1.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.Dead;
        Player2.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.Dead;
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
        Player1.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.Dead;
        Player2.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.Dead;

        yield return new WaitForSeconds(seconds);
        Player1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Player2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Player1.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        Player2.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

        Player1.transform.position = new Vector3(-4, 0.5f, 0);
        Player2.transform.position = new Vector3(4, 0.5f, 0);

        //Announcer and text anim
        SoundMan.playReady();
        MenuMan.ReadyAnimation();
        yield return new WaitForSeconds(1);
        SoundMan.playBegin();
        MenuMan.BeginAnimation();

        //Give control after begin has been said
        Player1.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.None;
        Player2.GetComponent<PlayerControls>().CurrentAction = PlayerControls.ActionType.None;

        Resetting = false;
    }


    /// <summary>
    /// Instantiate player prefabs and link scripts. Called in awake
    /// </summary>
    /// <param name="Player1PrefabName"></param>
    /// <param name="Player2PrefabName"></param>
    public void CreatePlayers(string Player1PrefabName, string Player2PrefabName)
    {
        Object Player1Prefab = Resources.Load(Player1PrefabName);
        Object Player2Prefab = Resources.Load(Player2PrefabName);
        if(Player2Prefab == null || Player2Prefab == null)
        {
            throw new System.Exception("Could not load player prefabs of name (" + Player1PrefabName + ") or (" + Player2PrefabName + ")");
        }

        Player1 = (GameObject)Instantiate(Player1Prefab);
        Player2 = (GameObject)Instantiate(Player2Prefab);

        PlayerControls P1Controls = Player1.GetComponent<PlayerControls>();
        PlayerControls P2Controls = Player2.GetComponent<PlayerControls>();

        P1Controls.gameManager = this;
        P2Controls.gameManager = this;

        P1Controls.PlayerNumber = "P1";
        P2Controls.PlayerNumber = "P2";

        P1Controls.OtherPlayer = Player2;
        P2Controls.OtherPlayer = Player1;

        Player1.transform.position = new Vector3(-4, 0.5f, 0);
        Player2.transform.position = new Vector3(4, 0.5f, 0);
    }
}
