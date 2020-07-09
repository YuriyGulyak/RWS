using System;
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


    int targetGateIndex;
    int prevGateIndex;
    
    
    void OnValidate()
    {
        if( gates == null || gates.Length == 0 )
        {
            gates = GetComponentsInChildren<RaceGate>();
        }
    }

    void Start()
    {
        for( var i = 0; i < gates.Length; i++ )
        {
            var gateIndex = i;
            var gate = gates[ gateIndex ];
            gate.OnSuccess.AddListener( craft => OnGateSuccess( gateIndex, craft ) );
        }
    }


    void OnGateSuccess( int gateIndex, GameObject craft )
    {
        if( gateIndex == targetGateIndex )
        {
            if( targetGateIndex == 0 && prevGateIndex == gates.Length - 1 )
            {
                OnFinish.Invoke( craft );
            }

            prevGateIndex = targetGateIndex;
            targetGateIndex = ++targetGateIndex % gates.Length;
        }
        
        if( gateIndex == 0 )
        {
            OnStart.Invoke( craft );
        }
    }
}