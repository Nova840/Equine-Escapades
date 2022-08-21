using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberCanvas : MonoBehaviour {

    private Rigidbody rb;
    private TMP_Text text;

    public Transform LookAt { get; set; }

    [SerializeField]
    private float upVelocityOnHit = 5, destroyAfterTime = 5, gravity = 5, fadeSpeed = 1;

    private float startTime;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        startTime = Time.time;
        SetVelocity();
    }

    private void FixedUpdate() {
        rb.AddForce(Vector3.down * gravity * rb.mass, ForceMode.Force);
    }

    private void Update() {
        if (text.color.a <= .0001f)
            Destroy(gameObject);
        if (Time.time - startTime >= destroyAfterTime) {
            Color c = text.color;
            c.a -= fadeSpeed * Time.deltaTime;
            text.color = c;
        }
        //https://answers.unity.com/questions/132592/lookat-in-opposite-direction.html
        transform.LookAt(2 * transform.position - LookAt.position);
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.root.name == "Level" && !other.isTrigger)
            SetVelocity();
    }

    private void SetVelocity() {
        rb.velocity = new Vector3(rb.velocity.x, upVelocityOnHit, rb.velocity.z);
    }

}