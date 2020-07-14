using UnityEngine;
using UnityEngine.EventSystems;

public class DragRect : MonoBehaviour, IDragHandler
{
    [SerializeField]
    RectTransform targetRect = null;

    
    public void OnDrag( PointerEventData eventData )
    {
        targetRect.anchoredPosition += eventData.delta;
    }
}