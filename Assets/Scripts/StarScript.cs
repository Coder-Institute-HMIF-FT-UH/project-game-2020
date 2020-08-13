using System;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public SceneLoader sceneLoader;
    [SerializeField] private bool isTaken;
    
    private float speed = 180f;
    private string currentSceneName;

    private void Start()
    {
        currentSceneName = sceneLoader.CurrentScene.name;
        // If star is taken, Destroy object
        if (PlayerPrefs.GetInt("is" + gameObject.name + currentSceneName) == 1)
            Destroy(gameObject);
    }

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
        PlayerPrefs.SetInt("is" + gameObject.name + currentSceneName, Convert.ToInt32(isTaken)); // Set prefs
    }
}
