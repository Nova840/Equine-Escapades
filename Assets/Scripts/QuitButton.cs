using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour {

    public void Quit() {
        Application.Quit();
    }

    private void Update() {
        if (Input.GetButtonDown(InputMapper.Map("K1B6")))
            Quit();
        for (int i = 1; i <= 4; i++)
            if (Input.GetButtonDown(InputMapper.Map("J" + i + "B6")))
                Quit();
    }

}