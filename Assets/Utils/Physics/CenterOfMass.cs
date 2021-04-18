using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public Vector3 value;
    
    public Color gizmoColor = Color.red;
    public float gizmoRadius = 0.005f;
    

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = value;
    }

    void OnDrawGizmosSelected()
    {
        var gizmosMatrixTemp = Gizmos.matrix;
        var gizmosColorTemp = Gizmos.color;

        Gizmos.color = gizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere( value, gizmoRadius );

        Gizmos.matrix = gizmosMatrixTemp;
        Gizmos.color = gizmosColorTemp;
    }
}
