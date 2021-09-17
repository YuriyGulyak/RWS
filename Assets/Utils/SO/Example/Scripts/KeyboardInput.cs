using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField]
    FloatVariable horizontalAxis = default;

    [SerializeField]
    FloatVariable verticalAxis = default;

    [SerializeField]
    GameEvent spaceKeyEvent = default;
    

    void Update()
    {
        horizontalAxis.Value = Input.GetAxis( "Horizontal" );
        verticalAxis.Value = Input.GetAxis( "Vertical" );

        if( Input.GetKey( KeyCode.Space ) )
        {
            spaceKeyEvent.Raise();
        }
    }
}