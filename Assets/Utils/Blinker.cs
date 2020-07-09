using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Blinker : MonoBehaviour
{
    public UnityEvent OnShow;
    public UnityEvent OnHide;

    [SerializeField]
    float interval = 0.5f;
    

    void OnEnable()
    {
        if( blinkCoroutine != null )
        {
            StopCoroutine( blinkCoroutine );
        }
        blinkCoroutine = BlinkCoroutine();
        StartCoroutine( blinkCoroutine );
    }

    void OnDisable()
    {
        if( blinkCoroutine != null )
        {
            StopCoroutine( blinkCoroutine );
            blinkCoroutine = null;
        }
    }


    IEnumerator BlinkCoroutine()
    {
        var delay = new WaitForSeconds( interval );
        var visible = true;
        
        while( true )
        {
            if( visible )
            {
                OnHide.Invoke();
            }
            else
            {
                OnShow.Invoke();
            }
            visible = !visible;

            yield return delay;
        }
    }
    IEnumerator blinkCoroutine;
}
