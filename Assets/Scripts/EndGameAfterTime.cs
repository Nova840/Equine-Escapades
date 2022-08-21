using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameAfterTime : MonoBehaviour {

    [SerializeField]
    private TMP_Text timeText = null;

    [SerializeField]
    private string timeTextPhrase = "Time Remaining: ";//space at the end on purpose

    [SerializeField]
    private TMP_Text winnerText = null;

    [SerializeField]
    private float time = 60 * 3;//3 minutes in seconds
    public static float TimeLeft { get; private set; }

    private bool ended = false;

    SpawnBombsAbove spawnBombsAbove;

    private void Start() {
        TimeLeft = time;
        spawnBombsAbove = gameObject.GetComponent<SpawnBombsAbove>();
    }

    private void Update() {
        if (ended)
            return;
        TimeLeft -= Time.deltaTime;
        int intTimeLeft = (int)TimeLeft;
        timeText.text = timeTextPhrase + intTimeLeft / 60 + "m " + intTimeLeft % 60 + "s";
        if (TimeLeft <= 0) {
            ended = true;
            winnerText.transform.parent.gameObject.SetActive(true);
            winnerText.text = PlayerScore.GetWinnerText();
            PlayerScore.DestroyScoreTexts();
            Time.timeScale = 0;//LoadStartOnBack sets this back
        }

        if (TimeLeft <= time / 2 && !spawnBombsAbove.enabled) {
            spawnBombsAbove.enabled = true;//Enables falling bombs         
        }

    }

}