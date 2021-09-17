using UnityEngine;

namespace RWS
{
    public class BuzzerSound : MonoBehaviour
    {
        [SerializeField]
        FlyingWing flyingWing = null;

        [SerializeField]
        AudioSource audioSource = null;

        [SerializeField]
        float updateRate = 10f;

        //--------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            customUpdate = new CustomUpdate( updateRate );
            customUpdate.OnUpdate += OnUpdate;
        }

        void Update()
        {
            customUpdate.Update( Time.time );
        }

        //--------------------------------------------------------------------------------------------------------------

        CustomUpdate customUpdate;

        void OnUpdate( float deltaTime )
        {
            if( flyingWing.Battery.VoltageStatus == Status.Critical )
            {
                if( !audioSource.isPlaying )
                {
                    audioSource.Play();
                }
            }
            else
            {
                if( audioSource.isPlaying )
                {
                    audioSource.Stop();
                }
            }
        }
    }
}