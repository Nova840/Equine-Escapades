using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFoodNumber : MonoBehaviour
{
    public float speed, textTime;
    private float startTime;

    void Awake()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > startTime + textTime)
        {
            Destroy(gameObject);                                                  
        }
        else
        {
            transform.position += Vector3.up * speed;
        }
    }
}
