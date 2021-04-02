using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class ControlListEntry : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI bindingName = null;

        [SerializeField]
        Toggle listenToggle = null;

        [SerializeField]
        Toggle invertToggle = null;

        //----------------------------------------------------------------------------------------------------

        public Action OnStartListening;
        public Action OnStopListening;
        public Action<bool> OnInvertChanged;

        public bool IsListening { get; private set; }

        public void StopListening()
        {
            listenToggle.isOn = false;
        }

        public string BindingName
        {
            get => bindingName.text;
            set => bindingName.text = value;
        }

        public bool Invert
        {
            get => invertToggle && invertToggle.isOn;
            set
            {
                if( invertToggle )
                {
                    invertToggle.isOn = value;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------

        void Awake()
        {
            listenToggle.onValueChanged.AddListener( value =>
            {
                if( value )
                {
                    OnStartListening?.Invoke();
                }
                else
                {
                    OnStopListening?.Invoke();
                }

                IsListening = value;

            } );

            if( invertToggle != null )
            {
                invertToggle.onValueChanged.AddListener( value => { OnInvertChanged?.Invoke( value ); } );
            }
        }
    }
}