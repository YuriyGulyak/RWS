using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text = null;

    [SerializeField]
    float updateRate = 60f;
    
    float deltaTime;
    int fps;
    float lastUpdateTime;
    
    Dictionary<int, string> pool = new Dictionary<int, string>();
    

    void Update()
    {
        if( Time.timeScale.Equals( 0f ) )
        {
            return;
        }

        deltaTime += ( Time.deltaTime - deltaTime ) * 0.1f;
        fps = Mathf.CeilToInt( 1f / deltaTime );

        var time = Time.time;
        if( time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = time;
            
            if( !pool.ContainsKey( fps ) )
            {
                pool.Add( fps, fps.ToString() );
            }
        
            text.text = pool[ fps ];
        }
    }
}