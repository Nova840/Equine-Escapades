using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveWhenGamePaused : MonoBehaviour {

    [SerializeField]
    private GameObject toSetActive = null;

    private void Update() {
        if (EndGameAfterTime.TimeLeft <= 0)
            return;
        if (toSetActive.activeSelf != Pause.Paused)
            toSetActive.SetActive(Pause.Paused);
    }

}