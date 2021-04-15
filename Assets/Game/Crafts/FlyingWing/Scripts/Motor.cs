using System.Threading;
using UnityEngine;

namespace RWS
{
    public class Motor : MonoBehaviour
    {
        [SerializeField]
        MotorModel motorModel = null;

        [SerializeField]
        Propeller propeller = null;

        [SerializeField]
        Transform rotorTransform = null;

        //----------------------------------------------------------------------------------------------------

        public float voltage = 16f;
        public float throttle;
        public float rpm;
        public float current; // A
        public float thrust; // N
        public float torque; // Nm

        public void UpdateState( float forwardSpeed, float voltage, float throttle )
        {
            if( propeller.isBlocked )
            {
                voltage = 0f;
                throttle = 0f;
            }

            this.voltage = voltage;
            this.throttle = throttle;

            if( throttle > 0f )
            {
                motorModel.Vin = voltage * Mathf.Lerp( 0.05f, 1f, throttle );
            }
            else
            {
                motorModel.Vin = 0f;
            }

            motorModel.Tl = torque;

            rpm = (float)motorModel.RPM;
            current = (float)motorModel.I;

            propeller.UpdateState( forwardSpeed, rpm );

            thrust = propeller.lift;
            torque = propeller.torque;
        }

        public void Reset()
        {
            rpm = 0f;
            current = 0f;
            thrust = 0f;
            torque = 0f;
            propeller.isBlocked = false;
        }

        //----------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            StartMotorModel();
        }

        void OnDisable()
        {
            StopMotorModel();
        }

        void Update()
        {
            var degPerSec = rpm / 60f * 360f;
            rotorTransform.localRotation *= Quaternion.Euler( 0f, 0f, degPerSec * Time.deltaTime );
        }

        //----------------------------------------------------------------------------------------------------

        Thread motorModelThread;

        void StartMotorModel()
        {
            motorModel.Init();

            if( motorModelThread != null && motorModelThread.IsAlive )
            {
                motorModelThread.Abort();
            }

            motorModelThread = new Thread( () =>
            {
                while( true )
                {
                    motorModel.Step( 0.001f );
                    Thread.Sleep( 1 );
                }
            } );
            motorModelThread.Start();
        }

        void StopMotorModel()
        {
            if( motorModelThread != null && motorModelThread.IsAlive )
            {
                motorModelThread.Abort();
            }
        }
    }
}
