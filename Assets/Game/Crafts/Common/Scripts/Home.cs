using UnityEngine;

namespace RWS
{
    public class Home : MonoBehaviour
    {
        [SerializeField]
        FlyingWing flyingWing = default;


        public float HomeDirection => homeDirection;
        public float HomeDistance => homeDistance;


        public void Init()
        {
            if( flyingWing )
            {
                Init( flyingWing );
            }
        }

        public void Init( FlyingWing flyingWing )
        {
            this.flyingWing = flyingWing;

            wingTransform = flyingWing.Transform;
            homePosition = wingTransform.position;
        }

        public void Reset()
        {
            homeDirection = 0f;
            homeDistance = 0f;
        }


        float homeDirection;
        float homeDistance;
        Transform wingTransform;
        Vector3 homePosition;

        
        void Update()
        {
            UpdateState();
        }


        void UpdateState()
        {
            if( flyingWing && flyingWing.Speedometer.SpeedMs > 0.1f )
            {
                var wingPosition = wingTransform.position;
                var vectorToHome = homePosition - wingPosition;
                vectorToHome.y = 0f;

                homeDistance = vectorToHome.magnitude;
                if( homeDistance > 1f )
                {
                    var wingForward = wingTransform.forward;
                    wingForward.y = 0f;

                    homeDirection = MathUtils.WrapAngle180( Vector3.SignedAngle( wingForward.normalized, vectorToHome.normalized, Vector3.up ) );
                }
            }
        }
    }
}
