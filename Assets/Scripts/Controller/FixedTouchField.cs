using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 touchDist;
    [HideInInspector]
    public Vector2 pointerOld;
    
    [HideInInspector]
    public bool pressed;
    protected int PointerId;

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                touchDist = Input.touches[PointerId].position - pointerOld;
                pointerOld = Input.touches[PointerId].position;
            }
            else
            {
                touchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pointerOld;
                pointerOld = Input.mousePosition;
            }
        }
        else
        {
            touchDist = new Vector2();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        PointerId = eventData.pointerId;
        pointerOld = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
    
}
