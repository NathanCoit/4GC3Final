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

    //UI
    public AudioClip titleScreenSelect;

    //Announcer
    public AudioClip ready;
    public AudioClip begin;

    //Two for that sick crossfade
    private AudioSource musicSource1;
    private AudioSource musicSource2;

    private AudioSource uiEffectsSource;
    private AudioSource announcerSource;

    private float fadeRate = 3.5f;

    void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

    void Start()
    {
        musicSource1 = GetComponents<AudioSource>()[0];
        musicSource2 = GetComponents<AudioSource>()[1];

        uiEffectsSource = GetComponents<AudioSource>()[2];
        announcerSource = GetComponents<AudioSource>()[3];

    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "TitleScreen" && !musicSource1.isPlaying)
        {
            playTitleMusic();
        }
        else if(SceneManager.GetActiveScene().name == "CharacterSelect" && !musicSource2.isPlaying)
        {
            playCharacterSelectMusic();
        }
        else if (SceneManager.GetActiveScene().name == "CombatScene" && !musicSource1.isPlaying)
        {
            playCombatMusic();
        }
    }

    private void playTitleMusic()
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
        else if(musicSource2.isPlaying)
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
        else if (musicSource2.isPlaying)
        {
            StartCoroutine(FadeOut(2));
            musicSource1.clip = combatMusic;
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
            audioSource.volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.0f, fadeRate* Time.deltaTime );
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

        while (audioSource.volume< 0.9f)
        {
            audioSource.volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 1.0f, fadeRate* Time.deltaTime);
            yield return null;
        }

        // Close enough, turn it on!
        audioSource.volume = 1.0f;
    }

    public void playTitleScreenSelect()
    {
        if (!uiEffectsSource.isPlaying)
        {
            uiEffectsSource.clip = titleScreenSelect;
            uiEffectsSource.Play();
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
}
