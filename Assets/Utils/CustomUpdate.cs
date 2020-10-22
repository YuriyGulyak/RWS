public class CustomUpdate
{
    public CustomUpdate( float maxFps )
    {
        waitSeconds = 1f / maxFps;
    }


    public delegate void OnUpdateDelegate( float deltaTime );
    public OnUpdateDelegate OnUpdate;

    public void Update( float newTime )
    {
        var deltaTime = newTime - lastUpdateTime;
        if( deltaTime >= waitSeconds )
        {
            lastUpdateTime = newTime;
            OnUpdate?.Invoke( deltaTime );
        }
    }

    
    float waitSeconds;
    float lastUpdateTime;
}