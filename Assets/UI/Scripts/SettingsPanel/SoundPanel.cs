using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour
{
    [SerializeField]
    SliderWithInputField masterVolumeSlider = null;
    
    [SerializeField]
    SliderWithInputField motorVolumeSlider = null;
    
    [SerializeField]
    SliderWithInputField windVolumeSlider = null;
    
    [SerializeField]
    Button backButton = null;

    [SerializeField]
    Button applyButton = null;
    
    //----------------------------------------------------------------------------------------------------

    public void Show( Action onBackButtonCallback = null )
    {
        this.onBackButtonCallback = onBackButtonCallback;
        gameObject.SetActive( true );

        applyButton.gameObject.SetActive( false );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onBackButtonCallback = null;
    }

    //----------------------------------------------------------------------------------------------------

    Action onBackButtonCallback;


    void Awake()
    {
        backButton.onClick.AddListener( OnBackButton );

        applyButton.onClick.AddListener( OnApplyButton );
        applyButton.gameObject.SetActive( false );
    }
    
    
    void OnBackButton()
    {
        onBackButtonCallback?.Invoke();
        Hide();
    }

    void OnApplyButton()
    {
        //soundsManager.MotorSoundVolume = 
        //soundsManager.WindSoundVolume = 

        applyButton.gameObject.SetActive( false );
    }
}