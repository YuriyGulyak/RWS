using UnityEngine;

interface IGameEvent
{
    void Raise();
        
    void RegisterListener( IGameEventListener listener );
        
    void UnregisterListener( IGameEventListener listener );
}
    
interface IGameEvent<T>
{
    void Raise( T value );
        
    void RegisterListener( IGameEventListener<T> listener );
        
    void UnregisterListener( IGameEventListener<T> listener );
}
