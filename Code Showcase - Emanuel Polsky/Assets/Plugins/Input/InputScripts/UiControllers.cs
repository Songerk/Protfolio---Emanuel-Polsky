using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UiControllers : MonoBehaviour
{
    [SerializeField] private GameObject[] _keyboards;
    [SerializeField] private GameObject[] _gamepad;
    bool _isGamePad;
 
    private void OnEnable()
    {
        if (Gamepad.all.Count > 0)
            _isGamePad = true;
        else
            _isGamePad = false;
        GamepadControl();
    }
    private void GamepadControl()
    {
        if (_gamepad.Length == 0) return;

        for (int i = 0; i < _gamepad.Length; i++)
            _gamepad[i].SetActive(_isGamePad? true:false);

        for (int i = 0; i < _keyboards.Length; i++)
            _keyboards[i].SetActive( _isGamePad ?false : true);
    }
    [ContextMenu("Find All")]
    private void FindAllGamePad()
    {
        var allComponets = GetComponentsInChildren<Transform>(true);
        List<GameObject> gamepadObjects = new List<GameObject>();
        List<GameObject> KeyboardObjects = new List<GameObject>();
        for (int i = 0; i < allComponets.Length; i++)
        {
            if (allComponets[i].CompareTag("Keyboard"))
                KeyboardObjects.Add(allComponets[i].gameObject);
            else if (allComponets[i].CompareTag("Gamepad"))
                gamepadObjects.Add(allComponets[i].gameObject);
        }
        _keyboards = KeyboardObjects.ToArray();
        _gamepad = gamepadObjects.ToArray();

    }
}
