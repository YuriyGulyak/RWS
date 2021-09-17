using UnityEngine;

namespace RWS
{
    public class OSDWarnings : MonoBehaviour
    {
        [SerializeField]
        StatusVariable voltageStatus = default;
        
        [SerializeField]
        StatusVariable signalStatus = default;
        
        [SerializeField]
        CanvasGroup[] voltageIndicators = default;

        [SerializeField]
        CanvasGroup voltageWarningText = default;

        [SerializeField]
        CanvasGroup signalIndicator = default;

        [SerializeField]
        float blinkFrequency = 3.5f;

        //--------------------------------------------------------------------------------------------------------------

        public void Reset()
        {
            voltageWarningText.alpha = 0f;
            foreach( var batteryIndicator in voltageIndicators )
            {
                batteryIndicator.alpha = 1f;
            }

            signalIndicator.alpha = 1f;

            prevVoltageStatus = Status.Ok;
            prevSignalStatus = Status.Ok;
        }

        //--------------------------------------------------------------------------------------------------------------

        CustomUpdate customUpdate;
        Status prevVoltageStatus;
        Status prevSignalStatus;
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
            alpha = alpha > 0f ? 0f : 1f;


            // Voltage

            if( voltageStatus.Value == Status.Critical )
            {
                voltageWarningText.alpha = alpha;
                foreach( var batteryIndicator in voltageIndicators )
                {
                    batteryIndicator.alpha = alpha;
                }
            }
            else if( voltageStatus.Value == Status.Warning )
            {
                if( prevVoltageStatus != voltageStatus.Value )
                {
                    voltageWarningText.alpha = 0f;
                }

                foreach( var batteryIndicator in voltageIndicators )
                {
                    batteryIndicator.alpha = alpha;
                }
            }
            else if( voltageStatus.Value == Status.Ok )
            {
                if( prevVoltageStatus != voltageStatus.Value )
                {
                    voltageWarningText.alpha = 0f;
                    foreach( var batteryIndicator in voltageIndicators )
                    {
                        batteryIndicator.alpha = 1f;
                    }
                }
            }

            prevVoltageStatus = voltageStatus.Value;


            // Signal
            
            if( signalStatus.Value == Status.Ok )
            {
                if( prevSignalStatus != signalStatus.Value )
                {
                    signalIndicator.alpha = 1f;
                }
            }
            else
            {
                signalIndicator.alpha = alpha;
            }

            prevSignalStatus = signalStatus.Value;
        }
    }
}