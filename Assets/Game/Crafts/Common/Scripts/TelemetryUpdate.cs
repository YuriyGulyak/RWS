using RWS;
using UnityEngine;

public class TelemetryUpdate : MonoBehaviour
{
    [SerializeField] FlyingWing craft = default;
    
    [SerializeField] FloatVariable altitude = default;
    [SerializeField] FloatVariable speedMs = default;
    [SerializeField] FloatVariable topSpeedMs = default;
    [SerializeField] FloatVariable rollDeg = default;
    [SerializeField] FloatVariable pitchDeg = default;
    [SerializeField] FloatVariable flytimeSec = default;
    [SerializeField] FloatVariable signalStrength = default;
    [SerializeField] StatusVariable signalStatus = default;
    [SerializeField] FloatVariable voltage = default;
    [SerializeField] FloatVariable cellVoltage = default;
    [SerializeField] StatusVariable voltageStatus = default;
    [SerializeField] FloatVariable capacityDrawn = default;
    [SerializeField] FloatVariable throttle = default;
    [SerializeField] FloatVariable motorRPM = default;
    [SerializeField] FloatVariable currentDraw = default;
    [SerializeField] FloatVariable homeDirection = default;
    [SerializeField] FloatVariable homeDistance = default;

    void Update()
    {
        altitude.Value = craft.Altitude;
        speedMs.Value = craft.Speedometer.SpeedMs;
        topSpeedMs.Value = craft.Speedometer.TopSpeedMs;
        rollDeg.Value = craft.RollDeg;
        pitchDeg.Value = craft.PitchDeg;
        flytimeSec.Value = craft.Flytime;
        signalStrength.Value = craft.Transceiver.SignalStrength;
        voltage.Value = craft.Battery.Voltage;
        cellVoltage.Value = craft.Battery.CellVoltage;
        voltageStatus.Value = craft.Battery.VoltageStatus;
        capacityDrawn.Value = craft.Battery.CapacityDrawn;
        throttle.Value = craft.Throttle;
        motorRPM.Value = craft.Motor.rpm;
        currentDraw.Value = craft.CurrentDraw;
        homeDirection.Value = craft.HomeDirection;
        homeDistance.Value = craft.HomeDistance;
        signalStatus.Value = craft.Transceiver.SignalStatus;
    }
}
