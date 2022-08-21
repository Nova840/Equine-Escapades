using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private Animator animator;
    [SerializeField] AudioClip explosion = null;

    [SerializeField] AudioClip neigh = null;

    [SerializeField]
    private Renderer turnRed = null;

    [SerializeField]
    private Transform sizeAnimated = null;

    [SerializeField]
    private float allRedSize = 1.5f, noRedSize = 1;

    [SerializeField]
    private float time = 3, radius = 5;
    private float timeLeft;

    [SerializeField]
    private Color originalColor = Color.black;

    [SerializeField]
    private float explosionForce = 25;

    [SerializeField]
    private GameObject bombExplosionPrefab = null;

    private void Awake() {
        turnRed.material.color = originalColor;
        animator = GetComponent<Animator>();
    }

    private void Start() {
        animator.Play("A_Bomb_Explode");
        timeLeft = time;
    }

    //show the area of effect of bomb explosion on selection 
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private bool exploded = false;
    private void Update() {
        if (exploded)//just in case it isn't destroyed. Don't think it's possible
            return;
        timeLeft -= Time.deltaTime;
        turnRed.material.color = Color.Lerp(originalColor, Color.red, PercentRed());
        if (timeLeft <= 0) {
            Instantiate(bombExplosionPrefab, transform.position, transform.rotation);
            Rigidbody[] rBodiesToExplode = GetRigidbodiesInRadius(transform.position, radius);
            foreach (Rigidbody r in rBodiesToExplode) {
                r.AddExplosionForce(explosionForce, transform.position, radius, 0, ForceMode.VelocityChange);               
                if (r.CompareTag("Player"))
                {
                    Sound.PlaySound(neigh);
                    r.GetComponent<PlayerScore>().DropFood(3);
                }                   
                    
            }
            Sound.PlaySound(explosion);
            DisconnectParticles();
            Destroy(gameObject);
            exploded = true;
        }
    }

    private float PercentRed() {
        return (sizeAnimated.localScale.x - noRedSize) / (allRedSize - noRedSize);
    }

    private void DisconnectParticles() {//so they can gradually fade out //copied from Present
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
            ps.Stop();
            Vector3 localScale = ps.transform.localScale;
            ps.transform.SetParent(null, true);
            ps.transform.localScale = localScale;
        }
    }

    private static Rigidbody[] GetRigidbodiesInRadius(Vector3 position, float radius) {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        List<Rigidbody> rBodies = new List<Rigidbody>();
        foreach (Collider c in colliders) {
            Rigidbody[] childRBodies = c.transform.root.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody r in childRBodies)
                if (!rBodies.Contains(r))
                    rBodies.Add(r);
        }
        return rBodies.ToArray();
    }

}