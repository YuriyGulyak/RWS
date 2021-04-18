using UnityEngine;
using UnityEngine.EventSystems;

public class DragRect : MonoBehaviour, IDragHandler
{
    [SerializeField]
    RectTransform targetRect = null;

    [SerializeField]
    Canvas canvas = null;
    
    
    public void OnDrag( PointerEventData eventData )
    {
        targetRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


    void OnValidate()
    {
        if( !targetRect )
        {
            targetRect = GetComponent<RectTransform>();
        }
        if( !canvas )
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }
}