// https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/

using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu( "UI/Extensions/UILineRenderer" )]
    [RequireComponent( typeof( RectTransform ) )]
    public class UILineRenderer : MaskableGraphic
    {
        UILineRenderer()
        {
            useLegacyMeshGeneration = false;
        }


        const float MIN_MITER_JOIN = 15f * Mathf.Deg2Rad;
        const float MIN_BEVEL_NICE_JOIN = 30f * Mathf.Deg2Rad;


        public enum JoinType
        {
            Bevel, Miter
        }

        [Tooltip( "The type of Join used between lines, Square/Mitre or Curved/Bevel" )]
        public JoinType LineJoins = JoinType.Bevel;

        [SerializeField, Tooltip( "Points to draw lines between\n Can be improved using the Resolution Option" )]
        Vector2[] points;

        [SerializeField, Tooltip( "Thickness of the line" )]
        float lineThickness = 2f;


        public float LineThickness
        {
            get
            {
                return lineThickness;
            }
            set
            {
                lineThickness = value;
                SetAllDirty();
            }
        }

        public Vector2[] Points
        {
            get
            {
                return points;
            }
            set
            {
                if( points == value )
                {
                    return;
                }

                points = value;
                SetAllDirty();
            }
        }


        protected override void OnPopulateMesh( VertexHelper vertexHelper )
        {
            if( points == null )
            {
                return;
            }

            // scale based on the size of the rect or use absolute, this is switchable
            if( !rectTransform )
            {
                rectTransform = GetComponent<RectTransform>();
            }

            var offsetX = -rectTransform.pivot.x;
            var offsetY = -rectTransform.pivot.y;

            vertexHelper.Clear();

            // Generate the quads that make up the wide line
            segments.Clear();
            for( var i = 1; i < points.Length; i++ )
            {
                var start = new Vector2( points[ i - 1 ].x + offsetX, points[ i - 1 ].y + offsetY );
                var end = new Vector2( points[ i ].x + offsetX, points[ i ].y + offsetY );

                segments.Add( CreateLineSegment( start, end ) );
            }

            // Add the line segments to the vertex helper, creating any joins as needed
            for( var i = 0; i < segments.Count; i++ )
            {
                if( i < segments.Count - 1 )
                {
                    var vec1 = segments[ i ][ 1 ].position - segments[ i ][ 2 ].position;
                    var vec2 = segments[ i + 1 ][ 2 ].position - segments[ i + 1 ][ 1 ].position;
                    var angle = Vector2.Angle( vec1, vec2 ) * Mathf.Deg2Rad;

                    // Positive sign means the line is turning in a 'clockwise' direction
                    var sign = Mathf.Sign( Vector3.Cross( vec1.normalized, vec2.normalized ).z );

                    // Calculate the miter point
                    var miterDistance = lineThickness / ( 2f * Mathf.Tan( angle / 2f ) );
                    var miterDistance2 = miterDistance * miterDistance;
                    var miterPointA = segments[ i ][ 2 ].position - vec1.normalized * ( miterDistance * sign );
                    var miterPointB = segments[ i ][ 3 ].position + vec1.normalized * ( miterDistance * sign );

                    var joinType = LineJoins;
                    if( joinType == JoinType.Miter )
                    {
                        // Make sure we can make a miter join without too many artifacts.
                        if( miterDistance2 < vec1.sqrMagnitude / 2f && miterDistance2 < vec2.sqrMagnitude / 2f && angle > MIN_MITER_JOIN )
                        {
                            segments[ i ][ 2 ].position = miterPointA;
                            segments[ i ][ 3 ].position = miterPointB;
                            segments[ i + 1 ][ 0 ].position = miterPointB;
                            segments[ i + 1 ][ 1 ].position = miterPointA;
                        }
                        else
                        {
                            joinType = JoinType.Bevel;
                        }
                    }

                    if( joinType == JoinType.Bevel )
                    {
                        if( miterDistance2 < vec1.sqrMagnitude / 2f && miterDistance2 < vec2.sqrMagnitude / 2f && angle > MIN_BEVEL_NICE_JOIN )
                        {
                            if( sign < 0 )
                            {
                                segments[ i ][ 2 ].position = miterPointA;
                                segments[ i + 1 ][ 1 ].position = miterPointA;
                            }
                            else
                            {
                                segments[ i ][ 3 ].position = miterPointB;
                                segments[ i + 1 ][ 0 ].position = miterPointB;
                            }
                        }

                        var join = new[] { segments[ i ][ 2 ], segments[ i ][ 3 ], segments[ i + 1 ][ 0 ], segments[ i + 1 ][ 1 ] };
                        vertexHelper.AddUIVertexQuad( join );
                    }
                }

                vertexHelper.AddUIVertexQuad( segments[ i ] );
            }

            if( vertexHelper.currentVertCount > 64000 )
            {
                Debug.LogError( "Max Verticies size is 64000, current mesh vertcies count is [" + vertexHelper.currentVertCount + "] - Cannot Draw" );
                vertexHelper.Clear();
            }
        }

        UIVertex[] CreateLineSegment( Vector2 start, Vector2 end )
        {
            var offset = new Vector2( start.y - end.y, end.x - start.x ).normalized * lineThickness / 2f;
            var vertices = new[] { start - offset, start + offset, end + offset, end - offset };

            var vbo = new UIVertex[ 4 ];
            for( var i = 0; i < 4; i++ )
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[ i ];

                vbo[ i ] = vert;
            }

            return vbo;
        }


        new RectTransform rectTransform;
        List<UIVertex[]> segments = new List<UIVertex[]>();
    }
}