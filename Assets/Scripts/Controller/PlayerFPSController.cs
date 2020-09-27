using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFPSController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;
    [SerializeField] private LoadLevelManager loadLevelManager;
    
    private RigidbodyFirstPersonController fps;
    private int starsCount;
    private string levelName;
    
    public int StarsCount => starsCount;

    // Start is called before the first frame update
    private void Start()
    {
        // Get prefs stars in currentScene's name
        starsCount = PlayerPrefs.GetInt(PlayerPrefsConstant.StarsTaken + loadLevelManager.LevelName);
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
            Debug.Log("Stars collected = " + starsCount);
            
            // Set star's renderer and collider to false
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<SphereCollider>().enabled = false;
            // Destroy star's particle system
            Destroy(other.GetComponentInChildren<ParticleSystem>());
        }
    }
}
