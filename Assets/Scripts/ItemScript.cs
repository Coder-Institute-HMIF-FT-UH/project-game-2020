using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private Transform bottomSpawner, 
        upperSpawner;

    private Vector3 originPosition;

    private void Start()
    {
        originPosition = GetComponent<Transform>().position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == bottomSpawner.gameObject)
        {
            transform.position = new Vector3(originPosition.x, upperSpawner.position.y, originPosition.z);
        }
    }
}
