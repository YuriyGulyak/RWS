using UnityEngine;

namespace RWS
{
    [CreateAssetMenu]
    public class AirfoilConfig : ScriptableObject
    {
        // Drag coefficient vs Angle of attack
        public AnimationCurve CxVsAlpha;

        // Lift coefficient vs Angle of attack
        public AnimationCurve CyVsAlpha;

        // Center Of Pressure vs Angle of attack
        public AnimationCurve CpVsAlpha;

        // Reflex coefficient vs Angle of attack
        public AnimationCurve ReflexVsAlpha;
    }
}