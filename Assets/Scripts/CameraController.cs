using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 nextPosition = Vector3.zero;
        
    public Transform player, target;
    public Vector3 speed = new Vector3(4f, 2f, 1f);
    
    private void LateUpdate()
    {
        FollowPlayer();
        LookAtPlayer();
    }

    /// <summary>
    /// FollowPlayer: Camera follows Player
    /// </summary>
    private void FollowPlayer()
    {
        // Get position
        Vector3 position = transform.position;
        Vector3 targetPosition = target.position;

        // Smooth Camera movement
        nextPosition.x = Mathf.Lerp(position.x, targetPosition.x, speed.x * Time.deltaTime);
        nextPosition.y = Mathf.Lerp(position.y, targetPosition.y, speed.y * Time.deltaTime);
        nextPosition.z = Mathf.Lerp(position.z, targetPosition.z, speed.z * Time.deltaTime);
        
        transform.position = nextPosition; // Move camera
    }

    /// <summary>
    /// LookAtPlayer: Look only to player
    /// </summary>
    private void LookAtPlayer()
    {
        transform.LookAt(player);
    }
}
