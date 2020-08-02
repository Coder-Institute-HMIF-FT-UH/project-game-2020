using System;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    // Declare variable
    Touch touchZero, touchOne;
    public float zoomOutMin = 3;
    public float zoomOutMax = 6;

    public int hintUsed = 0;

    [SerializeField] private Transform player;
    [SerializeField] private Image gps;
	

	// Update is called once per frame
    void Update()
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
                    zoom(difference * 0.01f);
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

    // Camera Zoom
    void zoom(float increment){
        this.GetComponent<Camera>().orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    // Hint reveal Stars

    public void HintUsed(){
        hintUsed++;
        if (hintUsed > 0){
        ShowStar1();
        }
        if (hintUsed > 1){
        ShowStar2();
        }
        if (hintUsed > 2){
        ShowStar3();
        }
    }

    //activate minimap culling mask layer
    public void ShowStar1() {
        
      this.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("Star 1");
    }
    public void ShowStar2() {
      this.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("Star 2");
    }
    public void ShowStar3() {
      this.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("Star 3");
    }
}
