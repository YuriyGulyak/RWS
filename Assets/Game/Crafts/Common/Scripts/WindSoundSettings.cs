using UnityEngine;

[CreateAssetMenu]
public class WindSoundSettings : ScriptableObject
{
    // time - normalized speed
    public AnimationCurve volumeCurve = AnimationCurve.Linear( 0f, 0f, 1f, 1f );
    public AnimationCurve pitchCurve = AnimationCurve.Linear( 0f, 0.75f, 1f, 1f );

    // For normalize speed
    public float maxSpeedKmh = 160f;
}
