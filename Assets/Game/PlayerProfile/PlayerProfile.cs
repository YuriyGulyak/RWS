using System;

[Serializable]
public class PlayerProfile
{
    public string playerName;
    
    /// <summary>Seconds</summary>
    public float totalFlightTime;
    
    /// <summary>Meters</summary>
    public float totalFlightDistance;
    
    /// <summary>Seconds</summary>
    public float longestFlightTime;
    
    /// <summary>m/s</summary>
    public float topSpeed;
    
    public int completedLaps;
    public int numberOfLaunches;
    public int numberOfCrashes;

    public PlayerProfile()
    {
        playerName = $"Pilot{new Random().Next( 0, 10000 ):0000}";
    }
}