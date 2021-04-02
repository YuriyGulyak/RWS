using UnityEngine;

namespace RWS
{
    [CreateAssetMenu]
    public class RateProfile : ScriptableObject
    {
        [Space]

        [SerializeField, Range( 0f, 1f )]
        float rollExpo = 0.2f;

        [SerializeField, Range( 0f, 1f )]
        float rollSuperExpo = 0.75f;

        [Space]

        [SerializeField, Range( 0f, 1f )]
        float pitchExpo = 0.2f;

        [SerializeField, Range( 0f, 1f )]
        float pitchSuperExpo = 0.75f;

        //--------------------------------------------------------------------------------------------------------------

        public float RollExpo
        {
            get => rollExpo;
            set
            {
                rollExpo = value;
                Init();
            }
        }

        public float RollSuperExpo
        {
            get => rollSuperExpo;
            set
            {
                rollSuperExpo = value;
                Init();
            }
        }

        public float PitchExpo
        {
            get => pitchExpo;
            set
            {
                pitchExpo = value;
                Init();
            }
        }

        public float PitchSuperExpo
        {
            get => pitchSuperExpo;
            set
            {
                pitchSuperExpo = value;
                Init();
            }
        }

        public float EvaluateRoll( float roll )
        {
            return Rates.BfCalc( roll, 1f, rollExpo, rollSuperExpo ) / maxRollValue;
        }

        public float EvaluatePitch( float pitch )
        {
            return Rates.BfCalc( pitch, 1f, pitchExpo, pitchSuperExpo ) / maxPitchValue;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField, HideInInspector]
        float maxRollValue;

        [SerializeField, HideInInspector]
        float maxPitchValue;


        void OnValidate()
        {
            Init();
        }


        void Init()
        {
            maxRollValue = Rates.BfCalc( 1f, 1f, rollExpo, rollSuperExpo );
            maxPitchValue = Rates.BfCalc( 1f, 1f, pitchExpo, pitchSuperExpo );
        }
    }
}
