using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField]  public AudioSource vfxAudioSource;

    [SerializeField] public AudioClip mucsicClip;
    [SerializeField] public AudioClip coin;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip over;
    [SerializeField] public AudioClip attack;
    [SerializeField] public AudioClip ruong;
    
    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource.clip = mucsicClip;
        musicAudioSource.Play();

        //if (!PlayerPrefs.HasKey("musicVolume"))
        //{
        //    PlayerPrefs.SetFloat("musicVolume", 1);
        //    Load();
        //}
        //else
        //{
        //    Load();
        //}
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.clip = sfxClip;
        vfxAudioSource.PlayOneShot(sfxClip);
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
       Save();
    }
    //// Update is called once per frame
    //private void Load()
    //{
    //    volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    //}
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    public void Mute()
    {
        volumeSlider.value = 0;
    }
}
