//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Resources/Global_Controllers/PlayerInput/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""MapControls"",
            ""id"": ""35bdeb0d-c0ff-4988-a142-6669c7d1baea"",
            ""actions"": [
                {
                    ""name"": ""SelectPOI_tap"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5e2dcbc6-70f3-4f5b-a654-e03bd48fce1c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""00d9b8ba-2205-47df-af14-b0a3cbbe9f1b"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectPOI_tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""271c6873-f74c-47bc-a0a3-61adea0d31e5"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectPOI_tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06253ed8-6ef9-4cde-93d8-d0c90429ad2a"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectPOI_tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""020d1ed3-257d-4bc6-9a96-418a15a01b31"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectPOI_tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerControls"",
            ""id"": ""ab74394f-bc84-4130-aaab-f4af32b14953"",
            ""actions"": [
                {
                    ""name"": ""TapDown"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a780930a-a351-4071-8764-c6f94a12f7b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TapUp"",
                    ""type"": ""Button"",
                    ""id"": ""31d8fe0f-5704-49e4-a7e9-ff796bb7749a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ae88ab79-d775-48f5-bd0b-4c3525ede35c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7dbe0d0b-2bc6-4ddf-adcd-2dd496fe998a"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MapControls
        m_MapControls = asset.FindActionMap("MapControls", throwIfNotFound: true);
        m_MapControls_SelectPOI_tap = m_MapControls.FindAction("SelectPOI_tap", throwIfNotFound: true);
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_TapDown = m_PlayerControls.FindAction("TapDown", throwIfNotFound: true);
        m_PlayerControls_TapUp = m_PlayerControls.FindAction("TapUp", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // MapControls
    private readonly InputActionMap m_MapControls;
    private IMapControlsActions m_MapControlsActionsCallbackInterface;
    private readonly InputAction m_MapControls_SelectPOI_tap;
    public struct MapControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public MapControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectPOI_tap => m_Wrapper.m_MapControls_SelectPOI_tap;
        public InputActionMap Get() { return m_Wrapper.m_MapControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapControlsActions set) { return set.Get(); }
        public void SetCallbacks(IMapControlsActions instance)
        {
            if (m_Wrapper.m_MapControlsActionsCallbackInterface != null)
            {
                @SelectPOI_tap.started -= m_Wrapper.m_MapControlsActionsCallbackInterface.OnSelectPOI_tap;
                @SelectPOI_tap.performed -= m_Wrapper.m_MapControlsActionsCallbackInterface.OnSelectPOI_tap;
                @SelectPOI_tap.canceled -= m_Wrapper.m_MapControlsActionsCallbackInterface.OnSelectPOI_tap;
            }
            m_Wrapper.m_MapControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectPOI_tap.started += instance.OnSelectPOI_tap;
                @SelectPOI_tap.performed += instance.OnSelectPOI_tap;
                @SelectPOI_tap.canceled += instance.OnSelectPOI_tap;
            }
        }
    }
    public MapControlsActions @MapControls => new MapControlsActions(this);

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_TapDown;
    private readonly InputAction m_PlayerControls_TapUp;
    public struct PlayerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TapDown => m_Wrapper.m_PlayerControls_TapDown;
        public InputAction @TapUp => m_Wrapper.m_PlayerControls_TapUp;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @TapDown.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapDown;
                @TapDown.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapDown;
                @TapDown.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapDown;
                @TapUp.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapUp;
                @TapUp.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapUp;
                @TapUp.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapUp;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TapDown.started += instance.OnTapDown;
                @TapDown.performed += instance.OnTapDown;
                @TapDown.canceled += instance.OnTapDown;
                @TapUp.started += instance.OnTapUp;
                @TapUp.performed += instance.OnTapUp;
                @TapUp.canceled += instance.OnTapUp;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    public interface IMapControlsActions
    {
        void OnSelectPOI_tap(InputAction.CallbackContext context);
    }
    public interface IPlayerControlsActions
    {
        void OnTapDown(InputAction.CallbackContext context);
        void OnTapUp(InputAction.CallbackContext context);
    }
}
