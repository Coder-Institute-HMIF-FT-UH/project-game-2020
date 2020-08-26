using UnityEngine;

public class OtherPlatformScript : MonoBehaviour
{
    [SerializeField] private float platformSpeed;
    [SerializeField] private MainPlatformScript mainPlatformScript;

    private Vector3[] platformPosition = new Vector3[2];    // [0] : starting position
                                                            // [1] : end position
    private int currentDestination = 1;

    private void Start()
    {
        //Match this platform destination position to the main platform
        if(mainPlatformScript.travelDistance.x != 0)
        {
            platformPosition[0] = new Vector3(mainPlatformScript.platformPosition[0].x, transform.position.y, transform.position.z);
            Debug.Log(platformPosition[0]);
            platformPosition[1] = new Vector3(mainPlatformScript.platformPosition[1].x, transform.position.y, transform.position.z);
            Debug.Log(platformPosition[1]);
        }
        else
        {
            platformPosition[0] = new Vector3(transform.position.x, transform.position.y, mainPlatformScript.platformPosition[0].z);
            Debug.Log(platformPosition[0]);
            platformPosition[1] = new Vector3(transform.position.x, transform.position.y, mainPlatformScript.platformPosition[1].z);
            Debug.Log(platformPosition[1]);
        }
    }

    private void FixedUpdate()
    {
        if (transform.position == platformPosition[currentDestination])
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
