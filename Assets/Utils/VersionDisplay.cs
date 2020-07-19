using TMPro;
using UnityEngine;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField] 
    TextMeshProUGUI targetText = null;

    [SerializeField] 
    string prefix = "v ";
    
    
    void Awake()
    {
        targetText.text = prefix + Application.version;
    }
}