using System;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Image gps;
    [SerializeField] private int hintLeft = 0;
    [SerializeField] private int starsRevealed = 3;
    
    private Camera minimapCamera;
    private Touch touchZero, touchOne;
    private float zoomOutMin = 3,
        zoomOutMax = 6;

    public int HintLeft
    {
        get => hintLeft;
        set => hintLeft = value;
    }

    public int StarsRevealed
    {
        get => starsRevealed;
        set => starsRevealed = value;
    }

    private void Start()
    {
        // Take hint left from player prefs
        hintLeft = 3;
        minimapCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        try
        {
            if (gps.gameObject.activeInHierarchy)
            {
                // If touches are equals to two, then ...
                if (Input.touchCount == 2)
                {
                    // Get first touch
                    touchZero = Input.GetTouch(0);
                    // Get the second touch
                    touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    // Zoom Camera
                    Zoom(difference * 0.01f);
                }
            }
        } catch(ArgumentException){}
    }


    private void LateUpdate()
    {
        // Minimap follow Player's position
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        
        // Minimap follow Player's rotation.y
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }

    /// <summary>
    /// GpsOn: Activate GPS
    /// </summary>
    public void GpsOn()
    {
        gps.gameObject.SetActive(true);
    }

    /// <summary>
    /// GpsOff: Deactivate GPS
    /// </summary>
    public void GpsOff()
    {
        gps.gameObject.SetActive(false);
    }

    /// <summary>
    /// Zoom mini map
    /// </summary>
    /// <param name="increment"></param>
    private void Zoom(float increment){
        minimapCamera.orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    /// <summary>
    /// Activate cullingMask with layerName
    /// </summary>
    /// <param name="layerName"></param>
    private void ShowStar(string layerName) {
        minimapCamera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
    }
    
    /// <summary>
    /// Show hints
    /// </summary>
    public void ShowHints(){
        hintLeft--;

        switch (hintLeft)
        {
            case 2:
                SwitchStars();
                break;
            case 1:
                SwitchStars();
                break;
            case 0:
                SwitchStars();
                break;
            default:
                hintLeft = 0;
                break;
        }

        starsRevealed--;
    }
    
    private void SwitchStars()
    {
        switch (starsRevealed)
        {
            case 3:
                ShowStar("Star 1");
                break;
            case 2:
                ShowStar("Star 2");
                break;
            case 1:
                ShowStar("Star 3");
                break;
            default:
                starsRevealed = 0;
                break;
        }
    }
}
