using System;
using UnityEngine;

namespace RWS
{
    [RequireComponent( typeof( DreamloLeaderboard ) )]
    public class Leaderboard : Singleton<Leaderboard>
    {
        [SerializeField]
        DreamloLeaderboard dreamloLeaderboard = null;

        void OnValidate()
        {
            if( !dreamloLeaderboard )
            {
                dreamloLeaderboard = GetComponent<DreamloLeaderboard>() ?? gameObject.AddComponent<DreamloLeaderboard>();
            }
        }


        const int maxTime = 10 * 60 * 1000;

        public struct Record
        {
            public string pilot;
            public string craft;
            public float seconds;
            public string date;
        }


        public void AddRecord( string privateCode, string pilot, string craft, float seconds, Action<string> onError )
        {
            dreamloLeaderboard.AddScore( privateCode, pilot, maxTime - Mathf.RoundToInt( seconds * 1000 ), 0, craft, onError );
        }

        public void DeleteRecord( string privateCode, string pilot, Action onSuccess, Action<string> onError )
        {
            dreamloLeaderboard.DeleteScore( privateCode, pilot, onSuccess, onError );
        }

        public void GetRecord( string publicCode, string pilot, Action<Record> onSuccess, Action<string> onError )
        {
            dreamloLeaderboard.GetScore( publicCode, pilot, score =>
                {
                    var record = new Record
                    {
                        pilot = score.player,
                        craft = score.text,
                        seconds = ( maxTime - score.score ) / 1000f,
                        date = score.date
                    };
                    onSuccess?.Invoke( record );
                },
                onError );
        }

        public void GetRecords( string publicCode, int offset, int count, Action<Record[]> onSuccess, Action<string> onError )
        {
            dreamloLeaderboard.GetScores( publicCode, offset, count, scores =>
                {
                    var records = new Record[scores.Length];

                    for( var i = 0; i < records.Length; i++ )
                    {
                        var score = scores[ i ];
                        var record = new Record
                        {
                            pilot = score.player,
                            craft = score.text,
                            seconds = ( maxTime - score.score ) / 1000f,
                            date = score.date
                        };
                        records[ i ] = record;
                    }

                    onSuccess?.Invoke( records );
                },
                onError );
        }
    }
}
