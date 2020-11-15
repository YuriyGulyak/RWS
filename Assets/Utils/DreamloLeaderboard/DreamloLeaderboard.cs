// How to use dreamlo - http://dreamlo.com/developer
// Use for watching leaderboard in browser -     

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public struct DreamloScore
{
    public string player;
    public int score;
    public int seconds;
    public string text;
    public string date;
}

public class DreamloLeaderboard : MonoBehaviour
{
    public void AddScore( string privateCode, string player, int score, int seconds, string text, Action<string> onError )
    {
        StartCoroutine( AddScoreCoroutine( privateCode, player, score, seconds, text, onError ) );
    }

    public void DeleteScore( string privateCode, string player, Action onSuccess, Action<string> onError )
    {
        StartCoroutine( DeleteScoreCoroutine( privateCode, player, onSuccess, onError ) );
    }

    public void GetScore( string publicCode, string player, Action<DreamloScore> onSuccess, Action<string> onError )
    {
        StartCoroutine( GetScoreCoroutine( publicCode, player, onSuccess, onError ) );
    }

    public void GetScores( string publicCode, Action<DreamloScore[]> onSuccess, Action<string> onError )
    {
        StartCoroutine( GetScoresCoroutine( publicCode, onSuccess, onError ) );
    }

    public void GetScores( string publicCode, int offset, int count, Action<DreamloScore[]> onSuccess, Action<string> onError )
    {
        StartCoroutine( GetScoresXmlCoroutine( publicCode, offset, count, onSuccess, onError ) );
    }

    //----------------------------------------------------------------------------------------------------

    const string dreamloURL = "http://dreamlo.com/lb/";


    static IEnumerator AddScoreCoroutine( string privateCode, string player, int score, int seconds, string text, Action<string> onError )
    {
        using( var webRequest = UnityWebRequest.Get( dreamloURL + privateCode + "/add/" + UnityWebRequest.EscapeURL( player ) + "/" + score + "/" + seconds + "/" + text ) )
        {
            yield return webRequest.SendWebRequest();

            if( !string.IsNullOrEmpty( webRequest.error ) )
            {
                onError?.Invoke( webRequest.error );
            }
        }
    }

    static IEnumerator DeleteScoreCoroutine( string privateCode, string player, Action onSuccess, Action<string> onError )
    {
        using( var webRequest = UnityWebRequest.Get( dreamloURL + privateCode + "/delete/" + UnityWebRequest.EscapeURL( player ) ) )
        {
            yield return webRequest.SendWebRequest();

            if( !string.IsNullOrEmpty( webRequest.error ) )
            {
                onError?.Invoke( webRequest.error );
            }
            else
            {
                onSuccess?.Invoke();
            }
        }
    }

    static IEnumerator GetScoreCoroutine( string publicCode, string player, Action<DreamloScore> onSuccess, Action<string> onError )
    {
        using( var webRequest = UnityWebRequest.Get( dreamloURL + publicCode + "/pipe-get/" + player ) )
        {
            yield return webRequest.SendWebRequest();


            if( !string.IsNullOrEmpty( webRequest.error ) )
            {
                onError?.Invoke( webRequest.error );
            }
            else
            {
                var entryInfo = webRequest.downloadHandler.text.Split( '|' );
                if( entryInfo.Length < 5 )
                {
                    yield break;
                }

                var score = new DreamloScore
                {
                    player = entryInfo[ 0 ],
                    score = int.Parse( entryInfo[ 1 ] ),
                    seconds = int.Parse( entryInfo[ 2 ] ),
                    text = entryInfo[ 3 ],
                    date = entryInfo[ 4 ]
                };

                onSuccess?.Invoke( score );
            }
        }
    }

    static IEnumerator GetScoresCoroutine( string publicCode, Action<DreamloScore[]> onSuccess, Action<string> onError )
    {
        using( var webRequest = UnityWebRequest.Get( dreamloURL + publicCode + "/pipe" ) )
        {
            yield return webRequest.SendWebRequest();


            if( !string.IsNullOrEmpty( webRequest.error ) )
            {
                onError?.Invoke( webRequest.error );
            }
            else
            {
                var entries = webRequest.downloadHandler.text.Split( new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries );
                var scores = new List<DreamloScore>();

                for( var i = 0; i < entries.Length; i++ )
                {
                    var entryInfo = entries[ i ].Split( '|' );
                    if( entryInfo.Length < 5 )
                    {
                        continue;
                    }

                    var score = new DreamloScore
                    {
                        player = entryInfo[ 0 ],
                        score = int.Parse( entryInfo[ 1 ] ),
                        seconds = int.Parse( entryInfo[ 2 ] ),
                        text = entryInfo[ 3 ],
                        date = entryInfo[ 4 ]
                    };

                    scores.Add( score );
                }

                onSuccess?.Invoke( scores.ToArray() );
            }
        }
    }

    static IEnumerator GetScoresXmlCoroutine( string publicCode, int offset, int count, Action<DreamloScore[]> onSuccess, Action<string> onError )
    {
        using( var webRequest = UnityWebRequest.Get( $"{dreamloURL}{publicCode}/xml/{offset}/{count}" ) )
        {
            yield return webRequest.SendWebRequest();


            if( !string.IsNullOrEmpty( webRequest.error ) )
            {
                onError?.Invoke( webRequest.error );
            }
            else
            {
                /*
                <dreamlo>
                    <leaderboard>
                        <entry>
                        <name>Player5</name>
                        <score>500000</score>
                        <seconds>0</seconds>
                        <text>Test text 1; Test text 2; Test text 3</text>
                        <date>10/28/2020 3:34:38 PM</date>
                        </entry>
                    </leaderboard>
                </dreamlo>
                */

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml( webRequest.downloadHandler.text );
                var leaderboardNode = xmlDocument.FirstChild.FirstChild;

                var scores = new List<DreamloScore>();

                for( var i = 0; i < leaderboardNode.ChildNodes.Count; i++ )
                {
                    var scoreNode = leaderboardNode.ChildNodes[ i ];
                    if( scoreNode.ChildNodes.Count < 5 )
                    {
                        continue;
                    }

                    var score = new DreamloScore
                    {
                        player = scoreNode.ChildNodes[ 0 ].InnerText,
                        score = int.Parse( scoreNode.ChildNodes[ 1 ].InnerText ),
                        seconds = int.Parse( scoreNode.ChildNodes[ 2 ].InnerText ),
                        text = scoreNode.ChildNodes[ 3 ].InnerText,
                        date = scoreNode.ChildNodes[ 4 ].InnerText
                    };
                    scores.Add( score );
                }

                onSuccess?.Invoke( scores.ToArray() );
            }
        }
    }
}