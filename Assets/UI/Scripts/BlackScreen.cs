using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlackScreen : Singleton<BlackScreen>
{
    [SerializeField] 
    Image image = null;

    //[SerializeField]
    //bool startFromBlackScreenOnAwake = false;
    
    
    public void StartFromBlackScreenAnimation( Action onAnimationFinished = null )
    {
        var colorBlack = new Color( 0f, 0f, 0f, 1f );
        var colorBlackTransparent = new Color( 0f, 0f, 0f, 0f );
        
        image.enabled = true;
        image.color = colorBlack;

        if( blackScreenAnimationCoroutine != null )
        {
            StopCoroutine( blackScreenAnimationCoroutine );
        }
        blackScreenAnimationCoroutine = ImageColorLerpCoroutine( colorBlack, colorBlackTransparent, () =>
        {
            image.enabled = false;
            onAnimationFinished?.Invoke();
        } );
        StartCoroutine( blackScreenAnimationCoroutine );
    }

    public void StartToBlackScreenAnimation( Action onAnimationFinished = null )
    {
        var colorCurrent = image.color;
        var colorBlack = new Color( 0f, 0f, 0f, 1f );
        
        image.enabled = true;

        if( blackScreenAnimationCoroutine != null )
        {
            StopCoroutine( blackScreenAnimationCoroutine );
        }
        blackScreenAnimationCoroutine = ImageColorLerpCoroutine( colorCurrent, colorBlack, () =>
        {
            onAnimationFinished?.Invoke();
        } );
        StartCoroutine( blackScreenAnimationCoroutine );
    }


    //void Awake()
    //{
    //    throw new NotImplementedException();
    //}

    /*
    void Update()
    {
        //image.color = Color.Lerp( new Color( 0f, 0f, 0f, 0f ), new Color( 0f, 0f, 0f, 1f ), Mathf.PingPong( Time.time, 1f ) );

        if( Keyboard.current.aKey.wasPressedThisFrame )
        {
            StartFromBlackScreenAnimation();
        }
        if( Keyboard.current.dKey.wasPressedThisFrame )
        {
            StartToBlackScreenAnimation();
        }
    }
*/

    IEnumerator ImageColorLerpCoroutine( Color colorA, Color colorB, Action onFinished = null )
    {
        var transition = 0f;
        var speed = 1f;
        
        while( transition < 1f )
        {
            transition += Time.deltaTime * speed;
            image.color = Color.Lerp( colorA, colorB, transition );
            
            yield return null;
        }

        blackScreenAnimationCoroutine = null;
        onFinished?.Invoke();        
    }
    IEnumerator blackScreenAnimationCoroutine;
}