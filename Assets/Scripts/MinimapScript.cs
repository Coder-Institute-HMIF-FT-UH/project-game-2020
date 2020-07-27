using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        // Minimap follow Player's position
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        
        // Minimap follow Player's rotation.y
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
