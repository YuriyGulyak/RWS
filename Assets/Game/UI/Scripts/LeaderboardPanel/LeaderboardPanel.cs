using System;
using System.Collections;
using RWS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField]
    RectTransform panelRect;

    [SerializeField]
    Button closeButton;

    [SerializeField]
    GameObject leaderboardEntryTemplate;

    [SerializeField]
    TextMeshProUGUI messageTextMesh;

    //----------------------------------------------------------------------------------------------------

    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }

        gameObject.SetActive( true );

        UpdateLeaderboard();
    }

    public void Hide()
    {
        if( !gameObject.activeSelf )
        {
            return;
        }

        gameObject.SetActive( false );
        panelRect.anchoredPosition = Vector2.zero;

        ClearLeaderboard();
        ClearLeaderboard();
    }

    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        closeButton.onClick.AddListener( Hide );

        //

        InputManager.Instance.OnEscapeButton += OnEscapeButton;

        leaderboardEntryTemplate.SetActive( false );
        messageTextMesh.gameObject.SetActive( false );

        leaderboard = Leaderboard.Instance;
    }

    //----------------------------------------------------------------------------------------------------

    string timeFormat = @"mm\:ss\.ff";
    readonly string dreamloPublicCode = "5f9550d3eb371809c4a783ef";

    Leaderboard leaderboard;
    IEnumerator instantiateLeaderboardEntries;
    GameObject[] entries;


    void UpdateLeaderboard()
    {
        leaderboard.GetRecords( dreamloPublicCode, 0, 100, OnDownloadSuccess, OnDownloadError );

        messageTextMesh.gameObject.SetActive( true );
        messageTextMesh.text = "Loading . . .";
    }

    void OnDownloadSuccess( Leaderboard.Record[] records )
    {
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
        entries = new GameObject[ records.Length ];
        var parent = leaderboardEntryTemplate.transform.parent;

        for( var i = 0; i < records.Length; i++ )
        {
            var leaderboardEntryGameObject = Instantiate( leaderboardEntryTemplate, parent );
            entries[ i ] = leaderboardEntryGameObject;

            leaderboardEntryGameObject.SetActive( true );
            leaderboardEntryGameObject.name = $"{leaderboardEntryTemplate.name} ({i + 1})";

            var record = records[ i ];

            var number = $"{i + 1}.";
            var pilotName = record.pilot;
            var craftName = record.craft;
            var lapTime = TimeSpan.FromSeconds( record.seconds ).ToString( timeFormat );
            var postedDate = record.date.Split( ' ' )[ 0 ];

            var leaderboardEntry = leaderboardEntryGameObject.GetComponent<LeaderboardEntry>();
            leaderboardEntry.Init( number, pilotName, craftName, lapTime, postedDate );

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

        if( entries != null && entries.Length > 0 )
        {
            foreach( var entry in entries )
            {
                if( entry )
                {
                    Destroy( entry );
                }
            }

            entries = null;
        }
    }


    void OnEscapeButton()
    {
        Hide();
    }
}