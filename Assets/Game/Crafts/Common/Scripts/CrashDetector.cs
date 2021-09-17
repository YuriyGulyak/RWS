using UnityEngine;
using UnityEngine.Events;

public class CrashDetector : MonoBehaviour
{
    [SerializeField]
    Rigidbody _rigidbody = null;

    
    public float energyThreshold = 2f;
    
    public UnityEvent OnCrashed;

    public bool IsCrashed { get; private set; }

    public void Reset()
    {
        IsCrashed = false;
    }


    void OnValidate()
    {
        if( !_rigidbody )
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }

    void OnCollisionEnter( Collision collision )
    {
        if( IsCrashed )
        {
            return;
        }

        var project = Vector3.Project( collision.relativeVelocity, collision.contacts[ 0 ].normal );
        
        var kineticEnergy = 0.5f * _rigidbody.mass * project.sqrMagnitude;
        if( kineticEnergy > energyThreshold )
        {
            IsCrashed = true;
            OnCrashed.Invoke();
        }
        
        //print( kineticEnergy );
    }
}
