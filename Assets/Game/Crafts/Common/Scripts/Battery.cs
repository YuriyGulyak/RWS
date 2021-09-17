// References --------------------------------------------------------------------------------------------
// http://www.thejumperwire.com/science/lipo-characteristics-part-2-electrical-properties/
// http://www.thejumperwire.com/science/lipo-characteristics-part-3-internal-resistance/
//--------------------------------------------------------------------------------------------------------

// Plans for the future:
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

namespace RWS
{
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

        const float warningCellVoltage = 3.4f;
        const float criticalCellVoltage = 3.3f;

        //----------------------------------------------------------------------------------------------------

        public int CellCount => cellCount;

        // Ah
        public float MaxCapacity => maxCapacity;

        // Ah
        public float CapacityDrawn => capacityDrawn;

        // Volts
        public float Voltage => voltage;

        public float CellVoltage => averageCellVoltage;

        // 1...0
        public float StateOfCharge => stateOfCharge;

        public Status VoltageStatus => voltageStatus;

        public bool InfiniteCapacity
        {
            get => infiniteCapacity;
            set => infiniteCapacity = value;
        }


        public void UpdateState( float currentDraw, float deltaTime )
        {
            capacityDrawn += currentDraw / 3600f * deltaTime;

            if( !infiniteCapacity )
            {
                // 1...0
                stateOfCharge = Mathf.InverseLerp( maxCapacity, 0f, capacityDrawn );
            }

            var voltageDrop = currentDraw * internalResistance * cellCount;
            voltage = cellVoltage.Evaluate( 1f - stateOfCharge ) * cellCount - voltageDrop;
            voltage = Mathf.Max( minVoltage, voltage );
            averageCellVoltage = voltage / cellCount;

            smoothedCellVoltage = Mathf.Lerp( smoothedCellVoltage, averageCellVoltage, deltaTime * 5f );

            if( smoothedCellVoltage > warningCellVoltage )
            {
                voltageStatus = Status.Ok;
            }
            else if( smoothedCellVoltage > criticalCellVoltage )
            {
                voltageStatus = Status.Warning;
            }
            else
            {
                voltageStatus = Status.Critical;
            }
        }

        public void Reset()
        {
            capacityDrawn = 0f;
            stateOfCharge = 1f;
            voltage = cellVoltage.Evaluate( 0f ) * cellCount;
            averageCellVoltage = voltage / cellCount;
            smoothedCellVoltage = averageCellVoltage;
            voltageStatus = Status.Ok;
        }

        //----------------------------------------------------------------------------------------------------

        float capacityDrawn;
        float stateOfCharge;
        float voltage;
        float averageCellVoltage;
        float smoothedCellVoltage;
        Status voltageStatus;


        void Awake()
        {
            stateOfCharge = 1f;
            voltage = cellVoltage.Evaluate( 0f ) * cellCount;
            averageCellVoltage = voltage / cellCount;
            smoothedCellVoltage = averageCellVoltage;
            voltageStatus = Status.Ok;
        }
    }
}