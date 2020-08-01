using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : Singleton<BlackScreen>
{
    [SerializeField] 
    Image image = null;

    [SerializeField]
    bool startOnAwake = false;
    

    public void StartFromBlackScreenAnimation( Action onAnimationFinished = null )
    {
        image.transform.SetAsLastSibling();
        
        var colorBlack = new Color( 0f, 0f, 0f, 1f );
        var colorBlackTransparent = new Color( 0f, 0f, 0f, 0f );
        
        image.enabled = true;
        image.color = colorBlack;

        if( blackScreenAnimationCoroutine != null )
        {
            StopCoroutine( blackScreenAnimationCoroutine );
        }
        blackScreenAnimationCoroutine = ImageColorLerpCoroutine( colorBlack, colorBlackTransparent, 1.5f, () =>
        {
            image.enabled = false;
            onAnimationFinished?.Invoke();
        } );
        StartCoroutine( blackScreenAnimationCoroutine );
    }

    public void StartToBlackScreenAnimation( Action onAnimationFinished = null )
    {
        image.transform.SetAsLastSibling();
    
        var colorCurrent = image.color;
        var colorBlack = new Color( 0f, 0f, 0f, 1f );
        
        image.enabled = true;

        if( blackScreenAnimationCoroutine != null )
        {
            StopCoroutine( blackScreenAnimationCoroutine );
        }
        blackScreenAnimationCoroutine = ImageColorLerpCoroutine( colorCurrent, colorBlack, 1f, () =>
        {
            onAnimationFinished?.Invoke();
        } );
        StartCoroutine( blackScreenAnimationCoroutine );
    }


    void Awake()
    {
        if( startOnAwake )
        {
            StartFromBlackScreenAnimation();
        }
    }


    IEnumerator ImageColorLerpCoroutine( Color colorA, Color colorB, float speed, Action onFinished = null )
    {
        var transition = 0f;

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