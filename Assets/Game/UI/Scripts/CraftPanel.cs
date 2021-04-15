using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class CraftPanel : MonoBehaviour
    {
        [SerializeField]
        RectTransform panelRect = null;

        [SerializeField]
        Button closeButton = null;

        //----------------------------------------------------------------------------------------------------

        public bool IsOpen => gameObject.activeSelf;

        public void Show()
        {
            if( gameObject.activeSelf )
            {
                return;
            }

            gameObject.SetActive( true );
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

        InputManager inputManager;
        
        
        void Awake()
        {
            closeButton.onClick.AddListener( Hide );

            inputManager = InputManager.Instance;
        }

        void OnEnable()
        {
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;
        }
        

        void OnEscapeButton()
        {
            Hide();
        }
    }
}