using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sound {

    public static float soundVolume = 1, musicVolume = 1;

    private static GameObject soundPrefab = null;

    public static void PlaySound(AudioClip audioClip, float volume = 1) {
        if (!soundPrefab)
            soundPrefab = Resources.Load("Sound") as GameObject;
        AudioSource audioSource = Object.Instantiate(soundPrefab).GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume * soundVolume;
        audioSource.Play();
    }

}