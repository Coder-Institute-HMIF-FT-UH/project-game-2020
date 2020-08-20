using UnityEngine;

public class FloorButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject linkedDoor; //Put the doors that want to get linked with the button here 

    private Animator doorAnimator;

    private void Start()
    {
        doorAnimator = linkedDoor.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item") || other.CompareTag("Player"))
        {
            doorAnimator.SetBool("IsClosed", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        doorAnimator.SetBool("IsClosed", true);
    }
}
