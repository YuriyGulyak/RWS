using RWS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class SingleplayerPanel : MonoBehaviour
    {
        [SerializeField] 
        RectTransform panelRect;

        [SerializeField] 
        Button closeButton;

        [SerializeField] 
        Toggle infiniteBatteryToggle;

        [SerializeField] 
        Toggle infiniteRangeToggle;

        [SerializeField] 
        Toggle showGhostToggle;

        [SerializeField] 
        Button startGameButton;

        [SerializeField] 
        ButtonWithCanvasGroup prevTrackButton;

        [SerializeField] 
        ButtonWithCanvasGroup nextTrackButton;

        [SerializeField] 
        GameObject[] trackPanels;

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

        readonly string infiniteBatteryKey = "InfiniteBattery";
        readonly string infiniteRangeKey = "InfiniteRange";
        readonly string showGhostKey = "ShowGhost";

        int currentTrackSelected;


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

            infiniteBatteryToggle.onValueChanged.AddListener( value =>
                PlayerPrefs.SetInt( infiniteBatteryKey, value ? 1 : 0 ) );
            infiniteRangeToggle.onValueChanged.AddListener( value =>
                PlayerPrefs.SetInt( infiniteRangeKey, value ? 1 : 0 ) );
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

            InputManager.Instance.OnEscapeButton += OnEscapeButton;
        }


        void OnEscapeButton()
        {
            Hide();
        }
    }
}