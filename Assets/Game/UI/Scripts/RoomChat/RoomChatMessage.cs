using System.Collections;
using TMPro;
using UnityEngine;

public class RoomChatMessage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMesh = null;

    [SerializeField]
    float lifeTime = 10f;


    public void Init( string text )
    {
        textMesh.text = text;
    }


    IEnumerator Start()
    {
        yield return new WaitForSeconds( lifeTime );

        var startTextColor = textMesh.color;
        var endTextColor = startTextColor;
        endTextColor.a = 0f;

        var transitionSpeed = 1f;
        var transition = 0f;

        while( transition < 1f )
        {
            transition += transitionSpeed * Time.deltaTime;
            textMesh.color = Color.Lerp( startTextColor, endTextColor, transition );
            yield return null;
        }
       
        Destroy( gameObject );
    }
}
