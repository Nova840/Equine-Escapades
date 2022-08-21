using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour {

    [SerializeField]
    private int value = 1;
    public int Value { get { return value; } }

    private bool hasTriggered = false;

    private float startTime;

    [SerializeField]
    private float pickupDelay = .25f;

    [SerializeField] AudioClip collectFood = null;
    [SerializeField] float volume = .8f;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        if (transform.position.y < -1000)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (hasTriggered || Time.time - startTime < pickupDelay)
            return;
        Transform root = collision.transform.root;
        if (root.CompareTag("Player")) {
            root.GetComponent<PlayerScore>().AddCarrying(value);
            Sound.PlaySound(collectFood, volume);
            Destroy(gameObject);
            hasTriggered = true;
        }
    }

}