using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFPSController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;
    
    private RigidbodyFirstPersonController fps;

    // Start is called before the first frame update
    void Start()
    {
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
}
