using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;

    public static AudioManager Instance
    {
        get;
        private set;
    }

    public void Awake()
    {
        this.source = this.gameObject.AddComponent<AudioSource>();
        Instance = this;
    }

    public void Play(AudioClip audioClip, float volume = 1f)
    {
        this.source.PlayOneShot(audioClip, volume);
    }
}
