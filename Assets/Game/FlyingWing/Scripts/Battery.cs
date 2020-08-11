// References --------------------------------------------------------------------------------------------
// http://www.thejumperwire.com/science/lipo-characteristics-part-2-electrical-properties/
// http://www.thejumperwire.com/science/lipo-characteristics-part-3-internal-resistance/
//--------------------------------------------------------------------------------------------------------


// Math model --------------------------------------------------------------------------------------------
//        +----------------------------------------+
//        |                       +---[ C1 ]---+   |
//        |                       |            |   |
// (–)--------| –  + |---[ R1 ]---+            +--------(+)
//        |                       |            |   |
//        |                       +---[ R2 ]---+   |
//        +----------------------------------------+
// A fully charged capacitor acts like a short circuit, and a fully discharged capacitor acts like an open circuit. 
// When the battery is disconnected from a load, the capacitor begins charging. When a load is connected, the capacitor begins discharging. 
// So, when the load is first connected, R2 is completely bypassed. 
// As the capacitor discharges, the current begins flowing down both branches and resistance increases.
//--------------------------------------------------------------------------------------------------------


using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField]
    int cellCount = 4;

    [SerializeField]
    float maxCapacity = 4f; // Ah

    [SerializeField]
    float internalResistance = 0.01f; // Ohms per cell
    
    [SerializeField]
    AnimationCurve cellVoltage = AnimationCurve.Linear( 0f, 4.2f, 1f, 0f ); // Resting voltage vs State of charge

    [SerializeField]
    float minVoltage = 1f;

    [SerializeField]
    bool infiniteCapacity = false;
    
    //----------------------------------------------------------------------------------------------------

    public int CellCount => cellCount;

    // Ah
    public float MaxCapacity => maxCapacity;

    // Ah
    public float CapacityDrawn => capacityDrawn;

    // Volts
    public float Voltage => voltage;

    // 1...0
    public float StateOfCharge => stateOfCharge;
    
    public void UpdateState( float currentDraw, float deltaTime )
    {
        capacityDrawn += currentDraw / 3600f * deltaTime;
        
        if( infiniteCapacity == false )
        {
            // 1...0
            stateOfCharge = Mathf.InverseLerp( maxCapacity, 0f, capacityDrawn );
        }

        var voltageDrop = currentDraw * internalResistance * cellCount;
        voltage = cellVoltage.Evaluate( 1f - stateOfCharge ) * cellCount - voltageDrop;
        voltage = Mathf.Max( minVoltage, voltage );
    }

    public void Reset()
    {
        capacityDrawn = 0f;
        stateOfCharge = 1f;
        voltage = cellVoltage.Evaluate( 0f ) * cellCount;
    }

    //----------------------------------------------------------------------------------------------------

    // TODO Temporary solution
    readonly string infiniteBatteryKey = "InfiniteBattery";
    
    float capacityDrawn;
    float stateOfCharge;
    float voltage;
    
    
    void Awake()
    {
        stateOfCharge = 1f;
        voltage = cellVoltage.Evaluate( 0f ) * cellCount;

        // TODO Temporary solution
        if( PlayerPrefs.HasKey( infiniteBatteryKey ) )
        {
            infiniteCapacity = PlayerPrefs.GetInt( infiniteBatteryKey ) > 0;
        }
    }
}