using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RWS
{
    public class SingleplayerPanel : MonoBehaviour
    {
        [SerializeField] 
        RectTransform panelRect = null;

        [SerializeField] 
        Button closeButton = null;

        [SerializeField] 
        Toggle infiniteBatteryToggle = null;

        [SerializeField] 
        Toggle infiniteRangeToggle = null;

        [SerializeField] 
        Toggle showGhostToggle = null;

        [SerializeField] 
        Button startGameButton = null;

        [SerializeField] 
        ButtonWithCanvasGroup prevTrackButton = null;

        [SerializeField] 
        ButtonWithCanvasGroup nextTrackButton = null;

        [SerializeField] 
        GameObject[] trackPanels = null;

        //----------------------------------------------------------------------------------------------------

        public bool IsOpen => gameObject.activeSelf;

        public void Show()
        {
            if( gameObject.activeSelf )
            {
                return;
            }
            gameObject.SetActive( true );

            infiniteBatteryToggle.isOn = PlayerPrefs.GetInt( infiniteBatteryKey, 0 ) > 0;
            infiniteRangeToggle.isOn = PlayerPrefs.GetInt( infiniteRangeKey, 0 ) > 0;
            showGhostToggle.isOn = PlayerPrefs.GetInt( showGhostKey, 0 ) > 0;
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

        const string infiniteBatteryKey = "InfiniteBattery";
        const string infiniteRangeKey = "InfiniteRange";
        const string showGhostKey = "ShowGhost";

        int currentTrackSelected;
        InputManager inputManager;
        

        void Awake()
        {
            closeButton.onClick.AddListener( Hide );

            currentTrackSelected = 0;
            prevTrackButton.Interactable = false;

            prevTrackButton.onClick.AddListener( () =>
            {
                trackPanels[ currentTrackSelected ].SetActive( false );

                currentTrackSelected--;
                trackPanels[ currentTrackSelected ].SetActive( true );

                if( currentTrackSelected == 0 )
                {
                    prevTrackButton.Interactable = false;
                }

                if( !nextTrackButton.Interactable )
                {
                    nextTrackButton.Interactable = true;
                }

            } );

            nextTrackButton.onClick.AddListener( () =>
            {
                trackPanels[ currentTrackSelected ].SetActive( false );

                currentTrackSelected++;
                trackPanels[ currentTrackSelected ].SetActive( true );

                if( currentTrackSelected == trackPanels.Length - 1 )
                {
                    nextTrackButton.Interactable = false;
                }

                if( !prevTrackButton.Interactable )
                {
                    prevTrackButton.Interactable = true;
                }

            } );

            infiniteBatteryToggle.onValueChanged.AddListener( value => PlayerPrefs.SetInt( infiniteBatteryKey, value ? 1 : 0 ) );
            infiniteRangeToggle.onValueChanged.AddListener( value => PlayerPrefs.SetInt( infiniteRangeKey, value ? 1 : 0 ) );
            showGhostToggle.onValueChanged.AddListener( value => PlayerPrefs.SetInt( showGhostKey, value ? 1 : 0 ) );

            startGameButton.onClick.AddListener( () =>
            {
                BlackScreen.Instance.StartToBlackScreenAnimation( () =>
                {
                    if( currentTrackSelected == 0 )
                    {
                        SceneManager.LoadSceneAsync( 1 );
                    }
                    else if( currentTrackSelected == 1 )
                    {
                        SceneManager.LoadSceneAsync( 2 );
                    }

                } );
            } );
            
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