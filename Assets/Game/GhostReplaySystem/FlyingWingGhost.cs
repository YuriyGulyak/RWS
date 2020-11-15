using UnityEngine;

public class FlyingWingGhost : MonoBehaviour
{
    [SerializeField]
    Transform craftTransform = null;

    [SerializeField]
    Transform rotorTransform = null;


    public void SetState( Vector3 craftPosition, Vector3 craftRotation, float motorRpm )
    {
        this.craftPosition = craftPosition;
        this.craftRotation = craftRotation;
        this.motorRpm = motorRpm;
    }

    
    Vector3 craftPosition;
    Vector3 craftRotation;
    float motorRpm;
    
    void Update()
    {
        craftTransform.position = craftPosition;
        craftTransform.eulerAngles = craftRotation;
        
        var degPerSec = motorRpm / 60f * 360f;
        rotorTransform.localRotation *= Quaternion.Euler( 0f, 0f, degPerSec * Time.deltaTime );
    }
}