using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private FixedButton pickUpBtn;
    [SerializeField] private GameObject passwordPanel;
    
    private bool pickUpAllowed = true, isPickingUp;
    private Camera mainCamera;
    private Image crosshairImage;
    [SerializeField] private Transform itemTransform;
    [SerializeField] private Rigidbody itemRigidbody;
    private Ray ray;
    private RaycastHit hit;

    public bool IsPickingUp => isPickingUp;

    private void Start()
    {
        crosshairImage = GetComponent<Image>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f , 0));
        
        if (Physics.Raycast(ray, out hit, maxDistance) && pickUpAllowed)
        {
            // Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (hit.transform.CompareTag("Item"))
            {
                crosshairImage.color = Color.blue;
                itemTransform = hit.transform;
                itemRigidbody = hit.rigidbody;
            }
            else if(hit.transform.CompareTag("Door") && pickUpBtn.pressed)
            {
                passwordPanel.SetActive(true);
            }
            else
            {
                itemTransform = null;
                itemRigidbody = null;
                crosshairImage.color = Color.white;
            }
        }
        
        // If pickUpBtn is pressed, ...
        // Be Careful... This is called 3-4 times if button is pressed.
        if(pickUpBtn.pressed)
        {
            pickUpBtn.pressed = false;
            if (pickUpAllowed) // If pickUpAllowed, ...
                PickUpItem(); // pick up item
            else // else, ...
                ReleaseItem(); // release item
        }
        
        // To continue picking up after pick up is allowed
        if (isPickingUp)
            PickUpItem();
    }

    private void PickUpItem()
    {
        if (!itemTransform || !itemRigidbody) return;
        
        // Check if pickUpButton is not pressed, then pickUpAllowed is false
        if (!pickUpBtn.pressed) pickUpAllowed = false;
        isPickingUp = true;
        
        // Set item
        itemRigidbody.useGravity = false;
        itemTransform.parent = itemHolder;
        itemTransform.position = itemHolder.position;
    }

    private void ReleaseItem()
    {
        if (!itemTransform || !itemRigidbody) return;
        
        pickUpAllowed = true;
        isPickingUp = false;
        
        // Set item to normal
        itemRigidbody.useGravity = true;
        itemRigidbody = null; 
        itemTransform.parent = null;
        itemTransform = null;
    }
}
