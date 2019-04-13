using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for allowing player choice on end game screen
/// </summary>
public class EndgameButtonSelect : MonoBehaviour
{
    /*
     * 0 = PlayAgain
     * 1 = CharacterSelect
     * 2 = QuitToTitle
     */
    public Button[] P1Options;
    public Button[] P2Options;
    public GameManager GameManagerScript;

    public Image P1OptionsBackground;
    public Image P2OptionsBackground;
    
    private bool mblnEndGameState = false;
    private int mintCurrentP1Button = 0;
    private int mintCurrentP2Button = 0;
    private bool mblnP1ButtonDown = false;
    private bool mblnP2ButtonDown = false;

    private bool mblnP1Selected;
    private bool mblnP2Selected;
    private SoundManager SoundMan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mblnEndGameState)
        {
            // Player 1 controls
            if(!mblnP1Selected)
            {
                float fP1Axis = Input.GetAxisRaw("P1_Vertical");
                if (fP1Axis > 0 && !mblnP1ButtonDown)
                {
                    // Up
                    if (mintCurrentP1Button - 1 >= 0)
                    {
                        mintCurrentP1Button--;
                        HighlightP1Button();
                    }
                }
                else if (fP1Axis < 0 && !mblnP1ButtonDown)
                {
                    // Down
                    if (mintCurrentP1Button + 1 <= P1Options.Length-1)
                    {
                        mintCurrentP1Button++;
                        HighlightP1Button();
                    }
                }

                if (fP1Axis == 0 && mblnP1ButtonDown)
                {
                    mblnP1ButtonDown = false;
                }

                if (Input.GetButton("P1_Fire1"))
                {
                    // Select
                    SelectP1Button();
                }
            }
            

            // Player 2 controls
            if(!mblnP2Selected)
            {
                float fP2Axis = Input.GetAxisRaw("P2_Vertical");
                if (fP2Axis > 0 && !mblnP2ButtonDown)
                {
                    // Up
                    if (mintCurrentP2Button - 1 >= 0)
                    {
                        mintCurrentP2Button--;
                        HighlightP2Button();
                    }
                }
                else if (fP2Axis < 0 && !mblnP2ButtonDown)
                {
                    // Down
                    if (mintCurrentP2Button + 1 <= P2Options.Length-1)
                    {
                        mintCurrentP2Button++;
                        HighlightP2Button();
                    }
                }

                if (fP2Axis == 0 && mblnP2ButtonDown)
                {
                    mblnP2ButtonDown = false;
                }

                if (Input.GetButton("P2_Fire1"))
                {
                    // Select
                    SelectP2Button();
                }
            }
            if (mblnP1Selected && mblnP2Selected)
            {
                mblnEndGameState = false;
                EvaluatePlayerSelections();
            }
        }
    }

    private void EvaluatePlayerSelections()
    {
        Debug.Log(mintCurrentP1Button + " " +  mintCurrentP2Button);
        // If either player wants to quit to title, quit to title
        if(mintCurrentP1Button == 2 || mintCurrentP2Button == 2)
        {
            GameManagerScript.QuitToTitle();
        }
        else if(mintCurrentP1Button == 1 || mintCurrentP2Button == 1)
        {
            // If either player wants to return to char select, return to char select
            GameManagerScript.ReturnToCharacterSelect();
        }
        else
        {
            GameManagerScript.PlayAgain();
        }
    }

    public void EnableOptionsPanel(Sprite puniP1Image, Sprite puniP2Image)
    {
        P1OptionsBackground.sprite = puniP1Image;
        P2OptionsBackground.sprite = puniP2Image;
        mblnEndGameState = true;
        DeselectP1Buttons();
        DeselectP2Buttons();
        P1Options[0].image.color = Color.red;
        P2Options[0].image.color = Color.blue;
        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void DeselectP1Buttons()
    {
        foreach( Button uniBtn in P1Options)
        {
            uniBtn.image.color = new Color(1, 1, 1, 0);
        }
    }
    private void DeselectP2Buttons()
    {
        foreach (Button uniBtn in P2Options)
        {
            uniBtn.image.color = new Color(1, 1, 1, 0);
        }
    }

    private void HighlightP1Button()
    {
        DeselectP1Buttons();
        P1Options[mintCurrentP1Button].image.color = Color.red;
        mblnP1ButtonDown = true;
        SoundMan.playHover();
    }

    private void HighlightP2Button()
    {
        DeselectP2Buttons();
        P2Options[mintCurrentP2Button].image.color = Color.blue;
        mblnP2ButtonDown = true;
        SoundMan.playHover();
    }

    private void SelectP1Button()
    {
        P1Options[mintCurrentP1Button].image.color = Color.grey;
        mblnP1Selected = true;
        SoundMan.playClick();
    }

    private void SelectP2Button()
    {
        P2Options[mintCurrentP2Button].image.color = Color.grey;
        mblnP2Selected = true;
        SoundMan.playClick();
    }
}
