using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioClip chillBackgroundMusic;
    [SerializeField] AudioClip intenseBgm;
    [SerializeField] AudioClip superIntenseBgm;
    float audioFadeOutTime = 1f;

    IEnumerator fadeOutSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        RoombaController.startedChasingPlayer.AddListener(PlayIntenseBackgroundMusic);
        RoombaController.stoppedChasingPlayer.AddListener(PlayChillBackgroundMusic);
        RoombaController.playerEnteredCloseRange.AddListener(PlaySuperIntenseBackgroundMusic);
        RoombaController.playerLeftCloseRange.AddListener(PlayIntenseBackgroundMusic);

        fadeOutSound = AudioFadeOut.FadeOut(audioSource, audioFadeOutTime);
    }

    void PlayChillBackgroundMusic()
    {
        audioSource.clip = chillBackgroundMusic;
        audioSource.Play();
    }

    public void StartPlayingChillBgm()
    {
        StartCoroutine(fadeOutSound);
        Invoke("PlayChillBackgroundMusic", audioFadeOutTime);
    }

    void StartPlayingIntenseBgm()
    {
    StartCoroutine(fadeOutSound);
        Invoke("PlayIntenseBackgroundMusic", audioFadeOutTime);
    }
    
    void StartPlayingSuperIntenseBgm()
    {
        StartCoroutine(fadeOutSound);
        Invoke("PlaySuperIntenseBackgroundMusic", audioFadeOutTime);
    }

    public void PlayIntenseBackgroundMusic()
    {
        audioSource.clip = intenseBgm;
        audioSource.Play();
    }

    public void PlaySuperIntenseBackgroundMusic()
    {
        audioSource.clip = superIntenseBgm;
        audioSource.Play();
    }

    public void StopPlaying()
    {
        StartCoroutine(fadeOutSound);
    }


}

public static class AudioFadeOut {

    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop ();
        audioSource.volume = startVolume;
    }

}
