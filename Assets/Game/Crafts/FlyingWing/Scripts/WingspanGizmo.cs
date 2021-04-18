using UnityEngine;

public class WingspanGizmo : MonoBehaviour
{
    public float wingspan = 1f;

    public Color gizmoColor = Color.black;


    void OnDrawGizmosSelected()
    {
        var gizmosMatrixTemp = Gizmos.matrix;
        var gizmosColorTemp = Gizmos.color;

        Gizmos.color = gizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        
        var leftWingTip = Vector3.left * ( wingspan * 0.5f );
        var rightWingTip = Vector3.right * ( wingspan * 0.5f );
        Gizmos.DrawLine( leftWingTip, rightWingTip );
        Gizmos.DrawRay( leftWingTip, Vector3.back * ( wingspan * 0.1f ) );
        Gizmos.DrawRay( rightWingTip, Vector3.back * ( wingspan * 0.1f ) );

        Gizmos.matrix = gizmosMatrixTemp;
        Gizmos.color = gizmosColorTemp;
    }
}