using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public SoundManager SoundMan;

    private GameObject readyText;
    private GameObject beginText;

    public int numRounds;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && SceneManager.GetActiveScene().name == "TitleScreen")
            StartCharacterSelect();
        else if (Input.anyKey && SceneManager.GetActiveScene().name == "CharacterSelect")
            if(player1Character != "" && player2Character != "")
                ShowRoundSelect();
        else if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            if (readyText == null)
                readyText = GameObject.FindGameObjectWithTag("ReadyText");
            if (beginText == null)
                beginText = GameObject.FindGameObjectWithTag("BeginText");
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

        updateRoundsText();
            
    }

    public void updateRoundsText()
    {
        GameObject.FindGameObjectWithTag("numRounds").GetComponent<Text>().text = numRounds.ToString();
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
}
