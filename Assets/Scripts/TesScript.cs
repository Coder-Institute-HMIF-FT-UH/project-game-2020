using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just a test script dont mind it
public class TesScript : MonoBehaviour
{
    private int tapCount = 0;

    private float newTime, maxTapTime = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    tapCount += 1;
                }

                if (tapCount == 1)
                {
                    newTime = Time.time + maxTapTime;
                    Debug.Log("New Time = " + newTime + 
                              "\nTime.time = " + Time.time);
                }
                else if (tapCount == 2 && Time.time <= newTime)
                {
                    Debug.Log("Double Tap");
                    tapCount = 0;
                }
            }
        }
        
        if (Time.time > newTime)
        {
            tapCount = 0;
        }
    }
}
