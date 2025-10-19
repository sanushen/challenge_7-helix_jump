using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip deathExplosion;
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip collectPoints;
    [SerializeField] private AudioClip platformDestroy;
    private AudioSource audioSource;
    private AudioSource sfxSource;
    private AudioSource sfxDeathExplosion;
    private AudioSource sfxCollectPoints;
    private AudioSource titleMusicSource;
    private AudioSource sfxPlatformDestroy;
    
    void Awake()
    {
        // Singleton pattern to ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keeps audio between scenes
        } else {
            Destroy(gameObject);
            return;
        }
        
        // Setup audio source component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // Set to loop automatically
        audioSource.playOnAwake = false; // Don't play immediately
        audioSource.volume = 0.5f; // Set to 50% volume (adjust as needed)

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 2f;

        sfxDeathExplosion = gameObject.AddComponent<AudioSource>();
        sfxDeathExplosion.loop = false;
        sfxDeathExplosion.playOnAwake = false;
        sfxDeathExplosion.volume = 2f;

        sfxCollectPoints = gameObject.AddComponent<AudioSource>();
        sfxCollectPoints.loop = false;
        sfxCollectPoints.playOnAwake = false;
        sfxCollectPoints.volume = 2f;

        sfxPlatformDestroy = gameObject.AddComponent<AudioSource>();
        sfxPlatformDestroy.loop = false;
        sfxPlatformDestroy.playOnAwake = false;
        sfxPlatformDestroy.volume = 2f;

        //Title music
        titleMusicSource = gameObject.AddComponent<AudioSource>();
        titleMusicSource.clip = titleMusic;
        titleMusicSource.loop = true; // Set to loop automatically
        titleMusicSource.playOnAwake = false; // Don't play immediately
        titleMusicSource.volume = 0.5f; // Set to 50% volume (adjust as needed)
    }
    
    public void StartMusic()
    {
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.Play();
        }
    }
    
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying) {
            audioSource.Stop();
        }
    }

    public void StartTitleMusic()
    {
        if (titleMusicSource != null && !titleMusicSource.isPlaying) {
            titleMusicSource.Play();
        }
    }

    public void StopTitleMusic()
    {
        if (titleMusicSource != null && titleMusicSource.isPlaying) {
            titleMusicSource.Stop();
        }
    }
    
    // Optional: method for fading out music
    public void FadeOutMusic(float fadeTime = 1.0f)
    {
        if (audioSource != null && audioSource.isPlaying) {
            StartCoroutine(FadeOut(fadeTime));
        }
    }
    
    private IEnumerator FadeOut(float fadeTime)
    {
        float startVolume = audioSource.volume;
        
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume for next play
    }

    public IEnumerator FadeInTitleMusic(float fadeTime)
    {
        if (!titleMusicSource.isPlaying)
            titleMusicSource.Play();
            
        titleMusicSource.volume = 0f; // Start from silent
        float targetVolume = 0.5f; // Your desired max volume
        
        while (titleMusicSource.volume < targetVolume)
        {
            titleMusicSource.volume += targetVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        titleMusicSource.volume = targetVolume;
    }

    public void PlayBounceSound()
    {
        if (sfxSource != null && bounceSound != null)
        {
            sfxSource.PlayOneShot(bounceSound, 1.0f);
        }
    }

    public void PlayDeathExplosionSound()
    {
        if (sfxDeathExplosion != null && deathExplosion != null)
        {
            sfxDeathExplosion.PlayOneShot(deathExplosion, 1.0f);
        }
    }

    public void PlayCollectPoints()
    {
        if (sfxCollectPoints != null && collectPoints != null)
        {
            sfxCollectPoints.PlayOneShot(collectPoints, 1.0f);
        }
    }

    public void PlayPlatformDestructionSound()
    {
        if (sfxPlatformDestroy != null && platformDestroy != null)
        {
            sfxPlatformDestroy.PlayOneShot(platformDestroy, 1.0f);
        }
    }
}