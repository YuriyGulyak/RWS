using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField]
        RectTransform panelRect = null;

        [SerializeField]
        Button closeButton = null;

        //--------------------------------------------------------------------------------------------------------------
        
        public NavigationPanel navigationPanel;
        public GraphicsPanel graphicsPanel;
        public SoundPanel soundPanel;
        public ControlsPanel controlsPanel;
        public SensitivityPanel sensitivityPanel;
        
        public bool IsOpen => gameObject.activeSelf;
        
        public void Show()
        {
            if( IsOpen )
            {
                return;
            }
            gameObject.SetActive( true );
            
            ChangeState( new NavigationTabState() );
        }

        public void Hide()
        {
            if( !IsOpen )
            {
                return;
            }
            
            ChangeState( null );

            gameObject.SetActive( false );
            ResetPanelPosition();
        }

        public void ChangeState( BaseState<SettingsPanel> newState )
        {
            if( newState == currentState )
            {
                return;
            }

            if( currentState != null )
            {
                currentState.OnDisableState();
            }

            currentState = newState;

            if( currentState != null )
            {
                currentState.owner = this;
                currentState.OnEnableState();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        
        BaseState<SettingsPanel> currentState;


        void Awake()
        {
            closeButton.onClick.AddListener( OnCloseButton );
        }

        
        void OnCloseButton()
        {
            Hide();
        }
        
        void ResetPanelPosition()
        {
            panelRect.anchoredPosition = Vector2.zero;
        }
    }
}
