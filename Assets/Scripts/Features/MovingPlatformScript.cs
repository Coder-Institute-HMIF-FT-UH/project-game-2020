using System;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [Tooltip("Set 2 indexes.\n0 is starting position.\n1 is finish one.")]
    [SerializeField] private Vector3[] platformPositions;
    [SerializeField] private float platformSpeed;
    private int currentDestination = 1;
    
    private enum SameAxis { XAxis, YAxis, ZAxis }
    [Tooltip("Set which axis will move")]
    [SerializeField] private SameAxis moveAxis;
    
    private void Awake()
    {
        Vector3 currentPosition = transform.position;
        
        // Set all axes of platformPosition (except moveAxis) equals to
        // currentPosition axes
        for(int i=0; i<platformPositions.Length; i++)
        {
            switch (moveAxis)
            {
                case SameAxis.XAxis:
                    platformPositions[i].y = currentPosition.y;
                    platformPositions[i].z = currentPosition.z;
                    break;
                case SameAxis.YAxis:
                    platformPositions[i].x = currentPosition.x;
                    platformPositions[i].z = currentPosition.z;
                    break;
                case SameAxis.ZAxis:
                    platformPositions[i].x = currentPosition.x;
                    platformPositions[i].y = currentPosition.y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void FixedUpdate()
    {
        // Move platform
        transform.position = Vector3.MoveTowards(transform.position, 
            platformPositions[currentDestination],
            Time.fixedDeltaTime * platformSpeed);
        
        // Move to the opposite position
        if (transform.position == platformPositions[currentDestination])
        {
            currentDestination = currentDestination == 1 ? 0 : 1;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Set other object to this object's child
        other.transform.parent = transform;
    }

    private void OnCollisionExit(Collision other)
    {
        // Set other object to this object's child
        other.transform.parent = null;
    }
}
