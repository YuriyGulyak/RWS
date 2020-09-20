using UnityEngine;

public class SwayEffect : MonoBehaviour
{
    [SerializeField]
    new Transform transform = null;

    [SerializeField]
    float swaySpeed = 0.5f;

    [SerializeField]
    float swayAmount = 1f;

    //--------------------------------------------------------------------------------------------------------------

    void OnValidate()
    {
        if( !transform )
        {
            transform = GetComponent<Transform>();
        }
    }

    void LateUpdate()
    {
        //var xSway = ( Mathf.PerlinNoise( 0f, Time.time * swaySpeed ) - 0.5f );
        //var ySway = ( Mathf.PerlinNoise( 0f, Time.time * swaySpeed ) + 100f ) - 0.5f;

        var xSway = Mathf.PerlinNoise( 0f, Time.time * swaySpeed ) - 0.5f;
        var ySway = Mathf.PerlinNoise( 0f, Time.time * swaySpeed + 100f ) - 0.5f;

        transform.localEulerAngles = new Vector3( xSway * swayAmount, ySway * swayAmount, 0f );
    }
}