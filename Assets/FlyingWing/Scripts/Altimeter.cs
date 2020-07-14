using UnityEngine;

public class Altimeter : MonoBehaviour
{
    [SerializeField] 
    Transform craftTransform = null;

    [SerializeField] 
    LayerMask terrainLayer = default;


    public float Altitude => craftTransform.position.y - terrainHeight;


    float terrainHeight;

    void OnValidate()
    {
        if( !craftTransform )
        {
            craftTransform = GetComponent<Transform>();
        }
    }

    void Start()
    {
        if( Physics.Raycast( craftTransform.position, Vector3.down, out var hit, 100f, terrainLayer, QueryTriggerInteraction.Ignore ) )
        {
            terrainHeight = hit.point.y;
        }
    }
}