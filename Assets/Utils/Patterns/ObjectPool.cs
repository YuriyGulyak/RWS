using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    public ObjectPool( string groupName, GameObject prefab, int initialSize )
    {
        if( !prefab.GetComponentInChildren<T>() )
        {
            throw new MissingComponentException( $"There is no '{typeof( T ).Name}' attached to the '{prefab.name}'" );
        }

        this.prefab = prefab;
        parent = GameObject.Find( groupName )?.transform ?? new GameObject( groupName ).transform;

        available = new Queue<T>( initialSize );
        gameObjects = new List<GameObject>( initialSize );

        for( var i = 0; i < initialSize; i++ )
        {
            available.Enqueue( CreateObject() );
        }
    }

    public T Get()
    {
        return available.Count > 0 ? available.Dequeue() : CreateObject();
    }

    public void Return( T obj )
    {
        available.Enqueue( obj );
    }

    public void Clear()
    {
        while( gameObjects.Count > 0 )
        {
            var gameObject = gameObjects[ 0 ];
            gameObjects.RemoveAt( 0 );

            var component = gameObject.GetComponentInChildren<T>();
            if( component )
            {
                #if UNITY_EDITOR
                    Object.DestroyImmediate( gameObject );
                #else
                    Object.Destroy( gameObject );
                #endif
            }
        }
        available.Clear();
    }

    //----------------------------------------------------------------------------------------------------
    
    readonly GameObject prefab;
    readonly Transform parent;
    readonly Queue<T> available;
    readonly List<GameObject> gameObjects;

    T CreateObject()
    {
        var newObject = Object.Instantiate( prefab );
        gameObjects.Add( newObject );

        newObject.name = prefab.name;
        newObject.transform.parent = parent;
        newObject.SetActive( false );

        return newObject.GetComponentInChildren<T>();
    }
}
