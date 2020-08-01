using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceTrack : MonoBehaviour
{
    [SerializeField]
    RaceGate[] gates = null;

    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }
    public GameObjectEvent OnStart = new GameObjectEvent();
    public GameObjectEvent OnFinish = new GameObjectEvent();
    

    struct TrackProgress
    {
        public int nextGateIndex;
        public int prevGateIndex;
    }
    Dictionary<int, TrackProgress> trackProgressDictionary;
    
    
    void OnValidate()
    {
        if( gates == null || gates.Length == 0 )
        {
            gates = GetComponentsInChildren<RaceGate>();
        }
    }

    void Start()
    {
        trackProgressDictionary = new Dictionary<int,TrackProgress>();
        
        for( var i = 0; i < gates.Length; i++ )
        {
            var gateIndex = i;
            var gate = gates[ gateIndex ];
            
            gate.OnSuccess.AddListener( craft => OnGateSuccess( gateIndex, craft ) );
        }
    }


    void OnGateSuccess( int gateIndex, GameObject craft )
    {
        var craftID = craft.GetInstanceID();

        if( !trackProgressDictionary.ContainsKey( craftID ) )
        {
            trackProgressDictionary.Add( craftID, new TrackProgress() );
        }
        var trackProgress = trackProgressDictionary[ craftID ];

        if( gateIndex == trackProgress.nextGateIndex )
        {
            if( trackProgress.nextGateIndex == 0 && trackProgress.prevGateIndex == gates.Length - 1 )
            {
                OnFinish.Invoke( craft );
            }

            trackProgress.prevGateIndex = trackProgress.nextGateIndex;
            trackProgress.nextGateIndex = ++trackProgress.nextGateIndex % gates.Length;

            trackProgressDictionary[ craftID ] = trackProgress;
        }
        
        if( gateIndex == 0 )
        {
            OnStart.Invoke( craft );
        }
    }
}