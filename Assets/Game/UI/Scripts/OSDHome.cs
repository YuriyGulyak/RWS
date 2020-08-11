using System.Globalization;
using TMPro;
using UnityEngine;

public class OSDHome : MonoBehaviour
{
    [SerializeField]
    FlyingWing flyingWing = null;

    [SerializeField]
    RectTransform directionRect = null;
    
    [SerializeField]
    TextMeshProUGUI distanceText = null;

    [SerializeField]
    float updateRate = 30f;
    
    
    public void Init( FlyingWing flyingWing )
    {
        this.flyingWing = flyingWing;
    }
    
    public bool IsActive => gameObject.activeSelf;
    
    public void Show()
    {
        if( !IsActive )
        {
            gameObject.SetActive( true );
        }
    }

    public void Hide()
    {
        if( IsActive )
        {
            gameObject.SetActive( false );
        }
    }

    public void Reset()
    {
        newArrowRotation = Quaternion.identity;
        directionRect.rotation = Quaternion.identity;
        distanceText.text = 0f.ToString( distanceFormat, cultureInfo );
    }


    string distanceFormat;
    CultureInfo cultureInfo;
    float lastUpdateTime;
    Rigidbody wingRigidbody;
    //Transform wingTransform;
    Vector3 homePosition;
    Quaternion newArrowRotation;
    

    void Awake()
    {
        distanceFormat = distanceText.text;
        cultureInfo = CultureInfo.InvariantCulture;

        if( flyingWing )
        {
            wingRigidbody = flyingWing.GetComponent<Rigidbody>();
            homePosition = wingRigidbody.position;
            //wingTransform = flyingWing.transform;
            //homePosition = wingTransform.position;
        }
    }

    void Update()
    {
        if( Time.time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = Time.time;
            UpdateUI();
        }

        directionRect.rotation = Quaternion.Lerp( directionRect.rotation, newArrowRotation, Time.deltaTime * 10f );
    }


    void UpdateUI()
    {
        if( flyingWing && flyingWing.TAS > 0.1f )
        {
            //var wingPosition = wingTransform.position;
            var wingPosition = wingRigidbody.position;
            wingPosition.y = homePosition.y;

            var vectorToHome = homePosition - wingPosition;
            var distanceToHome = vectorToHome.magnitude;

            if( distanceToHome > 1f )
            {
                //var wingForward = wingTransform.forward;
                var wingForward = wingRigidbody.velocity;
                wingForward.y = 0f;
            
                var rotationToHome = Quaternion.FromToRotation( wingForward.normalized, vectorToHome.normalized );
                var courseToHome = MathUtils.WrapAngle180( rotationToHome.eulerAngles.y );

                newArrowRotation = Quaternion.Euler( 0f, 0f, -courseToHome );
                //directionRect.rotation = Quaternion.Euler( 0f, 0f, -courseToHome );
            }

            distanceText.text = distanceToHome.ToString( distanceFormat, cultureInfo );
        }
    }
}
