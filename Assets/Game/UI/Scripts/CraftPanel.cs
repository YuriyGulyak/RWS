using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class CraftPanel : MonoBehaviour
    {
        [SerializeField]
        RectTransform panelRect;

        [SerializeField]
        Button closeButton;

        //----------------------------------------------------------------------------------------------------

        public bool IsOpen => gameObject.activeSelf;

        public void Show()
        {
            if( gameObject.activeSelf )
            {
                return;
            }

            gameObject.SetActive( true );


            //
        }

        public void Hide()
        {
            if( !gameObject.activeSelf )
            {
                return;
            }

            gameObject.SetActive( false );
            panelRect.anchoredPosition = Vector2.zero;
        }

        //----------------------------------------------------------------------------------------------------


        void Awake()
        {
            closeButton.onClick.AddListener( Hide );

            InputManager.Instance.OnEscapeButton += OnEscapeButton;
        }


        void OnEscapeButton()
        {
            Hide();
        }
    }
}