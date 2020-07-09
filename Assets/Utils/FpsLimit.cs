using UnityEngine;

public class FpsLimit : MonoBehaviour
{
    [SerializeField]
    int maxFps = 180;
    

    void Awake()
    {
        if( QualitySettings.vSyncCount == 0 )
        {
            Application.targetFrameRate = maxFps;
        }
    }
}
