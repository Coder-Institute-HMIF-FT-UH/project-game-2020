using System;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    public GameObject star1, star2, star3;
    private int distanceStar1 = 10000, distanceStar2 = 10000, distanceStar3 = 10000;
    private bool active1, active2, active3;

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
        active1 = false;
        active2 = false; 
        active3 = false;
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
        }
        catch (ArgumentException)
        {
        }
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
    /// Zoom mini map
    /// </summary>
    /// <param name="increment"></param>
    private void Zoom(float increment)
    {
        minimapCamera.orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize - increment,
            zoomOutMin, zoomOutMax);
    }

    /// <summary>
    /// Activate cullingMask with layerName
    /// </summary>
    /// <param name="layerName"></param>
    private void ShowStar(string layerName)
    {
        //// Turn on layer culling mask bit using an OR operation
        minimapCamera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
    }

    /// <summary>
    /// Show hints
    /// </summary>
    public void ShowHints()
    {
        Debug.Log("Hint pressed");
        hintLeft--;
        DecideStarReveal();
        // switch (hintLeft)
        // {
        //     case 2:
        //         SwitchStars();
        //         break;
        //     case 1:
        //         SwitchStars();
        //         break;
        //     case 0:
        //         SwitchStars();
        //         break;
        //     default:
        //         hintLeft = 0;
        //         break;
        // }
    }

    // private void SwitchStars()
    // {
    //     if (starsRevealed > 0)
    //     {
    //         ShowStar("Star " + starsRevealed);
    //         starsRevealed--;
    //     }
    //     else
    //         starsRevealed = 0;
    //

    private void DecideStarReveal()
    {
        // get stars distance to player if the stars arent destroyed
        if (star1 != null && active1 == false)
        {
            distanceStar1 = Mathf.RoundToInt(Vector3.Distance(star1.transform.position, player.position));
        }

        if (star2 != null && active2 == false)
        {
            distanceStar2 = Mathf.RoundToInt(Vector3.Distance(star2.transform.position, player.position));
        }

        if (star3 != null && active3 == false)
        {
            distanceStar3 = Mathf.RoundToInt(Vector3.Distance(star3.transform.position, player.position));
        }

        // decide which star layer to activate
        if (distanceStar1 <= distanceStar2 && distanceStar1 <= distanceStar3 && active1 == false)
            {
                distanceStar1 = 10000; //reset the distance
                active1 = true; //save that 1st star layer is activated
                ShowStar("Star 1");
            }
        
        else if (distanceStar2 <= distanceStar3 && active2 == false)
        {
            distanceStar2 = 10000; //reset the distance
            active2 = true;        //save that 1st star layer is activated
            ShowStar("Star 2");
        }
        
        else if (active3 == false)
        {
            distanceStar3 = 10000; //reset the distance
            active3 = true;        //save that 1st star layer is activated
            ShowStar("Star 3");
        }
    }
}