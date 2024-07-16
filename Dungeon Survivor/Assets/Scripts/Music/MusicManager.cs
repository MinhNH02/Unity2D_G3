using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;
    AudioSource musicSource;
    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Play(musicClip, true);
    }
    AudioClip switchTo;
    public void Play(AudioClip music, bool interrupt = false)
    {
        if (interrupt == true)
        {
            volume = 1f;
            musicSource.volume = volume;
            musicSource.clip = music;
            musicSource.Play();
        }
        else
        {
            switchTo = music;
            StartCoroutine(SmoothSwitchMusic());
        }

    }

    float volume;
    [SerializeField] float timeToSwitch;

    IEnumerator SmoothSwitchMusic()
    {
        volume = 1f;
        while (volume > 0f)
        {
            volume -= Time.deltaTime / timeToSwitch;
            if (volume < 0f) { volume = 0f; }
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
        Play(switchTo, true);
    }
}
