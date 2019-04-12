﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public AnimationManager AnimationController;

    private int mintPlayer1Score = 0;
    private int mintPlayer2Score = 0;
    private string mstrPlayer1CharName;
    private string mstrPlayer2CharName;
    private int mintTotalRounds = 0;

    public Image Player1Image;
    public Image Player2Image;

    //Sound man has finally blessed your scripts. Praise him. Love him;
    private SoundManager SoundMan;

    private MenuManager MenuMan;

    private bool Resetting = false;

    //How close to the edge a dash will kill
    public float lethalDistance;

    /// <summary>
    /// Awake called before start. Create players and link scripts
    /// </summary>
    void Awake()
    {
        MenuMan = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        // Get scoreboard components
        

        // Get chosen character names from magical place.
        mstrPlayer1CharName = MenuMan.player1Character;
        mstrPlayer2CharName = MenuMan.player2Character;
        mintTotalRounds = MenuMan.numRounds;

        if(string.IsNullOrWhiteSpace(mstrPlayer1CharName) && string.IsNullOrWhiteSpace(mstrPlayer2CharName))
        {
            mstrPlayer1CharName = "Harri";
            mstrPlayer2CharName = "Red";
        }
        CreatePlayers(mstrPlayer1CharName, mstrPlayer2CharName);
        CreateScoreboardIcons(mstrPlayer1CharName, mstrPlayer2CharName);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Do this for inital round start sounds and ui and stuff
        StartCoroutine(NewGameAnimations());
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
                mintPlayer2Score++;
            }
            else
            {
                mintPlayer1Score++;
            }
            

            if(mintPlayer1Score == mintTotalRounds || mintPlayer2Score == mintTotalRounds)
            {
                // Total rounds played, display winner, return to main menu.
                StartCoroutine(DisplayWinnerScreen());
            }
            else
            {
                DelayedGameReset();
            }
        }   
    }

    private IEnumerator DisplayWinnerScreen()
    {
        yield return null;

        MenuMan.gameEnd = true;

        string strWinnerName = string.Empty;

        SoundMan.cutMusic();
        SoundMan.playbwahbwahBWAH();

        if (mintPlayer1Score > mintPlayer2Score)
        {
            // Player 1 won
            strWinnerName = mstrPlayer1CharName;

            Player2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Player2.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(Player2.transform.position.x, Player2.transform.position.y + 2, Player2.transform.position.z));
            Camera.main.GetComponent<CombatCam>().setTarget(Player2);

            yield return new WaitForSeconds(0.3f);

            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(Player2.transform.position.x, Player2.transform.position.y + 2, Player2.transform.position.z - 20));

            yield return new WaitForSeconds(0.3f);

            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(Player2.transform.position.x + 5, Player2.transform.position.y + 2, Player2.transform.position.z + 10));

            yield return new WaitForSeconds(2);

            Camera.main.GetComponent<CombatCam>().setTarget(Player1);



            // TODO winning animation
            /*
            AnimationController.StartAnimation("Confetti");
            */
        }
        else if (mintPlayer2Score > mintPlayer1Score)
        {
            // Player 2 won
            strWinnerName = mstrPlayer2CharName;

            Player1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Player1.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(Player1.transform.position.x, Player1.transform.position.y + 2, Player1.transform.position.z));
            Camera.main.GetComponent<CombatCam>().setTarget(Player1);

            yield return new WaitForSeconds(0.3f);

            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(Player1.transform.position.x, Player1.transform.position.y + 2, Player1.transform.position.z - 20));

            yield return new WaitForSeconds(0.3f);

            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(Player1.transform.position.x + 5, Player1.transform.position.y + 2, Player1.transform.position.z + 10));

            yield return new WaitForSeconds(2);

            Camera.main.GetComponent<CombatCam>().setTarget(Player2);

            // TODO winning animation
            /*
            AnimationController.StartAnimation("Confetti");
            */
        }
        else
        {
            // tie
            strWinnerName = "Tie"; // ?
        }

        SoundMan.playResultsMusic();

        Player1.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Player2.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Player1.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        Player2.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

        Player1.transform.position = new Vector3(-2, 0.5f, 0);
        Player2.transform.position = new Vector3(2, 0.5f, 0);

        Camera.main.GetComponent<CombatCam>().resetCamera();

        Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(0, 3, 0));

        SoundMan.playCharacterName(strWinnerName);
        yield return new WaitForSeconds(1);
        SoundMan.playWins();


        // TODO Set winner name text

        // TODO Enable winner screen


        yield return null;
    }

    public void DelayedGameReset()
    {
        
        StartCoroutine(DelayedResetCoroutine(3));
    }

    private IEnumerator DelayedResetCoroutine(float seconds)
    {
        //Wait a frame cause unity
        yield return null;

        MenuMan.updateRoundsText();

        //Isnt first round
        if(!(mintPlayer1Score == 0 && mintPlayer2Score == 0))
            MenuMan.scoreBoardAnimation();

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

        Camera.main.GetComponent<CombatCam>().resetCamera();

        //Game and match point
        if (mintPlayer1Score == MenuMan.numRounds - 1 && mintPlayer2Score == MenuMan.numRounds - 1)
        {
            SoundMan.playMatchPoint();
            MenuMan.matchPointAnimation();
            yield return new WaitForSeconds(2);
        }
        else if (mintPlayer1Score == MenuMan.numRounds - 1 || mintPlayer2Score == MenuMan.numRounds - 1)
        {
            SoundMan.playGamePoint();
            MenuMan.gamePointAnimation();
            yield return new WaitForSeconds(1.75f);
        }
        

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


    /// <summary>
    /// Instantiate player prefabs and link scripts. Called in awake
    /// </summary>
    /// <param name="Player1PrefabName"></param>
    /// <param name="Player2PrefabName"></param>
    public void CreatePlayers(string Player1PrefabName, string Player2PrefabName)
    {
        UnityEngine.Object Player1Prefab = Resources.Load("PlayerModels/" + Player1PrefabName);
        UnityEngine.Object Player2Prefab = Resources.Load("PlayerModels/" + Player2PrefabName);
        if (Player2Prefab == null || Player2Prefab == null)
        {
            throw new System.Exception("Could not load player prefabs of name (" + Player1PrefabName + ") or (" + Player2PrefabName + ")");
        }

        Player1 = (GameObject)Instantiate(Player1Prefab);
        Player2 = (GameObject)Instantiate(Player2Prefab);

        AnimationController.AddAnimation("Player1Start", Player1.GetComponentInChildren<Animator>());
        AnimationController.AddAnimation("Player2Start", Player2.GetComponentInChildren<Animator>());

        PlayerControls P1Controls = Player1.GetComponent<PlayerControls>();
        PlayerControls P2Controls = Player2.GetComponent<PlayerControls>();

        P1Controls.gameManager = this;
        P2Controls.gameManager = this;

        P1Controls.PlayerNumber = "P1";
        P2Controls.PlayerNumber = "P2";

        P1Controls.OtherPlayer = Player2;
        P2Controls.OtherPlayer = Player1;

        // Hard coded starting positions
        Player1.transform.position = new Vector3(-4, 0.5f, 0);
        Player2.transform.position = new Vector3(4, 0.5f, 0);
    }

    private void CreateScoreboardIcons(string pstrPlayer1Name, string pstrPlayer2Name)
    {
        Sprite uniPlayer1Image = Resources.Load<Sprite>("PlayerIcons/" + pstrPlayer1Name);
        Sprite uniPlayer2Image = Resources.Load<Sprite>("PlayerIcons/" + pstrPlayer2Name);
        if (uniPlayer1Image == null || uniPlayer2Image == null)
        {
            throw new System.Exception("Could not load player icons for (" + pstrPlayer1Name + ") or (" + pstrPlayer2Name + ")");
        }

        Player1Image.sprite = uniPlayer1Image;
        Player2Image.sprite = uniPlayer2Image;
    }

    /// <summary>
    /// TODO hook up to button
    /// </summary>
    public void ReturnToCharacterSelect()
    {
        Destroy(MenuMan.gameObject);
        Destroy(SoundMan.gameObject);
        AnimationController.RunFunctionAfterAnimation(
             () => SceneManager.LoadScene("CharacterSelect"),
             "FadeOut");
    }

    /// <summary>
    /// TODO hook up to button
    /// </summary>
    public void QuitToTitle()
    {
        Destroy(MenuMan.gameObject);
        Destroy(SoundMan.gameObject);
        AnimationController.RunFunctionAfterAnimation(
            () => SceneManager.LoadScene("TitleScreen"),
            "FadeOut");
        
    }

    /// <summary>
    /// TODO hook up to button
    /// </summary>
    public void PlayAgain()
    {
        AnimationController.RunFunctionAfterAnimation(
            () => SceneManager.LoadScene("CombatScene"),
            "FadeOut");
    }

    /// <summary>
    /// Play some new game animation stuff
    /// </summary>
    /// <returns></returns>
    private IEnumerator NewGameAnimations()
    {
        yield return AnimationController.StartAndWaitForAnimation("FadeIn");
        // Save original cam position/rotation
        Vector3 uniInitialCamVector3 = Camera.main.gameObject.transform.position;

        // Player 1 animation
        Camera.main.gameObject.transform.transform.position = Player1.transform.position + new Vector3(0, 1, -2);
        SoundMan.playCharacterName(MenuMan.player1Character);
        yield return AnimationController.StartAndWaitForAnimation("Player1Start");

        //VERSUS....
        Camera.main.GetComponent<CombatCam>().resetCamera();
        SoundMan.playVersus();
        yield return new WaitForSeconds(2);

        // Player 2 animation
        Camera.main.gameObject.transform.transform.position = Player2.transform.position + new Vector3(0, 1, -2);
        SoundMan.playCharacterName(MenuMan.player2Character);
        yield return AnimationController.StartAndWaitForAnimation("Player2Start");

        Camera.main.GetComponent<CombatCam>().resetCamera();
        yield return DelayedResetCoroutine(0.1f);
    }
    public int getPlayer1Score()
    {
        return mintPlayer1Score;
    }

    public int getPlayer2Score()
    {
        return mintPlayer2Score;
    }
}
