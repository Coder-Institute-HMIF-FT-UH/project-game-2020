﻿using System;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    [SerializeField] private bool isTaken;
    
    private float speed = 180f;

    // private void Start()
    // {
    //     // If star is taken, Destroy object
    //     if (PlayerPrefs.GetInt("is" + gameObject.name) == 1)
    //         Destroy(gameObject);
    // }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0f, speed*Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If other object is Player, ...
        if (!other.CompareTag("Player")) return;
        isTaken = true; // Set isTaken to true
        PlayerPrefs.SetInt("is" + gameObject.name, Convert.ToInt32(isTaken)); // Set prefs
    }
}
