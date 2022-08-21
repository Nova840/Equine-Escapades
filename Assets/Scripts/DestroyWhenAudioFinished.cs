using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenAudioFinished : MonoBehaviour {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (!audioSource.isPlaying)
            Destroy(gameObject);
    }

}