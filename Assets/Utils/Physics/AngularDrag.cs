﻿using UnityEngine;

public class AngularDrag : MonoBehaviour
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
        
        var localAngularVelocity = _transform.InverseTransformDirection( _rigidbody.angularVelocity );

        localAngularVelocity.x *= Mathf.Clamp01( 1f - value.x * deltaTime );
        localAngularVelocity.y *= Mathf.Clamp01( 1f - value.y * deltaTime );
        localAngularVelocity.z *= Mathf.Clamp01( 1f - value.z * deltaTime );
 
        _rigidbody.angularVelocity = _transform.TransformDirection( localAngularVelocity );
    }
}
