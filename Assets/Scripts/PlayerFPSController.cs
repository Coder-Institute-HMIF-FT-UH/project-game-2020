using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFPSController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;
    
    private RigidbodyFirstPersonController fps;
    private int starsCount;

    // Start is called before the first frame update
    void Start()
    {
        starsCount = 0;
        fps = GetComponent<RigidbodyFirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move Player
        fps.RunAxis = moveJoystick.Direction;
        fps.JumpAxis = jumpButton.pressed;
        fps.mouseLook.LookAxis = touchField.touchDist;
    }

    /// <summary>
    /// OnTriggerEnter: When Player collide with other collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // If collide with star, ...
        if (other.gameObject.CompareTag("Star"))
        {
            starsCount += 1; // Increase starsCount
            
            // Add disappear particle
            
            other.gameObject.SetActive(false); // Set star to non-active
            
            Debug.Log("Stars collected = " + starsCount);
        }
    }
}
