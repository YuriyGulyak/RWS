using System.Collections;
using UnityEngine;

public class TestDreamloLeaderboard : MonoBehaviour
{
    [SerializeField]
    DreamloLeaderboard dreamloLeaderboard = null;


    const string publicCode = "5f9550d3eb371809c4a783ef";
    const string privateCode = "ZbIxEmdjiU6we6WbZHRGZQp8S0j4TytEuwOlNrARz_Aw";

    
    IEnumerator Start()
    {
        if( false )
        {
            for( var i = 0; i < 100; i++ )
            {
                var playerName = "Player" + ( i + 1 );
                var score = ( i + 1 ) * 1000;
                var text = "Test text";
            
                dreamloLeaderboard.AddScore( privateCode, playerName, score, 0, text, null );
            }
        }


        print( "Added scores" );
        
        
        yield return new WaitForSeconds( 1f );
        
        //dreamloLeaderboard.DeleteScore( privateCode, "Player1", null, null );
        
        yield return new WaitForSeconds( 1f );
        
        dreamloLeaderboard.GetScore( publicCode, "Player1", 
            score =>
            {
                print( $"*{score.player} | {score.score} | {score.seconds} | {score.text} | {score.date}" );
                
            },
            error =>
            {
                print( "Error: " + error );
                
            } );


        dreamloLeaderboard.GetScores( publicCode, 0, 100,
            scores =>
            {
                foreach( var score in scores )
                {
                    print( $"{score.player} | {score.score} | {score.seconds} | {score.text} | {score.date}" );
                    
                }
                print( "Success" );
            },
            error =>
            {
                print( "Error: " + error );
                
            } );
    }
}
