using Photon.Pun;
using UnityEngine;

public class WingPhotonView : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    Rigidbody targetRigidbody = null;

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
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            if( teleportEnabled )
            {
                if( Vector3.Distance( targetRigidbody.position, networkPosition ) > teleportIfDistanceGreaterThan )
                {
                    targetRigidbody.position = networkPosition;
                }
            }

            var lag = Mathf.Abs( (float)( PhotonNetwork.Time - info.SentServerTime ) );

            targetRigidbody.velocity = (Vector3)stream.ReceiveNext();
            networkPosition += targetRigidbody.velocity * lag;
            distance = Vector3.Distance( targetRigidbody.position, networkPosition );

            targetRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
            networkRotation = Quaternion.Euler( targetRigidbody.angularVelocity * lag ) * networkRotation;
            angle = Quaternion.Angle( targetRigidbody.rotation, networkRotation );
        }
    }


    Vector3 networkPosition;
    Quaternion networkRotation;
    float distance;
    float angle;
    
    
    void Awake()
    {
        networkPosition = targetRigidbody.position;
        networkRotation = targetRigidbody.rotation;
    }

    void FixedUpdate()
    {
        if( targetPhotonView.IsMine )
        {
            return;
        }

        targetRigidbody.position = Vector3.MoveTowards( targetRigidbody.position, networkPosition, distance * ( 1f / PhotonNetwork.SerializationRate ) );
        targetRigidbody.rotation = Quaternion.RotateTowards( targetRigidbody.rotation, networkRotation, angle * ( 1f / PhotonNetwork.SerializationRate ) );
    }
}