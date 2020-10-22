using UnityEngine;

public class AttitudeIndicator : MonoBehaviour
{
    [SerializeField]
    RectTransform horizonTransform = null;

    [SerializeField]
    RectTransform aircraftSymbolTransform = null;

    [SerializeField]
    float cameraVerticalFOV = 90f;
    
    [SerializeField]
    FlyingWing flyingWing = null;
    
    
    public void Init( FlyingWing flyingWing )
    {
        this.flyingWing = flyingWing;
    }

    public bool IsActive => gameObject.activeSelf;
    
    public void Show()
    {
        if( !IsActive )
        {
            gameObject.SetActive( true );
        }
    }

    public void Hide()
    {
        if( IsActive )
        {
            gameObject.SetActive( false );
        }
    }

    public void Reset()
    {
        roll = 0f;
        pitch = 0f;

        var horizonPosition = horizonTransform.anchoredPosition;
        horizonPosition.y = 0f;
        horizonTransform.anchoredPosition = horizonPosition;

        var aircraftSymbolAngles = aircraftSymbolTransform.eulerAngles;
        aircraftSymbolAngles.z = 0f;
        aircraftSymbolTransform.eulerAngles = aircraftSymbolAngles;
    }
    
    
    float pixelsInDegree;
    float roll; // For debug 
    float pitch;
    

    void Awake()
    {
        // TODO There will be problems if change the resolution during the game
        pixelsInDegree = Screen.height / cameraVerticalFOV;
    }

    void LateUpdate()
    {
        if( !flyingWing )
        {
            return;
        }

        roll = flyingWing.RollAngle;
        pitch = flyingWing.PitchAngle;

        var horizonPosition = horizonTransform.anchoredPosition;
        horizonPosition.y = pixelsInDegree * -flyingWing.PitchAngle;
        horizonTransform.anchoredPosition = horizonPosition;

        var aircraftSymbolAngles = aircraftSymbolTransform.eulerAngles;
        aircraftSymbolAngles.z = flyingWing.RollAngle;
        aircraftSymbolTransform.eulerAngles = aircraftSymbolAngles;
    }
}
