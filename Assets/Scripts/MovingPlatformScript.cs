using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private Vector3 travelDistance;
    [SerializeField] private float platformSpeed;

    private Vector3[] platformPosition = new Vector3[2];    // [0] : starting position
                                                            // [1] : end position
    private int currentDestination = 1;
    
    private void Start()
    {
        // Puts the platform on the starting  position
        platformPosition[0] = transform.position;
        platformPosition[1] = transform.position + travelDistance;
    }

    private void FixedUpdate()
    {
        if(transform.position == platformPosition[currentDestination])
        {
            currentDestination = currentDestination == 1 ? 0 : 1;
        }
        // Moves the platform to current destination
        transform.position = Vector3.MoveTowards(transform.position, platformPosition[currentDestination], Time.fixedDeltaTime * platformSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Used to attach other game object to this platform
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        // Used to detach other game object to this platform
        other.transform.parent = null;
    }
}
