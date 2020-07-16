using UnityEngine;

public class CursorHider : MonoBehaviour
{
    [SerializeField]
    bool hideInEditor = false;

    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        if( !Application.isEditor || hideInEditor )
        {
            Cursor.visible = false;
        }
    }

    void OnApplicationFocus( bool hasFocus )
    {
        if( Application.isEditor && !hideInEditor )
        {
            return;
        }

        Cursor.visible = !hasFocus;
    }
}