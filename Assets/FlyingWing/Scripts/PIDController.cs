using System;

[Serializable]
class PIDController
{
    public float pGain = 0f;
    public float iGain = 0f;
    public float dGain = 0f;
    
    public float pTerm;
    public float iTerm;
    public float dTerm;
    
    public float output = 0f;

    float errorPrev;
        
    
    public float UpdateState( float process, float target, float deltaTime )
    {
        var error = target - process;
        var errorDelta = error - errorPrev;
        errorPrev = error;

        pTerm = error * pGain;
        iTerm += error * iGain * deltaTime;
        dTerm = errorDelta * dGain;
            
        return output = pTerm + iTerm + dTerm;
    }
}
