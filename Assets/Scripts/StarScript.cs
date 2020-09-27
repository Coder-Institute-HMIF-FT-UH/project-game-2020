using UnityEngine;

public class StarScript : MonoBehaviour
{
    [SerializeField] private LoadLevelManager loadLevelManager;
    [SerializeField] private bool isTaken;
    
    private float speed = 180f;

    public bool IsTaken
    {
        get => isTaken;
        set => isTaken = value;
    }
    
    private void Start()
    {
        // If star is taken, Destroy object
        if (PlayerPrefs.GetInt("is" + gameObject.name + loadLevelManager.LevelName) == 1)
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
        if (other.CompareTag("Player")){
            isTaken = true; // Set isTaken to true
        }
    }
}
