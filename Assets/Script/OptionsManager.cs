using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixerMain;
    public AudioMixer audioMixerSfx;
    public Slider[] slides;

    private void Start()
    {      
        slides[0].value = PlayerPrefs.GetFloat("main");
        audioMixerMain.SetFloat("main",slides[0].value);

        slides[1].value = PlayerPrefs.GetFloat("sfx");
        audioMixerSfx.SetFloat("sfx",slides[1].value);
    }

    public void SetVolumeMain(float volume)
    {
        audioMixerMain.SetFloat("main",volume);
        PlayerPrefs.SetFloat("main", volume);
    }

    public void SetVolumeSFX(float volume)
    {
        audioMixerSfx.SetFloat("sfx",volume);
        PlayerPrefs.SetFloat("sfx", volume);
    }
}
