using UnityEngine;

public class WingLauncher : MonoBehaviour
{
    [SerializeField]
    Rigidbody wingRigidbody = null;

    [SerializeField]
    float launchDistance = 1.5f;
    
    [SerializeField]
    float launchForce = 1f; // Kg

    //----------------------------------------------------------------------------------------------------
    
    public void Launch()
    {
        if( state != State.Ready )
        {
            return;
        }
        
        state = State.Launching;
        wingRigidbody.isKinematic = false;
    }

    public void Reset()
    {
        state = State.Ready;
        this.enabled = true;
    }

    //----------------------------------------------------------------------------------------------------

    enum State
    {
        Ready,
        Launching,
        Launched
    }
    State state;
    
    Transform wingTransform;
    Vector3 startPosition;


    void Awake()
    {
        wingRigidbody.isKinematic = true;
        wingTransform = wingRigidbody.transform;
        startPosition = wingTransform.position;
        state = State.Ready;
    }

    void FixedUpdate()
    {
        if( state == State.Launching )
        {
            wingRigidbody.AddForce( wingRigidbody.transform.forward * ( launchForce * 9.8f ), ForceMode.Force );

            if( Vector3.Distance( startPosition, wingTransform.position ) > launchDistance )
            {
                state = State.Launched;
                this.enabled = false;
            }
        }
    }
}
