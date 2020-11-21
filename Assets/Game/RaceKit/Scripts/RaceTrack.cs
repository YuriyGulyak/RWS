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

    public void ResetProgressFor( GameObject craft )
    {
        var craftID = craft.GetInstanceID();
        
        if( trackProgressDictionary.ContainsKey( craftID ) )
        {
            trackProgressDictionary[ craftID ] = new TrackProgress();
        }
    }


    struct TrackProgress
    {
        public int nextGateIndex;
        public int prevGateIndex;
    }
    Dictionary<int, TrackProgress> trackProgressDictionary;
    
    int gateCount;
    
    
    void OnValidate()
    {
        if( gates == null || gates.Length == 0 )
        {
            gates = GetComponentsInChildren<RaceGate>();
        }
    }

    void Awake()
    {
        trackProgressDictionary = new Dictionary<int,TrackProgress>();

        gateCount = gates.Length;
        
        for( var i = 0; i < gateCount; i++ )
        {
            var gateIndex = i;
            var gate = gates[ gateIndex ];
            
            gate.OnSuccess.AddListener( craft => OnGateSuccess( gateIndex, craft ) );
        }
    }


    void OnGateSuccess( int gateIndex, GameObject target )
    {
        var targetID = target.GetInstanceID();

        if( !trackProgressDictionary.ContainsKey( targetID ) )
        {
            trackProgressDictionary.Add( targetID, new TrackProgress() );
        }
        var trackProgress = trackProgressDictionary[ targetID ];

        if( gateIndex == trackProgress.nextGateIndex )
        {
            if( trackProgress.nextGateIndex == 0 && trackProgress.prevGateIndex == gateCount - 1 )
            {
                OnFinish.Invoke( target );
            }

            trackProgress.prevGateIndex = trackProgress.nextGateIndex;
            trackProgress.nextGateIndex = ++trackProgress.nextGateIndex % gateCount;

            trackProgressDictionary[ targetID ] = trackProgress;
        }
        
        if( gateIndex == 0 )
        {
            OnStart.Invoke( target );
        }
    }
}