using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private float rayMaxDistance;
    [SerializeField] private Transform itemHolder;

    private bool isPickingUp = false;
    private float tapTimeLimit = 1f, variancePosition = 1f;
    private int tapCount = 0;
    private Transform targetObject;

    private void Update()
    {
        if (Input.touchCount > 0)
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

        // if(Input.touchCount > 0)
        // {
        //     TapCount(0);
        // }
        
        if (isPickingUp)
        {
            ChangeTargetParent();
        }
        // else
        // {
        //     if (Input.touchCount > 0)
        //     {
        //         for (int i = 0; i < Input.touchCount; i++)
        //         {
        //             if (Input.GetTouch(i).phase == TouchPhase.Began)
        //             {
        //                 Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
        //                 RaycastHit hit;
        //
        //                 if (Physics.Raycast(ray, out hit, rayMaxDistance))
        //                 {
        //                     if (hit.collider.CompareTag("Item"))
        //                     {
        //                         // Debug.Log("Release");
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }
    }

    /// <summary>
    /// ChangeTargetParent: Make itemHolder a parent of targetObject
    /// </summary>
    private void ChangeTargetParent()
    {
        if (targetObject)
        {
            Debug.Log("Grab " + targetObject.name);
            targetObject.GetComponent<Rigidbody>().useGravity = false;
            targetObject.GetComponent<Collider>().enabled = false;
            targetObject.parent = itemHolder;
            targetObject.position = itemHolder.position;
        }
        else
        {
            Debug.Log(targetObject.name + " is null");
            targetObject.GetComponent<Rigidbody>().useGravity = true;
            targetObject.GetComponent<Collider>().enabled = true;
            targetObject.parent = null;
        }
    }

    // This method is still wrong
    private bool TapCount(int index)
    {
        bool result = false;
        
        if (Input.GetTouch(index).phase == TouchPhase.Began)
        {
            float deltaTime = Input.GetTouch(index).deltaTime;
            float deltaPositionLength = Input.GetTouch(index).deltaPosition.magnitude;
            tapCount += 1;

            Debug.Log("Delta time = " + deltaTime +
                      "\nTap Count = " + tapCount + 
                      "\nDelta Position = " + deltaPositionLength);

            if (deltaTime > 0 && deltaTime < tapTimeLimit && deltaPositionLength < variancePosition && tapCount == 2)
            {
                Debug.Log("Double Tap");
                result = true;
                tapCount = 0;
            }
        }

        return result;
    }
}
