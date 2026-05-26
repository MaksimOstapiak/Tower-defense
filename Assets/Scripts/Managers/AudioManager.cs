using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Джерело звуку")]
    public AudioSource sfxSource;

    [Header("Аудіокліпи")]
    public AudioClip clickSound;
    public AudioClip buildSound;
    public AudioClip enemyDeathSound;
    public AudioClip roundStartSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayClickSound()
    {
        PlaySFX(clickSound);
    }
}