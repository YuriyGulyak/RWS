using UnityEngine;

namespace RWS
{
    public class ControlSensitivity
    {
        public ControlSensitivity( bool loadPlayerPrefs = false )
        {
            if( loadPlayerPrefs )
            {
                LoadPlayerPrefs();
            }

            UpdateMaxValues();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void LoadPlayerPrefs()
        {
            if( PlayerPrefs.HasKey( rollExpoKey ) )
            {
                rollExpo = PlayerPrefs.GetFloat( rollExpoKey );
            }

            if( PlayerPrefs.HasKey( rollSuperExpoKey ) )
            {
                rollSuperExpo = PlayerPrefs.GetFloat( rollSuperExpoKey );
            }

            if( PlayerPrefs.HasKey( pitchExpoKey ) )
            {
                pitchExpo = PlayerPrefs.GetFloat( pitchExpoKey );
            }

            if( PlayerPrefs.HasKey( pitchSuperExpoKey ) )
            {
                pitchSuperExpo = PlayerPrefs.GetFloat( pitchSuperExpoKey );
            }

            UpdateMaxValues();
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetFloat( rollExpoKey, rollExpo );
            PlayerPrefs.SetFloat( rollSuperExpoKey, rollSuperExpo );

            PlayerPrefs.SetFloat( pitchExpoKey, pitchExpo );
            PlayerPrefs.SetFloat( pitchSuperExpoKey, pitchSuperExpo );
        }


        public float RollExpo
        {
            get => rollExpo;
            set
            {
                rollExpo = value;
                UpdateMaxValues();
            }
        }

        public float RollSuperExpo
        {
            get => rollSuperExpo;
            set
            {
                rollSuperExpo = value;
                UpdateMaxValues();
            }
        }

        public float PitchExpo
        {
            get => pitchExpo;
            set
            {
                pitchExpo = value;
                UpdateMaxValues();
            }
        }

        public float PitchSuperExpo
        {
            get => pitchSuperExpo;
            set
            {
                pitchSuperExpo = value;
                UpdateMaxValues();
            }
        }


        public float EvaluateRoll( float roll )
        {
            return Rates.BfCalc( roll, 1f, rollExpo, rollSuperExpo ) / rollMaxValue;
        }

        public float EvaluatePitch( float pitch )
        {
            return Rates.BfCalc( pitch, 1f, pitchExpo, pitchSuperExpo ) / pitchMaxValue;
        }

        //--------------------------------------------------------------------------------------------------------------

        readonly string rollExpoKey = "RollExpo";
        readonly string rollSuperExpoKey = "RollSuperExpo";

        readonly string pitchExpoKey = "PitchExpo";
        readonly string pitchSuperExpoKey = "PitchSuperExpo";

        float rollExpo;
        float rollSuperExpo;
        float rollMaxValue;

        float pitchExpo;
        float pitchSuperExpo;
        float pitchMaxValue;

        void UpdateMaxValues()
        {
            rollMaxValue = Rates.BfCalc( 1f, 1f, rollExpo, rollSuperExpo );
            pitchMaxValue = Rates.BfCalc( 1f, 1f, pitchExpo, pitchSuperExpo );
        }
    }
}