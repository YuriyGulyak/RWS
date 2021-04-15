using UnityEngine;

namespace RWS
{
    public class StickDisplay : MonoBehaviour
    {
        [SerializeField]
        RectTransform handleTransform = null;

        [SerializeField]
        Vector2 areaHalfSize = new Vector2( 100f, 100f );

        [SerializeField, Range( -1f, 1f )]
        float x = 0f;

        [SerializeField, Range( -1f, 1f )]
        float y = 0f;

        //----------------------------------------------------------------------------------------------------

        public float X
        {
            get => x;
            set
            {
                x = Mathf.Clamp( value, -1f, 1f );
                UpdateHandlePosition();
            }
        }

        public float Y
        {
            get => y;
            set
            {
                y = Mathf.Clamp( value, -1f, 1f );
                UpdateHandlePosition();
            }
        }

        //----------------------------------------------------------------------------------------------------

        void OnValidate()
        {
            UpdateHandlePosition();
        }


        void UpdateHandlePosition()
        {
            handleTransform.anchoredPosition = new Vector2( areaHalfSize.x * x, areaHalfSize.y * y );
        }
    }
}
