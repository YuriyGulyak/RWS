using UnityEngine;

namespace RWS
{
    public class Elevon : MonoBehaviour
    {
        [SerializeField]
        Transform elevonTransform = default;

        public float Angle
        {
            get => angle;
            set
            {
                angle = value;
                UpdateElevonTransform();
            }
        }

        float angle;

        void UpdateElevonTransform()
        {
            var elevonAngles = elevonTransform.localEulerAngles;
            elevonAngles.x = angle;
            elevonTransform.localEulerAngles = elevonAngles;
        }
    }
}