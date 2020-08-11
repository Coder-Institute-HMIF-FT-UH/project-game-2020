using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFPSController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;
    [SerializeField] private MinimapScript minimapScript;
    
    private RigidbodyFirstPersonController fps;
    private int starsCount;
    public int StarsCount => starsCount;

    // Start is called before the first frame update
    private void Start()
    {
        starsCount = 0;
        fps = GetComponent<RigidbodyFirstPersonController>();
    }

    // Update is called once per frame
    private void Update()
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
            
            Destroy(other.gameObject); // Set star to non-active
            
            Debug.Log("Stars collected = " + starsCount);
        }
    }
}
