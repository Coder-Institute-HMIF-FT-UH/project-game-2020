using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Image gps;
	
	// Update is called once per frame
    private void LateUpdate()
    {
        // Minimap follow Player's position
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        
        // Minimap follow Player's rotation.y
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }

    /// <summary>
    /// GpsOn: Activate GPS
    /// </summary>
    public void GpsOn()
    {
        gps.gameObject.SetActive(true);
    }

    /// <summary>
    /// GpsOff: Deactivate GPS
    /// </summary>
    public void GpsOff()
    {
        gps.gameObject.SetActive(false);
    }
}
