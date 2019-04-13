using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //Music
    public AudioClip titleMusic;
    public AudioClip menuMusic;
    public AudioClip characterSelectMusic;
    public AudioClip combatMusic;
    public AudioClip resultsMusic;

    //UI
    public AudioClip titleScreenSelect;
    public AudioClip charSelect;
    public AudioClip hover;
    public AudioClip click;
    public AudioClip whoosh;

    //Announcer
    public AudioClip ready;
    public AudioClip begin;
    public AudioClip select;
    public AudioClip versus;
    public AudioClip gamePoint;
    public AudioClip matchPoint;
    public AudioClip Elijah;
    public AudioClip Harri;
    public AudioClip Nathan;
    public AudioClip Sully;
    public AudioClip Zack;
    public AudioClip Red;
    public AudioClip Wins;

    //Effects
    public AudioClip bwah;
    public AudioClip bwahbwahBWAH;

    //Two for that sick crossfade
    private AudioSource musicSource1;
    private AudioSource musicSource2;

    private List<AudioSource> uiEffectSources;
    private AudioSource announcerSource;
    private AudioSource effectsSource;

    private AudioSource player1Rolling;
    private AudioSource player2Rolling;

    private AudioSource collisionSource;

    private float fadeRate = 3.5f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        musicSource1 = GetComponents<AudioSource>()[0];
        musicSource2 = GetComponents<AudioSource>()[1];

        uiEffectSources = new List<AudioSource>();
        uiEffectSources.Add(GetComponents<AudioSource>()[2]);
        uiEffectSources.Add(GetComponents<AudioSource>()[3]);
        uiEffectSources.Add(GetComponents<AudioSource>()[4]);
        announcerSource = GetComponents<AudioSource>()[5];
        effectsSource = GetComponents<AudioSource>()[6];

        player1Rolling = GetComponents<AudioSource>()[7];
        player2Rolling = GetComponents<AudioSource>()[8];

        collisionSource = GetComponents<AudioSource>()[9];

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen" && !musicSource1.isPlaying)
        {
            playTitleMusic();
        }
        else if (SceneManager.GetActiveScene().name == "CharacterSelect" && !musicSource2.isPlaying)
        {
            playCharacterSelectMusic();
            playSelect();
        }
        else if (SceneManager.GetActiveScene().name == "CombatScene" && !musicSource1.isPlaying && !GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>().gameEnd)
        {
            playCombatMusic();
        }
    }

    public void cutMusic()
    {
        musicSource1.Stop();
        musicSource2.Stop();
    }

    public void fadeMusic()
    {
        StartCoroutine(FadeOut(1));
        StartCoroutine(FadeOut(2));
    }

    public void playCollision(float volume)
    {
        if (!collisionSource.isPlaying)
        {
            collisionSource.pitch = 0.8f + (volume / 2);
            collisionSource.volume = volume;
            collisionSource.Play();
        }
    }

    public void playRolling(string playerID, float volume)
    {
        if(playerID == "P1")
        {
            player1Rolling.volume = volume;
            if(!player1Rolling.isPlaying)
                player1Rolling.Play();
        }
        else
        {
            player2Rolling.volume = volume;
            if (!player2Rolling.isPlaying)
                player2Rolling.Play();
        }
    }

    public void stopRolling(string playerID)
    {
        if (playerID == "P1")
        {
            player1Rolling.Stop();
        }
        else
        {
            player2Rolling.Stop();
        }
    }

    public void playTitleMusic()
    {
        musicSource1.clip = titleMusic;
        musicSource1.Play();
    }

    private void playCharacterSelectMusic()
    {
        if (musicSource1.isPlaying)
        {
            StartCoroutine(FadeOut(1));
            musicSource2.clip = characterSelectMusic;
            StartCoroutine(FadeIn(2));
            musicSource2.Play();
        }
        else
        {
            StartCoroutine(FadeOut(2));
            musicSource1.clip = characterSelectMusic;
            StartCoroutine(FadeIn(1));
            musicSource1.Play();
        }
    }

    private void playCombatMusic()
    {
        if (musicSource1.isPlaying)
        {
            StartCoroutine(FadeOut(1));
            musicSource2.clip = combatMusic;
            StartCoroutine(FadeIn(2));
            musicSource2.Play();
        }
        else
        {
            StartCoroutine(FadeOut(2));
            musicSource1.clip = combatMusic;
            StartCoroutine(FadeIn(1));
            musicSource1.Play();
        }
    }

    public void playResultsMusic()
    {
        musicSource1.pitch = 1;
        musicSource2.pitch = 1;
        if (musicSource1.isPlaying)
        {
            StartCoroutine(FadeOut(1));
            musicSource2.clip = resultsMusic;
            StartCoroutine(FadeIn(2));
            musicSource2.Play();
        }
        else
        {
            StartCoroutine(FadeOut(2));
            musicSource1.clip = resultsMusic;
            StartCoroutine(FadeIn(1));
            musicSource1.Play();
        }
    }

    private IEnumerator FadeOut(int source)
    {
        AudioSource audioSource = null;
        if (source == 1)
            audioSource = musicSource1;
        else
            audioSource = musicSource2;

        while (audioSource.volume > 0.1f)
        {
            audioSource.volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.0f, fadeRate * Time.deltaTime);
            yield return null;
        }

        // Close enough, turn it off!
        audioSource.volume = 0.0f;
        audioSource.Stop();
    }

    private IEnumerator FadeIn(int source)
    {
        AudioSource audioSource = null;
        if (source == 1)
            audioSource = musicSource1;
        else
            audioSource = musicSource2;

        while (audioSource.volume < 0.9f)
        {
            audioSource.volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 1.0f, fadeRate * Time.deltaTime);
            yield return null;
        }

        // Close enough, turn it on!
        audioSource.volume = 1.0f;
    }

    public void playTitleScreenSelect()
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                a.clip = titleScreenSelect;
                a.Play();
                break;
            }
        }
    }

    public void playCharSelect()
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                a.clip = charSelect;
                a.Play();
                break;
            }
        }
    }

    public void playHover()
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                a.clip = hover;
                a.Play();
                break;
            }
        }
    }

    public void playClick()
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                a.clip = click;
                a.Play();
                break;
            }
        }
    }

    public void playWhoosh()
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                a.clip = whoosh;
                a.Play();
                break;
            }
        }
    }

    public void playCharacterName(string name)
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                switch (name)
                {
                    case "Elijah":
                        a.clip = Elijah;
                        break;
                    case "Harri":
                        a.clip = Harri;
                        break;
                    case "Nathan":
                        a.clip = Nathan;
                        break;
                    case "Sullivan":
                        a.clip = Sully;
                        break;
                    case "Zack":
                        a.clip = Zack;
                        break;
                    case "Red":
                        a.clip = Red;
                        break;
                }
                a.Play();
                break;
            }
            
        }
    }
    
    public void playVersus()
    {
        foreach (AudioSource a in uiEffectSources)
        {
            if (!a.isPlaying)
            {
                a.clip = versus;
                a.Play();
                break;
            }
        }
    }

    public void playBegin()
    {
        if (!announcerSource.isPlaying)
        {
            announcerSource.clip = begin;
            announcerSource.Play();
        }
    }

    public void playReady()
    {
        if (!announcerSource.isPlaying)
        {
            announcerSource.clip = ready;
            announcerSource.Play();
        }
    }

    public void playSelect()
    {
        if (!announcerSource.isPlaying)
        {
            announcerSource.clip = select;
            announcerSource.Play();
        }
    }

    public void playMatchPoint()
    {
        if (!announcerSource.isPlaying)
        {
            announcerSource.clip = matchPoint;
            musicSource1.pitch = 1.4f;
            announcerSource.Play();
        }
    }

    public void playGamePoint()
    {
        if (!announcerSource.isPlaying)
        {
            announcerSource.clip = gamePoint;
            musicSource1.pitch = 1.2f;
            announcerSource.Play();
        }
    }

    public void playWins()
    {
        if (!announcerSource.isPlaying)
        {
            announcerSource.clip = Wins;
            announcerSource.Play();
        }
    }

    public void playBwah()
    {
        if (!effectsSource.isPlaying)
        {
            effectsSource.clip = bwah;
            effectsSource.Play();
        }
    }

    public void playbwahbwahBWAH()
    {
        effectsSource.Stop();
        if (!effectsSource.isPlaying)
        {
            effectsSource.clip = bwahbwahBWAH;
            effectsSource.Play();
        }
    }

}
