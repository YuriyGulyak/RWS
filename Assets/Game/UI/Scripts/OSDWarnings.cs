using UnityEngine;

namespace RWS
{
    public class OSDWarnings : MonoBehaviour
    {
        [SerializeField]
        FlyingWing flyingWing = null;

        [SerializeField]
        CanvasGroup[] voltageIndicators = null;

        [SerializeField]
        CanvasGroup voltageWarningText = null;

        [SerializeField]
        CanvasGroup signalIndicator = null;

        [SerializeField]
        float blinkFrequency = 3.5f;

        //--------------------------------------------------------------------------------------------------------------

        public void Init( FlyingWing flyingWing )
        {
            this.flyingWing = flyingWing;
        }

        public void Reset()
        {
            voltageWarningText.alpha = 0f;
            foreach( var batteryIndicator in voltageIndicators )
            {
                batteryIndicator.alpha = 1f;
            }

            signalIndicator.alpha = 1f;

            prevVoltageStatus = VoltageStatus.Ok;
            prevSignalStatus = SignalStatus.Ok;
        }

        //--------------------------------------------------------------------------------------------------------------

        CustomUpdate customUpdate;
        VoltageStatus prevVoltageStatus;
        SignalStatus prevSignalStatus;
        float alpha;


        void Awake()
        {
            voltageWarningText.alpha = 0f;

            customUpdate = new CustomUpdate( blinkFrequency );
            customUpdate.OnUpdate += OnUpdate;
        }

        void Update()
        {
            customUpdate.Update( Time.time );
        }


        void OnUpdate( float deltatime )
        {
            if( !flyingWing )
            {
                return;
            }

            alpha = alpha > 0f ? 0f : 1f;


            // Voltage

            var voltageStatus = flyingWing.Battery.VoltageStatus;
            if( voltageStatus == VoltageStatus.Critical )
            {
                voltageWarningText.alpha = alpha;
                foreach( var batteryIndicator in voltageIndicators )
                {
                    batteryIndicator.alpha = alpha;
                }
            }
            else if( voltageStatus == VoltageStatus.Warning )
            {
                if( prevVoltageStatus != voltageStatus )
                {
                    voltageWarningText.alpha = 0f;
                }

                foreach( var batteryIndicator in voltageIndicators )
                {
                    batteryIndicator.alpha = alpha;
                }
            }
            else if( voltageStatus == VoltageStatus.Ok )
            {
                if( prevVoltageStatus != voltageStatus )
                {
                    voltageWarningText.alpha = 0f;
                    foreach( var batteryIndicator in voltageIndicators )
                    {
                        batteryIndicator.alpha = 1f;
                    }
                }
            }

            prevVoltageStatus = voltageStatus;


            // Signal

            var signalStatus = flyingWing.Transceiver.SignalStatus;
            if( signalStatus == SignalStatus.Ok )
            {
                if( prevSignalStatus != signalStatus )
                {
                    signalIndicator.alpha = 1f;
                }
            }
            else
            {
                signalIndicator.alpha = alpha;
            }

            prevSignalStatus = signalStatus;
        }
    }
}