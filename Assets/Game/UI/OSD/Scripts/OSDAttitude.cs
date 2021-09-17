using Unity.Mathematics;
using UnityEngine;

namespace RWS
{
    public class OSDAttitude : MonoBehaviour
    {
        [SerializeField]
        RectTransform horizonTransform = default;

        [SerializeField]
        RectTransform aircraftSymbolTransform = default;

        [SerializeField]
        float cameraVerticalFOV = 90f;

        [SerializeField]
        FloatVariable rollVariable = default;
        
        [SerializeField]
        FloatVariable pitchVariable = default;
        

        float pixelsInDegree;


        void Awake()
        {
            // TODO There will be problems if change the resolution during the game
            pixelsInDegree = Screen.height / cameraVerticalFOV;
        }

        void OnEnable()
        {
            var horizonPosition = horizonTransform.anchoredPosition;
            horizonPosition.y = 0f;
            horizonTransform.anchoredPosition = horizonPosition;

            var aircraftSymbolAngles = aircraftSymbolTransform.eulerAngles;
            aircraftSymbolAngles.z = 0f;
            aircraftSymbolTransform.eulerAngles = aircraftSymbolAngles;
        }

        void Update()
        {
            var horizonPosition = horizonTransform.anchoredPosition;
            horizonPosition.y = pixelsInDegree * math.clamp( -pitchVariable.Value, -60f, 60f );
            horizonTransform.anchoredPosition = horizonPosition;

            var aircraftSymbolAngles = aircraftSymbolTransform.eulerAngles;
            aircraftSymbolAngles.z = rollVariable.Value;
            aircraftSymbolTransform.eulerAngles = aircraftSymbolAngles;
        }
    }
}
