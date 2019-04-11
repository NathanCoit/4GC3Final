using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private SoundManager SoundMan;
    private GameManager GameMan;

    private GameObject readyText;
    private GameObject beginText;

    public int numRounds;

    private bool shownRoundSelect;
    private bool combatStartup;

    public string player1Character;
    public string player2Character;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        numRounds = 10;
        combatStartup = true;
        shownRoundSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && SceneManager.GetActiveScene().name == "TitleScreen")
            StartCharacterSelect();
        else if (Input.anyKey && SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            if (player1Character != "" && player2Character != "" && !shownRoundSelect)
            {
                ShowRoundSelect();
                shownRoundSelect = true;
            }
        }
        else if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            if (combatStartup)
            {
                if (readyText == null)
                    readyText = GameObject.FindGameObjectWithTag("ReadyText");
                if (beginText == null)
                    beginText = GameObject.FindGameObjectWithTag("BeginText");
                if (GameMan == null)
                    GameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

                updateRoundsText();
                combatStartup = false;
            }

        }


    }

    public void StartCharacterSelect()
    {
        Initiate.Fade("CharacterSelect", Color.white, 2.0f);
        SoundMan.playTitleScreenSelect();
    }

    public void ShowRoundSelect()
    {
        //This is a stupid way around the fact unity can find inactive gae objects. Stupid.
        GameObject.FindGameObjectWithTag("parentCanvas").transform.GetChild(1).gameObject.SetActive(true);
        RoundSelectAnimation();
        SoundMan.playWhoosh();
        updateRoundsText();
            
    }

    public void updateRoundsText()
    {
        
        GameObject.FindGameObjectWithTag("numRounds").GetComponent<Text>().text = numRounds.ToString();
        if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            GameObject.FindGameObjectWithTag("player1WinCount").GetComponent<Text>().text = GameMan.getPlayer1Score().ToString();
            GameObject.FindGameObjectWithTag("player2WinCount").GetComponent<Text>().text = GameMan.getPlayer2Score().ToString();
        }

    }

    public void StartGame()
    {
        Initiate.Fade("CombatScene", Color.white, 2.0f);
        SoundMan.playTitleScreenSelect();
    }

    public void ReadyAnimation()
    {
        readyText = GameObject.FindGameObjectWithTag("ReadyText");
        readyText.GetComponent<Animator>().SetTrigger("ready");
    }

    public void BeginAnimation()
    {
        beginText = GameObject.FindGameObjectWithTag("BeginText");
        beginText.GetComponent<Animator>().SetTrigger("begin");
    }

    public void RoundSelectAnimation()
    {
        GameObject.FindGameObjectWithTag("roundSelectCanvas").GetComponent<Animator>().SetTrigger("roundSelect");
    }

    public void scoreBoardAnimation()
    {
        GameObject.FindGameObjectWithTag("scoreBoard").GetComponent<Animator>().SetTrigger("scoreBoard");
    }
}
