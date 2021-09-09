using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake() {

        if (instance != null)
        {
            Debug.LogWarning("Mais de uma instancia feita!");
            return;
        }
        instance = this;
    }

    public AudioSource audioSource;
    public AudioClip[] audioClip;

    public void PlayAudioClip(int index){
        audioSource.PlayOneShot(audioClip[index]);
    }
}
