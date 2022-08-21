using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private Transform horsesContainer = null;
    private Image[] horseImages;

    [SerializeField]
    private Sprite noHorseSprite = null;

    [SerializeField]
    private Sprite[] horseSprites = new Sprite[0];//must match StartPlayerMaterial's possible matertials array

    private static List<string> players = new List<string>();//stores players as their input device (ex. {"K1", "J1"})

    private void Awake() {
        horseImages = horsesContainer.GetComponentsInChildren<Image>();
    }

    public static int FindIndex(string device) {
        return players.IndexOf(device);
    }

    public static string GetElementAt(int index) {
        return players[index];
    }

    public static int NumPlayers() {
        return players.Count;
    }

    private void Update() {
        if (Input.GetButtonDown(InputMapper.Map("K1B2")))
            AddOrRemove("K1");
        if (Input.GetButtonDown(InputMapper.Map("J1B2")))
            AddOrRemove("J1");
        if (Input.GetButtonDown(InputMapper.Map("J2B2")))
            AddOrRemove("J2");
        if (Input.GetButtonDown(InputMapper.Map("J3B2")))
            AddOrRemove("J3");
        if (Input.GetButtonDown(InputMapper.Map("J4B2")))
            AddOrRemove("J4");

        for (int i = 0; i < horseImages.Length; i++) {
            if (i >= players.Count) {
                horseImages[i].sprite = noHorseSprite;
            } else {
                horseImages[i].sprite = horseSprites[StartPlayerMaterial.PlayerMaterialIndices[i]];
            }
        }
    }

    private void AddOrRemove(string device) {
        if (players.Contains(device)) {
            int playerIndex = players.IndexOf(device);
            StartPlayerMaterial.PlayerMaterialIndices[players.IndexOf(device)] = -1;
            players.Remove(device);
            StartPlayerMaterial.LeftShiftIndices(playerIndex);//because player 2 could now be player 1 and stuff
        } else if (players.Count < 4) {
            players.Add(device);
            StartPlayerMaterial.IncrementPlayerMaterialIndex(players.IndexOf(device) + 1);
        }
    }

}