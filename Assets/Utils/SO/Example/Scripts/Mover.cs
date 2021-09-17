using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    FloatVariable horizontalAxis = default;
    
    [SerializeField]
    FloatVariable verticalAxis = default;

    [SerializeField]
    FloatVariable moveSpeed;


    void Start()
    {
        //horizontalAxis.OnValueChanged.AddListener( v => print( v ) );
    }

    void Update()
    {
        transform.position += new Vector3( horizontalAxis.Value, 0f, verticalAxis.Value ) * ( moveSpeed.Value * Time.deltaTime );
    }
    
    
    void OnMouseDown()
    {
        print( "OnMouseDown" );
    }

    void OnMouseUp()
    {
        print( "OnMouseUp" );
    }
}
