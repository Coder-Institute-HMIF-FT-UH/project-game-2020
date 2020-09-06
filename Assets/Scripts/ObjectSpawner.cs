using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform upperSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log("Spawn");
            other.transform.position = upperSpawner.position;
        }
    }
}
