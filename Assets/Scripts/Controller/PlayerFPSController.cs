using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFPSController : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;
    [SerializeField] private MinimapScript minimapScript;
    
    private RigidbodyFirstPersonController fps;
    // private Scene currentScene;
    private int starsCount;
    private string currentSceneName;
    
    public int StarsCount => starsCount;

    // Start is called before the first frame update
    private void Start()
    {
        currentSceneName =  sceneLoader.CurrentScene.name;
        // Get prefs stars in currentScene's name
        starsCount = PlayerPrefs.GetInt("stars" + currentSceneName);
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
            PlayerPrefs.SetInt("stars" + currentSceneName, starsCount);
            
            Destroy(other.gameObject); // Set star to non-active
            
            Debug.Log("Stars collected = " + starsCount);
        }
    }
}
