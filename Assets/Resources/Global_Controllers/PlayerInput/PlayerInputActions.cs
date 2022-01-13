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
            ""name"": ""PlayerControls"",
            ""id"": ""ab74394f-bc84-4130-aaab-f4af32b14953"",
            ""actions"": [
                {
                    ""name"": ""TapDown"",
                    ""type"": ""Button"",
                    ""id"": ""a780930a-a351-4071-8764-c6f94a12f7b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TapUp"",
                    ""type"": ""Button"",
                    ""id"": ""31d8fe0f-5704-49e4-a7e9-ff796bb7749a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TapPOS"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9bfc7660-5049-43d6-b6bf-301b444e4844"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TapDrag"",
                    ""type"": ""Button"",
                    ""id"": ""c5f5546a-6365-42a6-9739-a4920d7deb03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ae88ab79-d775-48f5-bd0b-4c3525ede35c"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4b2e151-fc5a-4f9a-b54a-773f6df57737"",
                    ""path"": ""<Mouse>/leftButton"",
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
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""611842ee-91c0-4bf4-8c1a-30ebe9027ad6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99a6d119-c11e-4b90-a537-8e21c95cafd8"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapPOS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""494b12d2-5016-4cba-9a64-2cc2389aebc5"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapPOS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43b62d36-f0f2-4973-978c-fb2a6e43d27b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapDrag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc5b5ae2-6d9c-4060-930a-b15b9a6bcec1"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapDrag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_TapDown = m_PlayerControls.FindAction("TapDown", throwIfNotFound: true);
        m_PlayerControls_TapUp = m_PlayerControls.FindAction("TapUp", throwIfNotFound: true);
        m_PlayerControls_TapPOS = m_PlayerControls.FindAction("TapPOS", throwIfNotFound: true);
        m_PlayerControls_TapDrag = m_PlayerControls.FindAction("TapDrag", throwIfNotFound: true);
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

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_TapDown;
    private readonly InputAction m_PlayerControls_TapUp;
    private readonly InputAction m_PlayerControls_TapPOS;
    private readonly InputAction m_PlayerControls_TapDrag;
    public struct PlayerControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TapDown => m_Wrapper.m_PlayerControls_TapDown;
        public InputAction @TapUp => m_Wrapper.m_PlayerControls_TapUp;
        public InputAction @TapPOS => m_Wrapper.m_PlayerControls_TapPOS;
        public InputAction @TapDrag => m_Wrapper.m_PlayerControls_TapDrag;
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
                @TapPOS.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapPOS;
                @TapPOS.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapPOS;
                @TapPOS.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapPOS;
                @TapDrag.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapDrag;
                @TapDrag.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapDrag;
                @TapDrag.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTapDrag;
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
                @TapPOS.started += instance.OnTapPOS;
                @TapPOS.performed += instance.OnTapPOS;
                @TapPOS.canceled += instance.OnTapPOS;
                @TapDrag.started += instance.OnTapDrag;
                @TapDrag.performed += instance.OnTapDrag;
                @TapDrag.canceled += instance.OnTapDrag;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    public interface IPlayerControlsActions
    {
        void OnTapDown(InputAction.CallbackContext context);
        void OnTapUp(InputAction.CallbackContext context);
        void OnTapPOS(InputAction.CallbackContext context);
        void OnTapDrag(InputAction.CallbackContext context);
    }
}
