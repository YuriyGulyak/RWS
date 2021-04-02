using UnityEngine;

namespace RWS
{
    public class PilotZoom : MonoBehaviour
    {
        [SerializeField]
        Transform target = null;

        [SerializeField]
        new Camera camera = null;

        [SerializeField]
        float maxDistance = 300f;

        [SerializeField]
        float maxZoom = 3f;


        public void SetTarget( Transform target )
        {
            this.target = target;
            UpdateZoom();
        }


        Transform cameraTransform;
        float initFov;


        void OnValidate()
        {
            if( !camera )
            {
                camera = GetComponentInChildren<Camera>();
            }
        }

        void Awake()
        {
            cameraTransform = camera.transform;
            initFov = camera.fieldOfView;
        }

        void FixedUpdate()
        {
            if( !target )
            {
                return;
            }

            UpdateZoom();
        }


        void UpdateZoom()
        {
            var distance = Vector3.Distance( target.position, cameraTransform.position );
            camera.fieldOfView = initFov / Mathf.Lerp( 1f, maxZoom, distance / maxDistance );
        }
    }
}
