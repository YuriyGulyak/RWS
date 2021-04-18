using UnityEngine;

public class Drag : MonoBehaviour
{
    public Vector3 value = Vector3.zero;

    Rigidbody _rigidbody;
    Transform _transform;
    
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = _rigidbody.transform;
    }

    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;
        
        var velocityLocal = _transform.InverseTransformDirection(  _rigidbody.velocity );

        velocityLocal.x *= Mathf.Clamp01( 1f - value.x * deltaTime );
        velocityLocal.y *= Mathf.Clamp01( 1f - value.y * deltaTime );
        velocityLocal.z *= Mathf.Clamp01( 1f - value.z * deltaTime );
 
        _rigidbody.velocity = _transform.TransformDirection( velocityLocal );
    }
}
