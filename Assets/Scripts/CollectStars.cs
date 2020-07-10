using UnityEngine;

public class CollectStars : MonoBehaviour
{
    private GameObject player;
    private int starsCount;

    private void Awake()
    {
        // Find Player
        player = GameObject.FindWithTag("Player");
        
        // Warning if there is no player in scene
        if (!player)
        {
            Debug.LogWarning("There is no player in scene.");
        }
    }

    /// <summary>
    /// OnTriggerEnter: When star collide with other collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // If collide with player, ...
        if (other.gameObject == player)
        {
            // Add effects
            
            // Collecting stars
            gameObject.SetActive(false);
        }
    }
}
