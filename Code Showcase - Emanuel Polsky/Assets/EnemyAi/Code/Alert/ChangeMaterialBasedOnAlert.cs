using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.AI.Alert
{
    public class ChangeMaterialBasedOnAlert : MonoBehaviour
    {
        [SerializeField] private HandelAlert _alet;
        [SerializeField] private MeshRenderer _mesh;
        private Material _material;
        [SerializeField] private Color _colorIdel;
        [SerializeField] private Color _colorAlert;
        [SerializeField] private Color _colorFleeing;
        [SerializeField] private Color _colorHiding;
        [SerializeField] private Color _colorRunningToHiding;
        private void OnEnable()
        {
            _alet.AlertChanged += changeMaterialLook;
            _material = _mesh.material;
        }
        private void OnDisable()
        {
            _alet.AlertChanged -= changeMaterialLook;
        }
        void changeMaterialLook()
        {
            switch (_alet.AlertState)
            {
                case StateOfAlert.Idel:
                    _material.color = _colorIdel;
                    break;
                case StateOfAlert.Alert:
                    _material.color = _colorAlert;
                    break;
                case StateOfAlert.Fleeing:
                    _material.color = _colorFleeing;
                    break;
                case StateOfAlert.RunningToHideSpot:
                    _material.color = _colorRunningToHiding;
                    break;
                case StateOfAlert.Hidding:
                    _material.color = _colorHiding;
                    break;
            }
        }
    }
}