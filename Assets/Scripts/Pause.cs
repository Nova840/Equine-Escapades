using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public static bool Paused { get; private set; } = false;

    private void Start() {
        PauseGame(false);
    }

    private void Update() {
        if (EndGameAfterTime.TimeLeft > 0) {
            CheckPause();
        } else if (!Paused) {
            PauseGame(true);
        }
        if (Paused) {
            CheckStart();
        }
    }

    private void CheckPause() {
        //escape/start button pushed and player is registered
        if (Input.GetButtonDown(InputMapper.Map("K1B6")) && PlayerInput.FindIndex("K1") != -1)
            PauseGame(!Paused);
        for (int i = 1; i <= 4; i++)
            if (Input.GetButtonDown(InputMapper.Map("J" + i + "B7")) && PlayerInput.FindIndex("J" + i) != -1)
                PauseGame(!Paused);
    }

    private void CheckStart() {
        if (Input.GetButtonDown(InputMapper.Map("K1B7")) && PlayerInput.FindIndex("K1") != -1)
            LoadStart();
        for (int i = 1; i <= 4; i++)
            if (Input.GetButtonDown(InputMapper.Map("J" + i + "B6")) && PlayerInput.FindIndex("J" + i) != -1)
                LoadStart();
    }

    private static void PauseGame(bool pause) {
        Paused = pause;
        Time.timeScale = Paused ? 0 : 1;
        Cursor.visible = Paused ? true : false;
        Cursor.lockState = Paused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private static void LoadStart() {
        Paused = false;
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Start");
    }

}