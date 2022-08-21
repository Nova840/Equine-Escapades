using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Present : MonoBehaviour {

    private Renderer[] renderers;
    private Collider coll;
    private Animator animator;

    [SerializeField] AudioClip tear = null;

    [SerializeField]
    private GameObject bombPrefab = null;

    [SerializeField]
    private GameObject[] spawns = new GameObject[0];
    public static GameObject[] spawnsStatic;

    [SerializeField]
    private float foodPercentChance = .75f;

    [SerializeField]
    private int minFoodSpawns = 1, maxFoodSpawns = 4;

    [SerializeField]
    private int minBombSpawns = 1, maxBombSpawns = 4;

    [SerializeField]
    private float fadeOutSpeed = 1, fadeInSpeed = 1;

    [SerializeField]
    private float launchForce = 10, launchTorque = 10, launchAngleDelta = 45, timeBetweenSpawns = .25f;

    private bool hasTriggered = false;

    private void Awake() {
        renderers = GetComponentsInChildren<Renderer>().Where(r => !r.GetComponent<ParticleSystem>()).ToArray();//all renderers except particle systems
        coll = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        if (spawnsStatic == null)
            spawnsStatic = spawns;
    }

    private void Start() {
        foreach (Renderer r in renderers) {
            Color c = r.material.color;
            c.a = 0;
            r.material.color = c;
        }
        coll.enabled = false;
    }

    private void Update() {
        if (hasTriggered)
            return;
        foreach (Renderer r in renderers) {
            Color c = r.material.color;
            c.a += fadeInSpeed * Time.deltaTime;
            c.a = Mathf.Min(c.a, 1);
            r.material.color = c;
        }
        if (renderers.All(r => r.material.color.a == 1))
            coll.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (hasTriggered)//so you can't trigger it twice with 2 separate player colliders
            return;
        if (other.transform.root.CompareTag("Player")) {
            if (animator)
                animator.Play("Open");
            Sound.PlaySound(tear);
            DisconnectParticles();
            StartCoroutine(FadeAndDestroy());
            if (Random.Range(0f, 1f) <= foodPercentChance)
                StartCoroutine(SpawnFood());
            else
                StartCoroutine(SpawnBombs());
            hasTriggered = true;
        }
    }

    private IEnumerator FadeAndDestroy() {
        if (renderers.Length > 0) {
            while (renderers[0].material.color.a > .0001f) {//all renderers should have the same alpha
                yield return new WaitForSeconds(0);
                foreach (Renderer r in renderers) {
                    Color c = r.material.color;
                    c.a -= fadeOutSpeed * Time.deltaTime;
                    r.material.color = c;
                }
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator SpawnFood() {
        coll.enabled = false;
        int numSpawns = Random.Range(minFoodSpawns, maxFoodSpawns + 1);
        for (int i = 0; i < numSpawns; i++) {
            int spawn = Random.Range(0, spawns.Length);
            GameObject g = Instantiate(spawns[spawn], transform.position, transform.rotation);
            g.transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Launch(g.GetComponent<Rigidbody>());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnBombs() {
        coll.enabled = false;
        int numSpawns = Random.Range(minBombSpawns, maxBombSpawns + 1);
        for (int i = 0; i < numSpawns; i++) {
            GameObject g = Instantiate(bombPrefab, transform.position, transform.rotation);
            g.transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Launch(g.GetComponent<Rigidbody>());
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void DisconnectParticles() {//so they can gradually fade out
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
            ps.Stop();
            Vector3 localScale = ps.transform.localScale;
            ps.transform.SetParent(null, true);
            ps.transform.localScale = localScale;
        }
    }

    private void Launch(Rigidbody rb) {
        rb.AddForce(
            new Vector3(
                Random.Range(-launchAngleDelta, launchAngleDelta),
                launchAngleDelta,
                Random.Range(-launchAngleDelta, launchAngleDelta)
            ).normalized * launchForce,
            ForceMode.VelocityChange
        );
        rb.AddTorque(//add a small spin to it as well
            Random.onUnitSphere * Random.Range(0, launchTorque),
            ForceMode.VelocityChange
        );
    }

}