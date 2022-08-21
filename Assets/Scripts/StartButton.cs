using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

    public void LoadScene(string sceneName) {
        if (PlayerInput.NumPlayers() > 0)
            SceneManager.LoadScene(sceneName);
    }

    private void Update() {
        if (IsDeviceReady("K1") ||
            IsDeviceReady("J1") ||
            IsDeviceReady("J2") ||
            IsDeviceReady("J3") ||
            IsDeviceReady("J4"))
            LoadScene("Game");
    }

    private bool IsDeviceReady(string device) {
        return Input.GetButtonDown(InputMapper.Map(device + "B7")) && PlayerInput.FindIndex(device) != -1;
    }

}