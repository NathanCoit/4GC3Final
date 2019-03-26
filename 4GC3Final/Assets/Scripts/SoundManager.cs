using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioClip titleMusic;
    public AudioClip menuMusic;
    public AudioClip characterSelectMusic;
    public AudioClip combatMusic;

    private AudioSource musicSource;

    void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "TitleScreen" && !musicSource.isPlaying)
        {
            playTitleMusic();
        }
    }

    private void playTitleMusic()
    {
        musicSource.clip = titleMusic;
        musicSource.Play();
    }


}
