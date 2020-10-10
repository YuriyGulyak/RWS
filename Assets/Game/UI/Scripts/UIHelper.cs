using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    public float scrollSensitivity = 2f;


    public void Apply()
    {
        var scrollRects = GetComponentsInChildren<ScrollRect>( true );

        foreach( var scrollRect in scrollRects )
        {
            //print( scrollRect.transform.parent.name );
            scrollRect.scrollSensitivity = scrollSensitivity;
        }
    }
}
