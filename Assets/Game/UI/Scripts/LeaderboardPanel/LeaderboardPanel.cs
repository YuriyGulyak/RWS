using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class LeaderboardPanel : MonoBehaviour
    {
        [SerializeField]
        RectTransform panelRect = null;

        [SerializeField]
        Button closeButton = null;

        [SerializeField]
        TMP_Dropdown trackDropdown = null;
        
        [SerializeField]
        GameObject leaderboardEntryTemplate = null;

        [SerializeField]
        TextMeshProUGUI messageTextMesh = null;

        [SerializeField]
        BestLapKeyStorage bestLapKeyStorage = null;
        
        //----------------------------------------------------------------------------------------------------

        public bool IsOpen => gameObject.activeSelf;
        
        public void Show()
        {
            gameObject.SetActive( true );
        }

        public void Hide()
        {
            gameObject.SetActive( false );
        }

        //----------------------------------------------------------------------------------------------------

        void Awake()
        {
            closeButton.onClick.AddListener( Hide );
            trackDropdown.onValueChanged.AddListener( OnTrackDropdownValueChanged );
            
            leaderboardEntryTemplate.SetActive( false );
            messageTextMesh.gameObject.SetActive( false );
        }

        void OnEnable()
        {
            inputManager = InputManager.Instance;
            inputManager.OnEscapeButton += OnEscapeButton;
            
            leaderboard = Leaderboard.Instance;
            UpdateLeaderboard();
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;

            panelRect.anchoredPosition = Vector2.zero;
            ClearLeaderboard();
        }
        
        //----------------------------------------------------------------------------------------------------

        string timeFormat = @"mm\:ss\.ff";

        Leaderboard leaderboard;
        IEnumerator instantiateLeaderboardEntries;
        GameObject[] leaderboardEntries;
        InputManager inputManager;
        

        void UpdateLeaderboard()
        {
            var bestLapKeyItem = bestLapKeyStorage.items[ trackDropdown.value ];
            var dreamloPublicCode = bestLapKeyItem.dreamloPublicCode;
            
            leaderboard.GetRecords( dreamloPublicCode, 0, 100, OnDownloadSuccess, OnDownloadError );

            messageTextMesh.gameObject.SetActive( true );
            messageTextMesh.text = "Loading . . .";
        }

        void OnDownloadSuccess( Leaderboard.Record[] records )
        {
            if( !gameObject.activeSelf )
            {
                return;
            }

            messageTextMesh.gameObject.SetActive( false );

            if( records.Length > 0 )
            {
                if( instantiateLeaderboardEntries != null )
                {
                    StopCoroutine( instantiateLeaderboardEntries );
                }

                instantiateLeaderboardEntries = InstantiateLeaderboardEntries( records );
                StartCoroutine( instantiateLeaderboardEntries );
            }
            else
            {
                messageTextMesh.gameObject.SetActive( true );
                messageTextMesh.text = "N/A";
            }
        }

        IEnumerator InstantiateLeaderboardEntries( Leaderboard.Record[] records )
        {
            leaderboardEntries = new GameObject[ records.Length ];
            var parent = leaderboardEntryTemplate.transform.parent;

            for( var i = 0; i < records.Length; i++ )
            {
                var leaderboardEntryGameObject = Instantiate( leaderboardEntryTemplate, parent );
                leaderboardEntries[ i ] = leaderboardEntryGameObject;

                leaderboardEntryGameObject.SetActive( true );
                leaderboardEntryGameObject.name = $"{leaderboardEntryTemplate.name} ({i + 1})";

                var record = records[ i ];

                var posNumber = $"{i + 1}.";
                var pilotName = record.pilot;
                var craftName = record.craft;
                var lapTime = TimeSpan.FromSeconds( record.seconds ).ToString( timeFormat );
                var postedDate = record.date.Split( ' ' )[ 0 ];

                var leaderboardEntry = leaderboardEntryGameObject.GetComponent<LeaderboardEntry>();
                leaderboardEntry.Init( posNumber, pilotName, craftName, lapTime, postedDate );

                if( i % 10 == 0 )
                {
                    yield return null;
                }
            }

            instantiateLeaderboardEntries = null;
        }

        void OnDownloadError( string error )
        {
            messageTextMesh.gameObject.SetActive( true );
            messageTextMesh.text = "Error: " + error;
        }

        void ClearLeaderboard()
        {
            if( instantiateLeaderboardEntries != null )
            {
                StopCoroutine( instantiateLeaderboardEntries );
                instantiateLeaderboardEntries = null;
            }

            if( leaderboardEntries != null )
            {
                foreach( var entry in leaderboardEntries )
                {
                    if( entry )
                    {
                        Destroy( entry );
                    }
                }

                leaderboardEntries = null;
            }
        }

        void OnTrackDropdownValueChanged( int trackIndex )
        {
            ClearLeaderboard();
            UpdateLeaderboard();
        }

        void OnEscapeButton()
        {
            Hide();
        }
    }
}