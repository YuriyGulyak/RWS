// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""7c414943-b190-47bd-8add-8906c39870f5"",
            ""actions"": [
                {
                    ""name"": ""Throttle"",
                    ""type"": ""Value"",
                    ""id"": ""e1ccef58-0f4a-41d2-934c-7fac018a724b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Value"",
                    ""id"": ""8c50cf82-00a9-43db-aae8-74df6b8dd872"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pitch"",
                    ""type"": ""Value"",
                    ""id"": ""5e8238ed-c2a0-4186-82a1-0e5c9c6f98de"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Yaw"",
                    ""type"": ""Value"",
                    ""id"": ""c694353b-2683-4df5-ac95-e024a5e72fe9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Trim"",
                    ""type"": ""Value"",
                    ""id"": ""41928cee-f5cd-4ef3-a76b-8a0c9fe750a0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Launch"",
                    ""type"": ""Button"",
                    ""id"": ""67addb26-2cd8-4980-b9e3-dd7d433968f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""448c3089-f383-4065-b156-b974b0606074"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff97f9b4-c961-4e43-86f7-4001e659879d"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/rx"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b12dfe6b-1c83-4e44-90d5-7e4c8b3257f4"",
                    ""path"": ""<HID::Logitech Logitech Force 3D Pro>/stick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f122b0cb-0688-40e1-8461-e41d95348639"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/ry"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9eba0a8d-73c2-4831-a7b4-86588c2f5c04"",
                    ""path"": ""<HID::Logitech Logitech Force 3D Pro>/stick/y"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9eef25cc-bac5-445c-9eb7-d96bc94501f0"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/stick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Yaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4fb4bc44-e139-44cd-8134-cd5592e1d091"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/slider"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Trim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf0edbb7-8ddf-4c16-b815-624c770d0772"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Launch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1247bf6-dae1-4783-8714-bf24d81cb35b"",
                    ""path"": ""<HID::Logitech Logitech Force 3D Pro>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Launch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64ccebbc-f9e8-418c-ae01-d88c0bcbb2b8"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7553bffe-7c11-42ec-b417-8ca80b7048f3"",
                    ""path"": ""<HID::Logitech Logitech Force 3D Pro>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fcfc01fb-f8ae-49dc-a584-8ee623f7319b"",
                    ""path"": ""<HID::FrSky FrSky Taranis Joystick>/stick/y"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0189500-ed28-42b3-a6f0-d20bd5353dbb"",
                    ""path"": ""<HID::Logitech Logitech Force 3D Pro>/slider"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_Throttle = m_Game.FindAction("Throttle", throwIfNotFound: true);
        m_Game_Roll = m_Game.FindAction("Roll", throwIfNotFound: true);
        m_Game_Pitch = m_Game.FindAction("Pitch", throwIfNotFound: true);
        m_Game_Yaw = m_Game.FindAction("Yaw", throwIfNotFound: true);
        m_Game_Trim = m_Game.FindAction("Trim", throwIfNotFound: true);
        m_Game_Launch = m_Game.FindAction("Launch", throwIfNotFound: true);
        m_Game_Restart = m_Game.FindAction("Restart", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_Throttle;
    private readonly InputAction m_Game_Roll;
    private readonly InputAction m_Game_Pitch;
    private readonly InputAction m_Game_Yaw;
    private readonly InputAction m_Game_Trim;
    private readonly InputAction m_Game_Launch;
    private readonly InputAction m_Game_Restart;
    public struct GameActions
    {
        private @Controls m_Wrapper;
        public GameActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Throttle => m_Wrapper.m_Game_Throttle;
        public InputAction @Roll => m_Wrapper.m_Game_Roll;
        public InputAction @Pitch => m_Wrapper.m_Game_Pitch;
        public InputAction @Yaw => m_Wrapper.m_Game_Yaw;
        public InputAction @Trim => m_Wrapper.m_Game_Trim;
        public InputAction @Launch => m_Wrapper.m_Game_Launch;
        public InputAction @Restart => m_Wrapper.m_Game_Restart;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @Throttle.started -= m_Wrapper.m_GameActionsCallbackInterface.OnThrottle;
                @Throttle.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnThrottle;
                @Throttle.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnThrottle;
                @Roll.started -= m_Wrapper.m_GameActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnRoll;
                @Pitch.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPitch;
                @Pitch.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPitch;
                @Pitch.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPitch;
                @Yaw.started -= m_Wrapper.m_GameActionsCallbackInterface.OnYaw;
                @Yaw.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnYaw;
                @Yaw.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnYaw;
                @Trim.started -= m_Wrapper.m_GameActionsCallbackInterface.OnTrim;
                @Trim.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnTrim;
                @Trim.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnTrim;
                @Launch.started -= m_Wrapper.m_GameActionsCallbackInterface.OnLaunch;
                @Launch.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnLaunch;
                @Launch.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnLaunch;
                @Restart.started -= m_Wrapper.m_GameActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnRestart;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Throttle.started += instance.OnThrottle;
                @Throttle.performed += instance.OnThrottle;
                @Throttle.canceled += instance.OnThrottle;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Pitch.started += instance.OnPitch;
                @Pitch.performed += instance.OnPitch;
                @Pitch.canceled += instance.OnPitch;
                @Yaw.started += instance.OnYaw;
                @Yaw.performed += instance.OnYaw;
                @Yaw.canceled += instance.OnYaw;
                @Trim.started += instance.OnTrim;
                @Trim.performed += instance.OnTrim;
                @Trim.canceled += instance.OnTrim;
                @Launch.started += instance.OnLaunch;
                @Launch.performed += instance.OnLaunch;
                @Launch.canceled += instance.OnLaunch;
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
            }
        }
    }
    public GameActions @Game => new GameActions(this);
    public interface IGameActions
    {
        void OnThrottle(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnPitch(InputAction.CallbackContext context);
        void OnYaw(InputAction.CallbackContext context);
        void OnTrim(InputAction.CallbackContext context);
        void OnLaunch(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
    }
}
