using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: CLEAN CODE.
public class PickUpObject : MonoBehaviour
{
    [SerializeField] private float rayMaxDistance;
    [SerializeField] private Transform itemHolder;

    private bool isPickingUp = false;
    private float tapTimeLimit = 1f,
        newTime = 0f;
    private int tapCount = 0;
    private Transform targetObject;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchItem();
            
            if (TapCount() && isPickingUp)
            {
                Debug.Log("berhasil");
                isPickingUp = false;
            }
        }

        if (Time.time > newTime)
        {
            tapCount = 0;
        }
        
        if (isPickingUp)
        {
            PickUpItem();
        }
        else
        {
            ReleaseItem();
        }
    }

    /// <summary>
    /// ChangeTargetParent: Make itemHolder a parent of targetObject
    /// </summary>
    private void PickUpItem()
    {
        if (targetObject)
        {
            Debug.Log("Grab " + targetObject.name);
            targetObject.GetComponent<Rigidbody>().useGravity = false;
            targetObject.parent = itemHolder;
            targetObject.position = itemHolder.position;
        }
    }
    
    private void ReleaseItem()
    {
        if(targetObject)
        {
            targetObject.GetComponent<Rigidbody>().useGravity = true;
            targetObject.parent = null;
            targetObject = null;
        }
    }

    private void TouchItem()
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
                        Debug.Log("Target objek = " + targetObject);
                        isPickingUp = true; // isPickingUp is true
                        break; // Break after get an item object
                    }
                }
            }
        }
    }
    
    private bool TapCount()
    {
        bool result = false;

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                tapCount += 1;
            }

            if (tapCount == 1)
            {
                newTime = Time.time + tapTimeLimit;
                Debug.Log("New Time = " + newTime +
                          "\nTime.time = " + Time.time);
            }
            else if (tapCount == 2 && Time.time <= newTime)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayMaxDistance))
                {
                    if (hit.collider.CompareTag("Item"))
                    {
                        Debug.Log("Release");
                    }
                }
                
                tapCount = 0;
                result = true;
                break;
            }
        }

        return result;
    }
}
