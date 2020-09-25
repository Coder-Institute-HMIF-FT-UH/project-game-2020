using UnityEngine;

public class FloorButtonScript : MonoBehaviour
{
    [Tooltip("Select the door that linked with this button.")] 
    [SerializeField] private GameObject linkedDoor; 

    private Transform doorTransform;
    private Collider doorCollider;
    private Vector3 doorOpenState, doorCloseState;
    private bool buttonPressed = true;
    private float distanceTraveled = 0f;

    private void Start()
    {
        doorTransform = linkedDoor.GetComponent<Transform>();
        doorCollider = linkedDoor.GetComponent<Collider>();
        doorCloseState = doorTransform.transform.position;
        doorOpenState = doorTransform.transform.position + new Vector3(0, 3);
    }

    private void Update()
    {
        // Door is closed when there are no colliders on the button
        if (buttonPressed) return;
        if (distanceTraveled > 0)
        {
            // Moves the door down
            doorTransform.transform.position = Vector3.LerpUnclamped(doorCloseState, doorOpenState, distanceTraveled);
            distanceTraveled -= 0.02f;
        }
        else
        {
            doorCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        doorCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if other is player or Item
        if (!other.CompareTag("Player") && !other.CompareTag("Item")) return;
        buttonPressed = true;
        if(distanceTraveled < 2)
        {
            // Moves the door up
            doorTransform.transform.position = Vector3.LerpUnclamped(doorCloseState, doorOpenState, distanceTraveled);
            distanceTraveled += 0.02f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Player") || other.CompareTag("Item")) return;
        buttonPressed = false;
    }
}
