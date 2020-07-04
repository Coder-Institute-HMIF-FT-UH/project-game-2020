using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Tweaks")]
    [SerializeField]
    private float joystickVisualDistance = 50f;
    
    [Header("Logic")]
    private Image joystick, handles;
    private Vector3 direction;

    public Vector3 Direction => direction;
    
    private void Start()
    {
        var imgs = GetComponentsInChildren<Image>();
        joystick = imgs[0]; // Joystick on parent object
        handles = imgs[1]; // Handle on first child
    }
    
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos = Vector2.zero;
        var joystickRectTransform = joystick.rectTransform;
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickRectTransform, ped.position,
            ped.pressEventCamera, out pos))
        {
            pos.x /= joystickRectTransform.sizeDelta.x;
            pos.y /= joystickRectTransform.sizeDelta.y;

            // Pivot might be giving us an offset, adjust it here
            Vector2 pivot = joystickRectTransform.pivot;
            pos.x += pivot.x - 0.5f;
            pos.y += pivot.y - 0.5f;
            
            // Clamp values
            float x = Mathf.Clamp(pos.x, -1, 1);
            float y = Mathf.Clamp(pos.y, -1, 1);
            direction = new Vector3(x, 0, y).normalized;

            // Move the visual to reflect the inputs
            handles.rectTransform.anchoredPosition = new Vector3(
                direction.x * joystickVisualDistance, 
                direction.z * joystickVisualDistance);
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        direction = default(Vector3);
        handles.rectTransform.anchoredPosition = default(Vector3);
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
}
