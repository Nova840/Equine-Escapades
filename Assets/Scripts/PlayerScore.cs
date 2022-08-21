using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour {

    private static List<PlayerScore> refs = new List<PlayerScore>();

    public Transform Camera { get; set; }

    private List<int> carrying = new List<int>();//list of food item values
    public int TotalCarrying { get { return carrying.Sum(); } }

    public int Banked { get; private set; } = 0;

    public int PlayerNumber { get; set; }

    public TMP_Text ScoreText { get; set; }

    [SerializeField]
    private float launchAngleDelta = 45, launchForce = 5, timeBetweenFood = .25f;

    [SerializeField]
    private GameObject number = null;

    private Rigidbody playerRB;
    private Collider[] playerColliders;

    private void Awake() {
        playerRB = GetComponent<Rigidbody>();
        playerColliders = GetComponentsInChildren<Collider>();
    }

    private void Start() {
        refs.Add(this);
        SetScoreText();
    }

    private void OnDestroy() {
        refs.Remove(this);
    }

    public void AddCarrying(int value) {
        carrying.Add(value);
        SetScoreText();
        CreateNumberCanvas(value, PlayerNumber);
    }

    private static void CreateNumberCanvas(int value, int playerNumber) {
        for (int i = 0; i < refs.Count; i++) {//one for each player's camera
            GameObject g = Instantiate(refs[playerNumber - 1].number, refs[playerNumber - 1].transform.position, Quaternion.identity);
            g.GetComponentInChildren<TMP_Text>().text = value.ToString();
            foreach (Transform t in g.GetComponentsInChildren<Transform>())
                t.gameObject.layer = i + 9;
            g.GetComponent<NumberCanvas>().LookAt = refs[i].Camera;
        }
    }

    public void Deposit() {
        Banked += TotalCarrying;
        carrying.Clear();
        SetScoreText();
    }

    public void DropFood(int numberToDrop) {
        StartCoroutine(DropFoodRoutine(numberToDrop));
    }

    private IEnumerator DropFoodRoutine(int numberToDrop) {
        yield return new WaitForSeconds(0);//wait 1 frame so food can inherit explosion velocity
        while (numberToDrop > 0 && carrying.Count > 0) {

            int randomCarryingIndex = Random.Range(0, carrying.Count);
            GameObject foodPrefab = Present.spawnsStatic.First(f => f.GetComponent<FoodItem>().Value == carrying[randomCarryingIndex]);
            GameObject food = Instantiate(foodPrefab, transform.position, transform.rotation);

            food.transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            Rigidbody foodRB = food.GetComponent<Rigidbody>();
            foodRB.velocity = playerRB.velocity;
            foodRB.AddForce(//copied from present script
                new Vector3(
                    Random.Range(-launchAngleDelta, launchAngleDelta),
                    launchAngleDelta,
                    Random.Range(-launchAngleDelta, launchAngleDelta)
                ).normalized * launchForce,
                ForceMode.Impulse
            );

            StartCoroutine(IgnoreCollisions(food));

            CreateNumberCanvas(-carrying[randomCarryingIndex], PlayerNumber);

            carrying.RemoveAt(randomCarryingIndex);
            SetScoreText();

            numberToDrop--;

            yield return new WaitForSeconds(timeBetweenFood);
        }
    }

    private IEnumerator IgnoreCollisions(GameObject food) {
        Collider foodCollider = food.GetComponent<Collider>();
        foreach (Collider c in playerColliders)
            Physics.IgnoreCollision(c, foodCollider, true);
        yield return new WaitForSeconds(1);
        foreach (Collider c in playerColliders)
            Physics.IgnoreCollision(c, foodCollider, false);
    }

    private void SetScoreText() {
        ScoreText.text = "Carrying: " + TotalCarrying + "\n Delivered: " + Banked;
    }

    public static void DestroyScoreTexts() {
        foreach (PlayerScore ps in refs) {
            Destroy(ps.ScoreText.transform.parent.gameObject);
        }
    }

    public static string GetWinnerText() {
        PlayerScore[] playerScores = refs.OrderByDescending(ps => ps.Banked).ToArray();
        string text = "P" + playerScores[0].PlayerNumber + " Wins!\n\n";
        foreach (PlayerScore playerScore in playerScores)
            text += "P" + playerScore.PlayerNumber + ": " + playerScore.Banked + "\n";
        text += "\nPress escape/back to return to start.";
        return text;
    }

}