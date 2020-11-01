using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI posNumberTextMesh = null;
    
    [SerializeField]
    TextMeshProUGUI pilotTextMesh = null;
    
    [SerializeField]
    TextMeshProUGUI craftTextMesh = null;
    
    [SerializeField]
    TextMeshProUGUI timeTextMesh = null;
    
    [SerializeField]
    TextMeshProUGUI dateTextMesh = null;
    

    public void Init( string posNumber, string pilotName, string craftName, string lapTime, string postedDate )
    {
        posNumberTextMesh.text = posNumber;
        pilotTextMesh.text = pilotName;
        craftTextMesh.text = craftName;
        timeTextMesh.text = lapTime;
        dateTextMesh.text = postedDate;
    }
}
