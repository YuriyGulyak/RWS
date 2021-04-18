using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RWS
{
    public class NavigationPanel : MonoBehaviour
    {
        [SerializeField]
        InputManager inputManager;
        
        [SerializeField]
        Button graphicsButton = null;

        [SerializeField]
        Button soundButton = null;

        [SerializeField]
        Button controlsButton = null;

        [SerializeField]
        Button sensitivityButton = null;


        public UnityAction OnGraphicsButtonClicked;
        public UnityAction OnSoundButtonClicked;
        public UnityAction OnControlsButtonClicked;
        public UnityAction OnSensitivityButtonClicked;
        public UnityAction OnEscapeButtonClicked;
    
        public void Show()
        {
            gameObject.SetActive( true );

            inputManager.OnEscapeButton += OnEscapeButtonClicked;
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            
            inputManager.OnEscapeButton -= OnEscapeButtonClicked;
        }


        void OnValidate()
        {
            if( !inputManager )
            {
                inputManager = InputManager.Instance;
            }
        }

        void Awake()
        {
            graphicsButton.onClick.AddListener( () => OnGraphicsButtonClicked?.Invoke() );
            soundButton.onClick.AddListener( () => OnSoundButtonClicked?.Invoke() );
            controlsButton.onClick.AddListener( () => OnControlsButtonClicked?.Invoke() );
            sensitivityButton.onClick.AddListener( () => OnSensitivityButtonClicked?.Invoke() );
        }
    }
}