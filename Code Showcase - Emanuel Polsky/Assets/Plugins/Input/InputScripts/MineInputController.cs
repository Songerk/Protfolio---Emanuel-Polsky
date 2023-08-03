using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GarmentButton.InputControl
{
    public class MineInputController : MonoBehaviour
    {


        private TardovInputController barrowInput;
        public event Action<Dpad> PressedDpad;
        public event Action Interact;
        public event Action Skip;
        public event Action Pause;

        private void Awake()
        {
            barrowInput = GetComponent<TardovInputController>();
            barrowInput.FinishedInput += SetUp;
        }
        void SetUp()
        {
            var _actions = barrowInput.Actions;
            //Set up Interactions
            var interaction = _actions.Player.Interact;
            interaction.performed += preformeInteract;

            //Set up Pause
            var pause = _actions.Player.Pause;
            pause.performed += preformePause;



            //Set Up Dpad
            var changeType = _actions.Player.ChangeType;
            changeType.performed += PreformedDpad;

            //Set Up Dpad
            var skip = _actions.Player.Jump;
            skip.performed += PreformedSkip;


            //FinishedSetup
            barrowInput.FinishedInput -= SetUp;
        }
        void preformeInteract(InputAction.CallbackContext button) => Interact?.Invoke();
        void preformePause(InputAction.CallbackContext button) => Pause?.Invoke();

        void PreformedDpad(InputAction.CallbackContext DpadButton)
        {
            var WhichButtonPressed = DpadButton.ReadValue<Vector2>();
            Dpad WhichDpad;
            if (WhichButtonPressed.x == 1)
                WhichDpad = Dpad.Right;
            else if (WhichButtonPressed.x == -1)
                WhichDpad = Dpad.Left;
            else if (WhichButtonPressed.y == 1)
                WhichDpad = Dpad.Up;
            else if (WhichButtonPressed.y == -1)
                WhichDpad = Dpad.Down;
            else
                return;
            PressedDpad?.Invoke(WhichDpad);

        }

        void PreformedSkip(InputAction.CallbackContext button) => Skip?.Invoke();

        // Start is called before the first frame update
    }
    public enum Dpad { Up, Down, Left, Right }
}
