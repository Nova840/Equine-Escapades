using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAudioSourceToMusicVolume : MonoBehaviour {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        audioSource.volume = Sound.musicVolume;
    }

}