using System.Globalization;
using TMPro;
using UnityEngine;

public class OSDElement : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    
    public FloatVariable floatVariable;
    public float valueScale = 1f;
    
    public float updateRate = 30f;

    public bool useCustomTextFormat;
    public string customTextFormat;
    
    
    protected string textFormat;
    CustomUpdate customUpdate;
    
    
    void Awake()
    {
        textFormat = useCustomTextFormat ? customTextFormat : targetText.text;

        customUpdate = new CustomUpdate( updateRate );
        customUpdate.OnUpdate += OnUpdate;
    }

    void Update()
    {
        customUpdate.Update( Time.time );
    }
    
    
    protected virtual void OnUpdate( float deltaTime )
    {
        targetText.text = ( floatVariable.Value * valueScale ).ToString( textFormat, CultureInfo.InvariantCulture );
    }
}
