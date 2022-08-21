using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwitchStartMenu : MonoBehaviour {

    private GameObject[] containers;

    private void Awake() {
        containers = GetComponentsInChildren<Transform>()
            .Where(t => t.parent == transform && !t.GetComponent<Image>())//ignore background image
            .Select(t => t.gameObject)
            .ToArray();
        ChangeTo("Main");//if they start deactivated they won't be added to the array
    }

    private void Update() {
        GetInput("B1", "Main");
    }

    private bool GetInput(string buttonName, string containerName) {
        if (Input.GetButtonDown(InputMapper.Map("K1" + buttonName))) {
            ChangeTo(containerName);
            return true;
        }
        for (int i = 1; i <= 4; i++) {
            if (Input.GetButtonDown(InputMapper.Map("J" + i + buttonName))) {
                ChangeTo(containerName);
                return true;
            }
        }
        return false;
    }

    public void ChangeTo(string containerName) {
        foreach (GameObject container in containers)
            container.SetActive(container.name == containerName);
    }

}