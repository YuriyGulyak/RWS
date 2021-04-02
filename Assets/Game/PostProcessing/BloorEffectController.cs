using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace RWS
{
    public class BloorEffectController : MonoBehaviour
    {
        [SerializeField]
        PostProcessVolume postProcessVolume = null;

        public bool BloorEffectEnabled
        {
            get
            {
                postProcessVolume.profile.TryGetSettings( out DepthOfField depthOfField );
                return depthOfField && depthOfField.active;
            }
            set
            {
                postProcessVolume.profile.TryGetSettings( out DepthOfField depthOfField );
                if( depthOfField )
                {
                    depthOfField.active = value;
                }
            }
        }
    }
}
