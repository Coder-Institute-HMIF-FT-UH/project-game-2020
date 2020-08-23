using UnityEngine;

public class FloorButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject linkedDoor; //Put the doors that want to get linked with the button here 

    private Transform doorTransform;
    private Collider doorCollider;
    private Vector3 doorOpenState, doorCloseState;
    private bool buttonPressed = true;
    private float distanceTraveled = 0f;

    private void Start()
    {
        doorTransform = linkedDoor.GetComponent<Transform>();
        doorCloseState = doorTransform.transform.position;
        doorOpenState = doorTransform.transform.position + new Vector3(0, 3);

        doorCollider = linkedDoor.GetComponent<Collider>();
    }

    private void Update()
    {
        if (!buttonPressed) //Door is closed when there are no colliders on the button
        {
            if (distanceTraveled > 0)
            {
                doorTransform.transform.position = Vector3.LerpUnclamped(doorCloseState, doorOpenState, distanceTraveled); //Moves the door down
                distanceTraveled -= 0.02f;
            }
            else
            {
                doorCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        doorCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Item"))//Door is opened when the collider is a game object with a player/item tag
        {
            buttonPressed = true;
            if(distanceTraveled < 1)
            {
                doorTransform.transform.position = Vector3.LerpUnclamped(doorCloseState, doorOpenState, distanceTraveled); //Moves the door up
                distanceTraveled += 0.02f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        buttonPressed = false;
    }
}
