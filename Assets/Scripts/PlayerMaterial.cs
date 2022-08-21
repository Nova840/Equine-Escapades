using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterial : MonoBehaviour {

    private Renderer[] renderers;

    public int PlayerNumber { get; set; }

    private void Awake() {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start() {
        foreach (Renderer r in renderers)
            r.material = StartPlayerMaterial.possibleMaterialsStatic[StartPlayerMaterial.PlayerMaterialIndices[PlayerNumber - 1]];
    }

}