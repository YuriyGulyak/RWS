using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if( instance == null )
            {
                instance = FindObjectOfType<T>();
                
                if( instance == null )
                {
                    Debug.LogWarning( typeof( T ).Name + " not found" );
                }
                if( FindObjectsOfType<T>().Length > 1 )
                {
                    Debug.LogWarning( typeof( T ).Name + " instances > 1" );
                }
            }
            
            return instance;
        }
    }
}