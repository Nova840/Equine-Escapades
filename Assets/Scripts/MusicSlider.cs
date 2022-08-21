using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour {

    private void Start() {
        GetComponent<Slider>().value = Sound.musicVolume;
    }

    public void OnValueChanged(float newValue) {
        Sound.musicVolume = newValue;
    }

}