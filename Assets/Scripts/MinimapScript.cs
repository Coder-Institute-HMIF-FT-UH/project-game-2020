using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] private GameObject[] hints;
    [SerializeField] private int[] distanceHint;
    [SerializeField] private bool[] isHintActive;
    
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
        }
        catch (ArgumentException) {}
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
    
    private void ShowHintCullingMask(int layer)
    {
        // Turn on layer culling mask bit using an OR operation
        minimapCamera.cullingMask |= 1 << layer;
    }

    /// <summary>
    /// Show hints
    /// </summary>
    public void ShowHints()
    {
        Debug.Log("Hint pressed");
        hintLeft--;
        DecideStarReveal();
    }

    private void DecideStarReveal()
    {
        // Get stars distance to player if the stars aren't destroyed
        for (int i = 0; i < hints.Length; i++)
        {
            if (hints[i] && !isHintActive[i])
            {
                distanceHint[i] = Mathf.RoundToInt(Vector3.Distance(hints[i].transform.position, player.position));
            }
        }
        
        // Get the index of lowest distance
        int minDistanceIndex = Array.IndexOf(distanceHint, distanceHint.Min());
        int layer = hints[minDistanceIndex].layer; // Get layer

        if (!isHintActive[minDistanceIndex])
        {
            isHintActive[minDistanceIndex] = true;
            distanceHint[minDistanceIndex] = 1000;
            ShowHintCullingMask(layer);
        }
    }
}