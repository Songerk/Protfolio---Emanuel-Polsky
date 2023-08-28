using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class ResetPositions : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;
        private Vector3[] _positions;

        private void Awake()
        {
            _positions = new Vector3[_transforms.Length];
            for (int i = 0; i < _positions.Length; i++)
                _positions[i] = _transforms[i].position;
        }
        public void ResetPosition()
        {
            for (int i = 0; i < _transforms.Length; i++)
                _transforms[i].position = _positions[i];
        }
    }
}
