using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartPlayerMaterial : MonoBehaviour {

    [SerializeField]
    private GameObject mainContainer = null;

    [SerializeField]
    private Material[] possibleMaterials = new Material[0];
    public static Material[] possibleMaterialsStatic;

    public static int[] PlayerMaterialIndices { get; } = new int[4] { -1, -1, -1, -1 };//4 players, points to index of possibleMaterials

    public static void IncrementPlayerMaterialIndex(int playerNumber) {
        int newIndex = Increment(PMI(playerNumber));//will never end up on -1
        for (int i = 0; i < possibleMaterialsStatic.Length - 1; i++) {//if it finishes it will have gone all the way around
            if (!PlayerMaterialIndices.Contains(newIndex))
                break;
            newIndex = Increment(newIndex);
        }
        SetPMI(playerNumber, newIndex);
    }

    public static void LeftShiftIndices(int playerIndex) {//when a player is removed, player 2 becomes player 1 and so forth and this fixes that
        for (int i = 0; i < PlayerMaterialIndices.Length - 1; i++) {//leave last because that's always set to -1 after
            if (i < playerIndex)
                continue;
            PlayerMaterialIndices[i] = PlayerMaterialIndices[i + 1];
        }
        PlayerMaterialIndices[PlayerMaterialIndices.Length - 1] = -1;
    }

    private static int Increment(int value) {
        return (value + 1) % possibleMaterialsStatic.Length;
    }

    private static int PMI(int playerNumber) {
        return PlayerMaterialIndices[playerNumber - 1];
    }

    private static void SetPMI(int playerNumber, int value) {
        PlayerMaterialIndices[playerNumber - 1] = value;
    }

    private void Awake() {
        possibleMaterialsStatic = possibleMaterials;
    }

    private void Update() {
        if (!mainContainer.activeInHierarchy)
            return;
        if (Input.GetButtonDown(InputMapper.Map("K1B1")) && PlayerInput.FindIndex("K1") != -1)
            IncrementPlayerMaterialIndex(PlayerInput.FindIndex("K1") + 1);
        if (Input.GetButtonDown(InputMapper.Map("J1B1")) && PlayerInput.FindIndex("J1") != -1)
            IncrementPlayerMaterialIndex(PlayerInput.FindIndex("J1") + 1);
        if (Input.GetButtonDown(InputMapper.Map("J2B1")) && PlayerInput.FindIndex("J2") != -1)
            IncrementPlayerMaterialIndex(PlayerInput.FindIndex("J2") + 1);
        if (Input.GetButtonDown(InputMapper.Map("J3B1")) && PlayerInput.FindIndex("J3") != -1)
            IncrementPlayerMaterialIndex(PlayerInput.FindIndex("J3") + 1);
        if (Input.GetButtonDown(InputMapper.Map("J4B1")) && PlayerInput.FindIndex("J4") != -1)
            IncrementPlayerMaterialIndex(PlayerInput.FindIndex("J4") + 1);
    }

}