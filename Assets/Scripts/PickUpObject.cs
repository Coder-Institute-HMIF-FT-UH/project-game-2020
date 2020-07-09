using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private Transform itemHolder;

    private bool isPickingUp = false;
    private float newTime = 0f,
        rayMaxDistance = 5f,
        tapTimeLimit = 1f;
    private int tapCount = 0;
    [SerializeField] private Transform targetObject;

    private void Update()
    {
        // If there are touches, ... 
        if (Input.touchCount > 0)
        {
            TapItem(); // Tap item
            
            // If player is picking up item and double tap on item, ...
            if (IsDoubleTapItem() && isPickingUp)
            {
                Debug.Log("Double tap");
                isPickingUp = false; // Set isPickingUp to false
            }
        }

        // If time in game is greater than newTime, ...
        if (Time.time > newTime)
            tapCount = 0; // Reset tapCount to 0
        
        // If player isPickingUp, ...
        if (isPickingUp)
            PickUpItem(); // Pick up the item
        else // Else, ...
            ReleaseItem(); // Release the item
    }

    /// <summary>
    /// PickUpItem: When user tap on item, pick up item
    /// </summary>
    private void PickUpItem()
    {
        // Check if there are targetObject
        if (targetObject)
        {
            // Set targetObject
            targetObject.GetComponent<Rigidbody>().useGravity = false;
            targetObject.parent = itemHolder;
            targetObject.position = itemHolder.position;
        }
    }
    
    /// <summary>
    /// ReleaseItem: When user tap 2 times on item, release item
    /// </summary>
    private void ReleaseItem()
    {
        // Check if there are targetObject
        if(targetObject)
        {
            // Set targetObject to normal
            targetObject.GetComponent<Rigidbody>().useGravity = true;
            targetObject.parent = null;
            targetObject = null;
        }
    }

    /// <summary>
    /// TapItem: When user tap an "item" object
    /// </summary>
    private void TapItem()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began) // If touch began to tap
            {
                // Get ray from touch position
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayMaxDistance))
                {
                    // If touch tap item object, ...
                    if (hit.collider.CompareTag("Item"))
                    {
                        targetObject = hit.transform; // Change targetObject
                        // Uncomment lines below to debugging
                        // Debug.Log("Tap " + targetObject);
                        // Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
                        isPickingUp = true; // isPickingUp is true
                        break; // Break after get an item object
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// IsDoubleTapItem: Check if user double tap on item
    /// </summary>
    /// <returns>
    /// True if user double tap on item object
    /// </returns>
    private bool IsDoubleTapItem()
    {
        bool result = false;

        for (int i = 0; i < Input.touchCount; i++)
        {
            // If user taps, ...
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                tapCount += 1; // Increase tapCount
            }

            // If tapCount equals to 1, ...
            if (tapCount == 1)
            {
                newTime = Time.time + tapTimeLimit; // Change newTime
            }
            // If tapCount equals to 2 and time in game is smaller or equals to newTime, ...
            else if (tapCount == 2 && Time.time <= newTime)
            {
                // Set Ray and RaycastHit
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayMaxDistance))
                {
                    // If user taps on Item Object
                    if (hit.collider.CompareTag("Item"))
                    {
                        tapCount = 0; // Reset tapCount to 0
                        result = true; // Set the result
                        // Uncomment lines below to debugging
                        // Debug.Log("Release " + targetObject);
                        // Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
                        break; // Out from looping
                    }
                }
            }
        }

        return result;
    }
}
