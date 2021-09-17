using System;
using System.Globalization;

public class OSDElementTime : OSDElement
{
    protected override void  OnUpdate( float deltaTime )
    {
        targetText.text = TimeSpan.FromSeconds( floatVariable.Value * valueScale ).ToString( textFormat, CultureInfo.InvariantCulture );
    }
}
