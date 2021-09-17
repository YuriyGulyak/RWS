using UnityEngine;

namespace RWS
{
    public class Altimeter : MonoBehaviour
    {
        public Transform targetTransform;
        public LayerMask terrainLayer;


        /// <summary>Meters</summary>
        public float Altitude => altitude;

        
        float terrainHeight;
        float altitude;


        void OnValidate()
        {
            if( !targetTransform )
            {
                targetTransform = GetComponent<Transform>();
            }
        }


        public void CalcTerrainHeight()
        {
            if( Physics.Raycast( targetTransform.position, Vector3.down, out var hit, 1000f, terrainLayer, QueryTriggerInteraction.Ignore ) )
            {
                terrainHeight = hit.point.y;
            }
        }

        public void UpdateStaet()
        {
            altitude = targetTransform.position.y - terrainHeight;
        }

        public void Reset()
        {
            altitude = 0f;
        }
    }
}