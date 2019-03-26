using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControls : MonoBehaviour
{
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
            StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("CombatScene");
    }
}
