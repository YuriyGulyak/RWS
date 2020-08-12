﻿using System.Globalization;
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
        directionRect.rotation = newArrowRotation;
        distanceText.text = 0f.ToString( distanceFormat, cultureInfo );
    }


    string distanceFormat;
    CultureInfo cultureInfo;
    float lastUpdateTime;
    Rigidbody wingRigidbody;
    //Transform wingTransform;
    Vector3 homePosition;
    Quaternion newArrowRotation;
    Vector3 newArrowAngles;
    float courseToHome;
    

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
        var time = Time.time;
        if( time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = time;
            UpdateUI();
        }

        var directionRectAngles = directionRect.eulerAngles;
        directionRectAngles.z = Mathf.LerpAngle( directionRectAngles.z, courseToHome, Time.deltaTime * 10f );
        directionRect.eulerAngles = directionRectAngles;
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
                courseToHome = MathUtils.WrapAngle180( rotationToHome.eulerAngles.y ) * -1f;
            }

            distanceText.text = distanceToHome.ToString( distanceFormat, cultureInfo );
        }
    }
}
