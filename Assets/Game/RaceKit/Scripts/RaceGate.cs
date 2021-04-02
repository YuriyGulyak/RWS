using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RWS
{
    public class RaceGate : MonoBehaviour
    {
        [SerializeField]
        Transform gateTransform = null;

        [SerializeField]
        bool reverse = false;

        //----------------------------------------------------------------------------------------------------

        [System.Serializable]
        public class GameObjectEvent : UnityEvent<GameObject>
        {
        }

        public GameObjectEvent OnSuccess = new GameObjectEvent();

        //----------------------------------------------------------------------------------------------------

        int craftLayerIndex;

        List<Rigidbody> entered = new List<Rigidbody>();
        //Vector3 enterPoint;
        //Vector3 exitPoint;

        void OnValidate()
        {
            if( !gateTransform )
            {
                gateTransform = transform;
            }
        }

        void OnTriggerEnter( Collider other )
        {
            var attachedRigidbody = other.attachedRigidbody;
            if( attachedRigidbody && !entered.Contains( attachedRigidbody ) )
            {
                //enterPoint = attachedRigidbody.position;

                var targetLocalPosZ = gateTransform.InverseTransformPoint( attachedRigidbody.position ).z;
                var isValid = reverse ? targetLocalPosZ > 0f : targetLocalPosZ < 0f;
                if( isValid )
                {
                    entered.Add( attachedRigidbody );
                }
            }
        }

        void OnTriggerExit( Collider other )
        {
            var attachedRigidbody = other.attachedRigidbody;
            if( attachedRigidbody && entered.Contains( attachedRigidbody ) )
            {
                //exitPoint = attachedRigidbody.position;

                var targetLocalPosZ = gateTransform.InverseTransformPoint( attachedRigidbody.position ).z;
                var isValid = reverse ? targetLocalPosZ < 0f : targetLocalPosZ > 0f;
                if( isValid )
                {
                    OnSuccess.Invoke( attachedRigidbody.gameObject );
                }

                entered.Remove( attachedRigidbody );
            }
        }

        //#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            var colorBackup = Gizmos.color;

            Gizmos.color = Color.white;
            Gizmos.DrawRay( transform.position, transform.forward * ( reverse ? -5f : 5f ) );
            Gizmos.DrawWireSphere( transform.position, 0.05f );

            //Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere( enterPoint, 0.5f );

            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere( exitPoint, 0.5f );

            Gizmos.color = colorBackup;
        }
        //#endif
    }
}