using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour {

    [SerializeField]
    private GameObject playerPrefab = null, cameraPrefab = null, screenZonePrefab = null;

    [SerializeField]
    private Transform mainCanvas = null;

    [SerializeField]
    private Transform spawnPointsContainer = null;
    private List<Transform> spawnPoints = new List<Transform>();

    private void Awake() {
        spawnPoints = spawnPointsContainer.GetComponentsInChildren<Transform>().Where(t => t != spawnPointsContainer).ToList();
    }

    private void Start() {
        List<Transform> spawnPointsCopy = new List<Transform>(spawnPoints);
        for (int i = 0; i < PlayerInput.NumPlayers(); i++) {
            int spawnPointIndex = Random.Range(0, spawnPointsCopy.Count);
            Transform spawnpoint = spawnPointsCopy[spawnPointIndex];

            GameObject player = Instantiate(playerPrefab, spawnpoint.position, spawnpoint.rotation);
            GameObject cameraRig = Instantiate(cameraPrefab, spawnpoint.position, spawnpoint.rotation);
            GameObject screenZone = Instantiate(screenZonePrefab, mainCanvas);

            Camera camera = cameraRig.transform.Find("Camera").GetComponent<Camera>();

            MovePlayer movePlayer = player.GetComponent<MovePlayer>();
            movePlayer.Device = PlayerInput.GetElementAt(i);
            movePlayer.CameraRig = cameraRig.transform;
            PlayerScore playerScore = player.GetComponent<PlayerScore>();
            playerScore.PlayerNumber = i + 1;
            playerScore.ScoreText = screenZone.transform.Find("Score Background/Score Text").GetComponent<TMP_Text>();
            playerScore.Camera = camera.transform;
            player.GetComponent<PlayerMaterial>().PlayerNumber = i + 1;

            CameraRig rig = cameraRig.GetComponent<CameraRig>();
            rig.Device = PlayerInput.GetElementAt(i);
            rig.Follow = player.GetComponent<Rigidbody>();
            rig.SpawnAngleY = spawnpoint.eulerAngles.y;
            camera.rect = GetCameraRect(i);

            SetAnchors(screenZone.GetComponent<RectTransform>(), i);

            spawnPointsCopy.RemoveAt(spawnPointIndex);
        }
    }

    private static void SetAnchors(RectTransform rt, int playerIndex) {
        if (PlayerInput.NumPlayers() == 1) {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
        } else if (PlayerInput.NumPlayers() == 2) {
            if (playerIndex == 0) {
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(.5f, 1);
            } else {
                rt.anchorMin = new Vector2(.5f, 0);
                rt.anchorMax = new Vector2(1, 1);
            }
        } else if (PlayerInput.NumPlayers() == 3) {
            if (playerIndex == 0) {
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(.5f, 1);
            } else if (playerIndex == 1) {
                rt.anchorMin = new Vector2(.5f, .5f);
                rt.anchorMax = new Vector2(1, 1);
            } else {
                rt.anchorMin = new Vector2(.5f, 0);
                rt.anchorMax = new Vector2(1, .5f);
            }
        } else {
            if (playerIndex == 0) {
                rt.anchorMin = new Vector2(0, .5f);
                rt.anchorMax = new Vector2(.5f, 1);
            } else if (playerIndex == 1) {
                rt.anchorMin = new Vector2(.5f, .5f);
                rt.anchorMax = new Vector2(1, 1);
            } else if (playerIndex == 2) {
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(.5f, .5f);
            } else {
                rt.anchorMin = new Vector2(.5f, 0);
                rt.anchorMax = new Vector2(1, .5f);
            }
        }
    }

    private static Rect GetCameraRect(int playerIndex) {//Y axis reversed
        Rect r;
        if (PlayerInput.NumPlayers() == 1) {
            r = new Rect() { x = 0, y = 0, width = 1, height = 1 };
        } else if (PlayerInput.NumPlayers() == 2) {
            if (playerIndex == 0) {
                r = new Rect() { x = 0, y = 0, width = .5f, height = 1 };
            } else {
                r = new Rect() { x = .5f, y = 0, width = .5f, height = 1 };
            }
        } else if (PlayerInput.NumPlayers() == 3) {
            if (playerIndex == 0) {
                r = new Rect() { x = 0, y = 0, width = .5f, height = 1 };
            } else if (playerIndex == 1) {
                r = new Rect() { x = .5f, y = .5f, width = .5f, height = .5f };
            } else {
                r = new Rect() { x = .5f, y = 0, width = .5f, height = .5f };
            }
        } else {
            if (playerIndex == 0) {
                r = new Rect() { x = 0, y = .5f, width = .5f, height = .5f };
            } else if (playerIndex == 1) {
                r = new Rect() { x = .5f, y = .5f, width = .5f, height = .5f };
            } else if (playerIndex == 2) {
                r = new Rect() { x = 0, y = 0, width = .5f, height = .5f };
            } else {
                r = new Rect() { x = .5f, y = 0, width = .5f, height = .5f };
            }
        }
        return r;
    }

}