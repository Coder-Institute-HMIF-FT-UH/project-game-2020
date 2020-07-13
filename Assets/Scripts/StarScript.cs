using UnityEngine;

public class StarScript : MonoBehaviour
{
    private float speed = 180f;
    
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0f, speed*Time.deltaTime, 0f);
    }
}
