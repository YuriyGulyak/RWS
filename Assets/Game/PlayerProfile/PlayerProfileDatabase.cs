using UnityEngine;

public static class PlayerProfileDatabase
{
    const string PLAYER_PROFILE_KEY = "PlayerProfile";
    
    public static void SavePlayerProfile( PlayerProfile playerProfile )
    {
        var playerProfileJson = JsonUtility.ToJson( playerProfile );
        PlayerPrefs.SetString( PLAYER_PROFILE_KEY, playerProfileJson );
    }

    public static PlayerProfile LoadPlayerProfile()
    {
        if( !PlayerPrefs.HasKey( PLAYER_PROFILE_KEY ) )
        {
            return null;
        }
        var playerProfileJson = PlayerPrefs.GetString( PLAYER_PROFILE_KEY );
        return JsonUtility.FromJson<PlayerProfile>( playerProfileJson );
    }
}