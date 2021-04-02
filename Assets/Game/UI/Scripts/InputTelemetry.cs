using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class InputTelemetry : Singleton<InputTelemetry>
    {
        [SerializeField]
        StickDisplay leftStick = null;

        [SerializeField]
        StickDisplay rightStick = null;

        [SerializeField]
        Slider trimSlider = null;

        //----------------------------------------------------------------------------------------------------

        public void Init( InputManager inputManager )
        {
            this.inputManager = inputManager;
        }

        public void Show()
        {
            gameObject.SetActive( true );

            inputManager.ThrottleControl.Performed += OnThrottlePerformed;
            inputManager.RollControl.Performed += OnRollPerformed;
            inputManager.PitchControl.Performed += OnPitchPerformed;
            inputManager.TrimControl.Performed += OnTrimPerformed;
        }

        public void Hide()
        {
            gameObject.SetActive( false );

            inputManager.ThrottleControl.Performed -= OnThrottlePerformed;
            inputManager.RollControl.Performed -= OnRollPerformed;
            inputManager.PitchControl.Performed -= OnPitchPerformed;
            inputManager.TrimControl.Performed -= OnTrimPerformed;
        }

        //----------------------------------------------------------------------------------------------------

        InputManager inputManager;


        void OnThrottlePerformed( float value )
        {
            leftStick.Y = value;
        }

        void OnRollPerformed( float value )
        {
            rightStick.X = value;
        }

        void OnPitchPerformed( float value )
        {
            rightStick.Y = -value;
        }

        void OnTrimPerformed( float value )
        {
            trimSlider.value = value;
        }
    }
}
