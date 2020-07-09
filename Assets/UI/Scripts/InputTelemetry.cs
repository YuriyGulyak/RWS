using UnityEngine;
using UnityEngine.UI;

public class InputTelemetry : MonoBehaviour
{
    [SerializeField] 
    StickDisplay leftStick = null;
    
    [SerializeField] 
    StickDisplay rightStick = null;
    
    [SerializeField] 
    Slider trimSlider = null;


    void Awake()
    {
        var playerInput = PlayerInputWrapper.Instance;
        
        playerInput.Throttle.AddListener( value => leftStick.Y = Mathf.Lerp( -1f, 1f, value ) );
        playerInput.Roll.AddListener( value => rightStick.X = value );
        playerInput.Pitch.AddListener( value => rightStick.Y = -value );
        playerInput.Trim.AddListener( value => trimSlider.value = value );
    }
}
