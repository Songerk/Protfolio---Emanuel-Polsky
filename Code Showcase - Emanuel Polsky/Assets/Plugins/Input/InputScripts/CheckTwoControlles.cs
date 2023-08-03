using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.InputControl
{
    public class CheckTwoControlles : MonoBehaviour
    {
        [SerializeField] private GameObject _active;
        [SerializeField] private GameObject _Disable;
        [SerializeField] private GameObject _cantEnter;
        bool IsConnectedMore()
        {
            string[] joystickNames = Input.GetJoystickNames();
            // Count how many gamepads are connected
            int connectedGamepads = 0;
            foreach (string joystickName in joystickNames)
            {
                if (!string.IsNullOrEmpty(joystickName))
                {
                    connectedGamepads++;
                }
            }
            return connectedGamepads >= 2;
        }
        public void CheckIfTwo()
        {
            if (IsConnectedMore())
            {
                _active.SetActive(true);
                _Disable.SetActive(false);
            }
            else
            {
                StopCoroutine(ShowCantEndter());
                StartCoroutine(ShowCantEndter());
            }
        }
        IEnumerator ShowCantEndter()
        {
            _cantEnter.SetActive(true);
            yield return new WaitForSeconds(2);
            _cantEnter.SetActive(false);
        }
    }
}
