using System;
using System.Collections;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
#endif

namespace GarmentButton.InputControl
{


    public class TardovInputController : MonoBehaviour
    {
        public FrameInput FrameInput { get; private set; }

        private void Update()
        {

            FrameInput = Gather();
        }


#if ENABLE_INPUT_SYSTEM
        public int DeviceNumber;
        public PlayerInputActions Actions;
        private InputAction _move, _jump, _dash, _attack, _defend, _changeType;
        public Action FinishedInput;
        private void Awake()
        {
            Actions = new PlayerInputActions();
            /*            Actions.devices = new[] { Gamepad.all[0] };
                        Actions.bindingMask = InputBinding.MaskByGroup("Gamepad");*/
            if (Gamepad.all.Count > 1)
            {
                if(Gamepad.all.Count < DeviceNumber + 1)
                {
                    gameObject.SetActive(false);
                    return;

                }
                var p1User = InputUser.PerformPairingWithDevice(Gamepad.all[DeviceNumber]);
                p1User.AssociateActionsWithUser(Actions);
                p1User.ActivateControlScheme("Gamepad");
            }


            // Actions.devices = new[] { Gamepad.all[0] };

            //Actions.bindingMask = InputBinding.MaskByGroup("Gamepad");


            _move = Actions.Player.Move;
            _changeType = Actions.Player.ChangeType;
            _jump = Actions.Player.Jump;
            _dash = Actions.Player.Dash;
            _attack = Actions.Player.Attack;
            _defend = Actions.Player.Defend;
            StartCoroutine(LateStart());
        }


        IEnumerator LateStart()
        {
            yield return null;
            FinishedInput?.Invoke();
        }
        private void OnEnable()
        {
            Actions.Enable();
        }

        private void OnDisable()
        {
            Actions.Disable();
        }

        private FrameInput Gather()
        {
            return new FrameInput
            {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),
                AttackDown = _attack.WasPressedThisFrame(),
                DefenseDown = _defend.WasPressedThisFrame(),
                Move = _move.ReadValue<Vector2>(),
                ChangeType = _changeType.ReadValue<Vector2>()
            };
        }

#elif ENABLE_LEGACY_INPUT_MANAGER
        private FrameInput Gather() {
            return new FrameInput {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                DashDown = Input.GetKeyDown(KeyCode.X),
                AttackDown = Input.GetKeyDown(KeyCode.Z),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
            };
        }
#endif
    }

    public struct FrameInput
    {
        public Vector2 Move;
        public Vector2 ChangeType;
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool AttackDown;
        public bool DefenseDown;
    }
}
