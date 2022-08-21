using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour {

    private void Start() {
        GetComponent<Slider>().value = Sound.soundVolume;
    }

    public void OnValueChanged(float newValue) {
        Sound.soundVolume = newValue;
    }

}