using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFPSController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;
    [SerializeField] private LoadLevelManager loadLevelManager;
    [SerializeField] private MinimapScript minimapScript;
    
    private RigidbodyFirstPersonController fps;
    private int starsCount;
    private string levelName;
    
    public int StarsCount => starsCount;

    // Start is called before the first frame update
    private void Start()
    {
        // Get prefs stars in currentScene's name
        starsCount = PlayerPrefs.GetInt("stars" + loadLevelManager.LevelName);
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
        if (!other.gameObject.CompareTag("Star")) return;
        starsCount += 1; // Increase starsCount
        
        // Set prefs
        PlayerPrefs.SetInt("stars" + loadLevelManager.LevelName, starsCount);
        // Set asset
        loadLevelManager.levelDetails.starsTaken = starsCount;
        
        Destroy(other.gameObject); // Set star to non-active
            
        Debug.Log("Stars collected = " + starsCount);
    }
}
