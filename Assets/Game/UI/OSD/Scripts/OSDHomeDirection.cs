using UnityEngine;

namespace RWS
{
    public class OSDHomeDirection : MonoBehaviour
    {
        [SerializeField]
        RectTransform directionRect = default;

        [SerializeField]
        FloatVariable homeDirection = default;

        [SerializeField]
        FloatVariable rollVariable = default;

        [SerializeField]
        float updateRate = 30f;

        
        float lastUpdateTime;
        float directionArrowAngle;
        

        void Update()
        {
            var time = Time.time;
            if( time - lastUpdateTime > 1f / updateRate )
            {
                lastUpdateTime = time;
                UpdateArrowAngle();
            }

            var directionRectAngles = directionRect.eulerAngles;
            directionRectAngles.z = Mathf.LerpAngle( directionRectAngles.z, directionArrowAngle, Time.deltaTime * 10f );
            directionRect.eulerAngles = directionRectAngles;
        }


        void UpdateArrowAngle()
        {
            directionArrowAngle = ( homeDirection.Value + rollVariable.Value ) * -1f;
        }
    }
}
