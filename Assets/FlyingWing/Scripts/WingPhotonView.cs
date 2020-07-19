using Photon.Pun;
using UnityEngine;

public class WingPhotonView : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    Rigidbody targetRigidbody = null;

    [SerializeField]
    Elevon leftElevon = null;
    
    [SerializeField]
    Elevon rightElevon = null;

    [SerializeField]
    Motor motor = null;
    
    [SerializeField]
    Transform rotorTransform = null;
    
    [SerializeField]
    MotorSound motorSound = null;
    
    [SerializeField]
    PhotonView targetPhotonView = null;

    [SerializeField]
    bool teleportEnabled = true;
    
    [SerializeField]
    float teleportIfDistanceGreaterThan = 50f;
    
    
    public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
    {
        if( stream.IsWriting )
        {
            stream.SendNext( targetRigidbody.position );
            stream.SendNext( targetRigidbody.rotation );
            stream.SendNext( targetRigidbody.velocity );
            stream.SendNext( targetRigidbody.angularVelocity );
            stream.SendNext( leftElevon.Angle );
            stream.SendNext( rightElevon.Angle );
            stream.SendNext( motor.rpm );
            stream.SendNext( motorSound.SoundTransition );
        }
        else
        {
            netPosition = (Vector3)stream.ReceiveNext();
            netRotation = (Quaternion)stream.ReceiveNext();
            netVelocity = (Vector3)stream.ReceiveNext();
            netAngularVelocity = (Vector3)stream.ReceiveNext();
            netLeftElevonAngle = (float)stream.ReceiveNext();
            netRightElevonAngle = (float)stream.ReceiveNext();
            netRpm = (float)stream.ReceiveNext();
            netSoundTransition = (float)stream.ReceiveNext();
            
            if( teleportEnabled )
            {
                if( Vector3.Distance( targetRigidbody.position, netPosition ) > teleportIfDistanceGreaterThan )
                {
                    targetRigidbody.position = netPosition;
                }
            }

            var lag = Mathf.Abs( (float)( PhotonNetwork.Time - info.SentServerTime ) );

            targetRigidbody.velocity = netVelocity;
            netPosition += targetRigidbody.velocity * lag;
            distance = Vector3.Distance( targetRigidbody.position, netPosition );

            targetRigidbody.angularVelocity = netAngularVelocity;
            netRotation = Quaternion.Euler( targetRigidbody.angularVelocity * lag ) * netRotation;
            angle = Quaternion.Angle( targetRigidbody.rotation, netRotation );
        }
    }


    Vector3 netPosition;
    Quaternion netRotation;
    Vector3 netVelocity;
    Vector3 netAngularVelocity;
    float netLeftElevonAngle;
    float netRightElevonAngle;
    float netRpm;
    float netSoundTransition;
    float distance;
    float angle;
    float rotorSpeed;
    

    void Awake()
    {
        netPosition = targetRigidbody.position;
        netRotation = targetRigidbody.rotation;
    }

    void Update()
    {
        motorSound.SoundTransition = netSoundTransition;

        var degPerSec = netRpm / 60f * 360f;
        rotorTransform.localRotation *= Quaternion.Euler( 0f, 0f, degPerSec * Time.deltaTime );
    }

    void FixedUpdate()
    {
        if( targetPhotonView.IsMine )
        {
            return;
        }

        targetRigidbody.position = Vector3.MoveTowards( targetRigidbody.position, netPosition, distance * ( 1f / PhotonNetwork.SerializationRate ) );
        targetRigidbody.rotation = Quaternion.RotateTowards( targetRigidbody.rotation, netRotation, angle * ( 1f / PhotonNetwork.SerializationRate ) );

        leftElevon.Angle = netLeftElevonAngle;
        rightElevon.Angle = netRightElevonAngle;
    }
}