using UnityEngine;

public class CursorHider : MonoBehaviour
{
    [SerializeField]
    bool hideInEditor = false;

    //----------------------------------------------------------------------------------------------------

    void OnApplicationFocus( bool hasFocus )
    {
        if( Application.isEditor && !hideInEditor )
        {
            return;
        }

        Cursor.visible = !hasFocus;
    }
}