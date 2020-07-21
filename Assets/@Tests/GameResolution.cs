using UnityEngine;

public class GameResolution : MonoBehaviour
{
    // 1366x768
    // 1280x720
    // 960x540
    
    [SerializeField]
    int width = 960;
    
    [SerializeField]
    int height = 540;
    
    void Awake()
    {
        Screen.SetResolution( width, height, true );
        
        //foreach( var resolution in Screen.resolutions )
        //{
        //    print( resolution );
        //}
    }
}
