using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositZone : MonoBehaviour {

    [SerializeField]
    private Animator giftHorseAnimator = null;

    [SerializeField] AudioClip depositFood = null;

    [SerializeField]
    private GameObject heartParticles = null;

    [SerializeField]
    private Transform heartParticlesSpawnpoint = null;

    private void OnTriggerStay(Collider other) {
        Transform root = other.transform.root;
        if (root.CompareTag("Player")) {
            PlayerScore playerScore = root.GetComponent<PlayerScore>();
            if (playerScore.TotalCarrying > 0) {
                Sound.PlaySound(depositFood);
                Instantiate(heartParticles, heartParticlesSpawnpoint.position, heartParticlesSpawnpoint.rotation);
                giftHorseAnimator.Play("A_RoundHorse_React");//only plays if not already playing. idk why.
                playerScore.Deposit();
            }
        }
    }

}