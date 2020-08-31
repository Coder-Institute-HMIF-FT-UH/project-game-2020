using UnityEngine;
using UnityEngine.EventSystems;

public class OuterDetailsManager : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private DetailsManager detailsManager;
    
    private RectTransform outerDetailsRectArea;

    private void Start()
    {
        outerDetailsRectArea = GetComponent<RectTransform>();
    }

    /// <summary>
    /// When player click or tap 
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        // If player click on this area, ...
        if (RectTransformUtility.RectangleContainsScreenPoint(
            outerDetailsRectArea, data.position, data.pressEventCamera))
        {
            detailsManager.IsDetailsTouch = false;
        }
    }
}
