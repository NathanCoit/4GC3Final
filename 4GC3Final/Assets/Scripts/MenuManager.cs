using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public SoundManager SoundMan;

    private GameObject readyText;
    private GameObject beginText;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && SceneManager.GetActiveScene().name == "TitleScreen")
            StartCharacterSelect();
        else if (Input.anyKey && SceneManager.GetActiveScene().name == "CharacterSelect")
            StartGame();
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

    public void StartGame()
    {
        Initiate.Fade("CombatScene", Color.white, 2.0f);
        SoundMan.playTitleScreenSelect();
    }

    public void ReadyAnimation()
    {
        readyText.GetComponent<Animator>().SetTrigger("ready");
    }

    public void BeginAnimation()
    {
        beginText.GetComponent<Animator>().SetTrigger("begin");
    }
}
