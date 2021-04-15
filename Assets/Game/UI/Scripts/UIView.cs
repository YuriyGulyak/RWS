using UnityEngine;

public class UIView : MonoBehaviour
{
    public virtual void ShowView()
    {
        gameObject.SetActive( true );
    }

    public virtual void HideView()
    {
        gameObject.SetActive( false );
    }
}