using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class ReactionAttack : MonoBehaviour
    {
        #region reference variables
        [SerializeField] private HitDetection _hitDeteaction;
        [SerializeField] private Rigidbody _rigidbody;
        #endregion

        #region private Variables
        [SerializeField] private float _force;
        private Vector3 _originPosition;
        #endregion

        #region On And Off Subscribtion
        private void OnEnable()
        {
            _originPosition = _rigidbody.transform.position;

            _hitDeteaction.GotHit += HitAction;
            _hitDeteaction.GotHalfHit += HitHalfAction;
        }
        private void OnDisable()
        {
            _hitDeteaction.GotHit -= HitAction;
            _hitDeteaction.GotHalfHit -= HitHalfAction;
        }
        #endregion

        #region private functions
        private void HitHalfAction(Vector3 vector)
        {
            AddForce(vector, _force / 2);
        }

        private void HitAction(Vector3 vector)
        {
            AddForce(vector, _force);
        }

        void AddForce(Vector3 direction ,float force)
        {
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }

        IEnumerator ReturnToPlace()
        {
            var waitTime = new WaitForSeconds(3);
            yield return waitTime;
            _rigidbody.transform.position = _originPosition;
        }
        #endregion
    }
}
