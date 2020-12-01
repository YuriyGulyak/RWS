//#if UNITY_EDITOR
using UnityEngine;

public class WingSetupHelper : MonoBehaviour
{
    [SerializeField]
    AirfoilSection[] leftSections = null;

    [SerializeField]
    AirfoilSection[] rightSections = null;
    
    [SerializeField] 
    bool alignSections = false;
    
    [SerializeField]
    AnimationCurve liftScaleVsPosition = AnimationCurve.EaseInOut( 0f, 1.5f, 47f, 0f );
    
    [SerializeField]
    AnimationCurve dragScaleVsPosition = AnimationCurve.EaseInOut( 0f, 1f, 47f, 1.5f );
    

    public void UpdateSections()
    {
        UpdateSections( leftSections, -1f );
        UpdateSections( rightSections, 1f );
    }

    
    void UpdateSections( AirfoilSection[] sections, float wingDirection )
    {
        for( var i = 0; i < sections.Length; i++ )
        {
            var section = sections[ i ];
            var sectionWidth = section.Scale.x;
            var sweepAngle = section.SweepAngle;
            
            var sectionLocalPosition = transform.InverseTransformPoint( section.Position );
            
            if( alignSections )
            {
                sectionLocalPosition.x = ( sectionWidth * i + sectionWidth / 2f ) * wingDirection;
                sectionLocalPosition.z = Mathf.Tan( Mathf.Abs( sweepAngle ) * -1f * Mathf.Deg2Rad ) * ( sectionWidth / 2f + sectionWidth * i );
                
                section.transform.localPosition = sectionLocalPosition;
            }
            
            section.LiftScale = liftScaleVsPosition.Evaluate( Mathf.Abs( sectionLocalPosition.x ) );
            section.DragScale = dragScaleVsPosition.Evaluate( Mathf.Abs( sectionLocalPosition.x ) );
        }
    }
}

//#endif