using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustSettingsSliders : MonoBehaviour {

    [SerializeField]
    private Slider topSlider = null, bottomSlider = null;

    [SerializeField]
    private float verticalThreshold = .75f, horizontalSensitivity = 1;

    private Slider selectedSlider;

    private void Awake() {
        selectedSlider = topSlider;
        ActivateHighlight(selectedSlider, true);
    }

    private void Update() {
        CheckIfSwitchSliders();
        AdjustActiveSlider();
    }

    private bool inputTriggerred = false;
    private void CheckIfSwitchSliders() {
        if (Input.GetAxisRaw("UIA2") <= -verticalThreshold || Input.GetAxisRaw("UIA2") >= verticalThreshold) {//no inputmapper on purpose
            if (!inputTriggerred) {
                Slider notSelected = selectedSlider == topSlider ? bottomSlider : topSlider;

                ActivateHighlight(selectedSlider, false);
                selectedSlider = notSelected;
                ActivateHighlight(selectedSlider, true);

                inputTriggerred = true;
            }
        } else {
            inputTriggerred = false;
        }
    }

    private void AdjustActiveSlider() {
        selectedSlider.value += Input.GetAxisRaw("UIA1") * horizontalSensitivity * Time.deltaTime;
    }

    private void ActivateHighlight(Slider slider, bool active) {
        slider.transform.Find("Selected Image").gameObject.SetActive(active);
    }

}