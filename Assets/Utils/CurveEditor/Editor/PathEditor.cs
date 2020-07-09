using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( PathCreator ) )]
public class PathEditor : Editor
{
    PathCreator creator;

    Path Path => creator.path;

    const float segmentSelectDistanceThreshold = 0.1f;
    int selectedSegmentIndex = -1;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if( GUILayout.Button( "Create new" ) )
        {
            Undo.RecordObject( creator, "Create new" );
            creator.CreatePath();
        }

        var isClosed = GUILayout.Toggle( Path.IsClosed, "Closed" );
        if( isClosed != Path.IsClosed )
        {
            Undo.RecordObject( creator, "Toggle closed" );
            Path.IsClosed = isClosed;
        }
        
        if( EditorGUI.EndChangeCheck() )
        {
            SceneView.RepaintAll();
        }
    }

    
    void OnEnable()
    {
        creator = (PathCreator) target;
        if( creator.path == null )
        {
            creator.CreatePath();
        }
    }
    
    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    
    void Input()
    {
        var guiEvent = Event.current;
        var mousePos = HandleUtility.GUIPointToWorldRay( guiEvent.mousePosition ).origin;
        mousePos.y = creator.transform.position.y;

        if( guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift )
        {
            if( selectedSegmentIndex != -1 )
            {
                Undo.RecordObject( creator, "Split segment" );
                Path.SplitSegment( mousePos, selectedSegmentIndex );
            }
            else if( !Path.IsClosed )
            {
                Undo.RecordObject( creator, "Add segment" );
                Path.AddSegment( mousePos );
            }
        }

        if( guiEvent.type == EventType.MouseDown && guiEvent.button == 1 )
        {
            var minDstToAnchor = creator.anchorDiameter * .5f;
            var closestAnchorIndex = -1;

            for( var i = 0; i < Path.NumPoints; i += 3 )
            {
                var dst = Vector3.Distance( mousePos, Path[ i ] );
                if( dst < minDstToAnchor )
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if( closestAnchorIndex != -1 )
            {
                Undo.RecordObject( creator, "Delete segment" );
                Path.DeleteSegment( closestAnchorIndex );
            }
        }

        if( guiEvent.type == EventType.MouseMove )
        {
            var minDstToSegment = segmentSelectDistanceThreshold;
            var newSelectedSegmentIndex = -1;

            for( var i = 0; i < Path.NumSegments; i++ )
            {
                var points = Path.GetPointsInSegment( i );
                var dst = HandleUtility.DistancePointBezier( mousePos, points[ 0 ], points[ 3 ], points[ 1 ], points[ 2 ] );
                if( dst < minDstToSegment )
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if( newSelectedSegmentIndex != selectedSegmentIndex )
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }

        HandleUtility.AddDefaultControl( 0 );
    }

    void Draw()
    {
        for( var i = 0; i < Path.NumSegments; i++ )
        {
            var points = Path.GetPointsInSegment( i );
            if( creator.displayControlPoints )
            {
                Handles.color = Color.black;
                Handles.DrawLine( points[ 1 ], points[ 0 ] );
                Handles.DrawLine( points[ 2 ], points[ 3 ] );
            }

            var segmentCol = ( i == selectedSegmentIndex && Event.current.shift ) ? creator.selectedSegmentCol : creator.segmentCol;
            Handles.DrawBezier( points[ 0 ], points[ 3 ], points[ 1 ], points[ 2 ], segmentCol, null, 2 );
        }


        for( var i = 0; i < Path.NumPoints; i++ )
        {
            if( i % 3 == 0 || creator.displayControlPoints )
            {
                Handles.color = ( i % 3 == 0 ) ? creator.anchorCol : creator.controlCol;
                var handleSize = ( i % 3 == 0 ) ? creator.anchorDiameter : creator.controlDiameter;
                var newPos = Handles.FreeMoveHandle( Path[ i ], Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap );
                if( Path[ i ] != newPos )
                {
                    Undo.RecordObject( creator, "Move point" );
                    Path.MovePoint( i, newPos );
                }
            }
        }
    }
}