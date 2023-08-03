using LifeMechanics;
using System;
using UnityEngine;

namespace Attack
{


    public class HitDetection : MonoBehaviour, IHit
    {
        #region Reference Variables
        [SerializeField] private Life _lifeScript;
        [SerializeField] private Transform CeneterBodyMass;
        #endregion

        #region Private Variables
        [SerializeField] private bool _invinsiable = false;
        [SerializeField] private bool _debug = false;
        [SerializeField] private float _timeBetweenHits = 0.5f;
        private float _time;
        private IDefense _defenceState;
        #endregion

        #region Public events
        public event Action<Vector3> ConfirmedHitEvent;
        public event Action<Vector3> GotHit;
        public event Action<Vector3> GotHalfHit;
        public event Action Defended;
        public event Action CounteredTheAttack;
        #endregion


        #region Private Function
        private void Start()
        {
            _defenceState = GetComponent<IDefense>();
            if (_lifeScript == null)
                _lifeScript = GetComponent<Life>();

            _time = Time.time;
        }
        private void GetHit(int damagePoints, Vector3 direction, bool fullHit)
        {
            _time = Time.time;

            if (_lifeScript != null)
                _lifeScript.GetDamage(damagePoints);

            if (fullHit)
                GotHit?.Invoke(direction);
            else
                GotHalfHit?.Invoke(direction);
        }
        #endregion

        #region Public Function
        public void Hit(AttackType type, int damagePoints, Vector3 FroWhereGettingHit, int howManyFrameToCounterLasting, IHit attackerHitScript = null)
        {
            if (_debug) Debug.Log("Recived Hit");
            if (_invinsiable || Time.time < _time + _timeBetweenHits) return;

            var dir = (CeneterBodyMass.position - FroWhereGettingHit).normalized;
            Debug.Log(dir);
            if (_defenceState == null)
            {
                GetHit(damagePoints, dir, true);
                attackerHitScript?.ConfirmedTheHit(dir);
                _time = Time.time;
                return;
            }

            switch (_defenceState.CheckDefenseState(type, howManyFrameToCounterLasting))
            {
                case DefenseType.Vulnerable:
                    GetHit(damagePoints, dir, true);
                    attackerHitScript?.ConfirmedTheHit(dir);
                    break;

                case DefenseType.HalfSafe:
                    GetHit(damagePoints, dir, false);
                    attackerHitScript?.ConfirmedTheHit(dir);
                    break;

                case DefenseType.Safe:
                    attackerHitScript?.ConfirmedTheHit(dir);
                    Defended?.Invoke();
                    break;

                case DefenseType.CounterAttack:
                    attackerHitScript.CounterAttack(Mathf.RoundToInt(damagePoints * 1.5f), CeneterBodyMass.position);
                    CounteredTheAttack?.Invoke();
                    break;
            }
        }
        public void InvinsiableState(bool state = true)
        {
            _invinsiable = state;
        }
        #endregion


        #region Function Activated By Other IHit
        public void ConfirmedTheHit(Vector3 direction)
        {
            ConfirmedHitEvent?.Invoke(-direction * 2);
        }

        public void CounterAttack(int damagePoints, Vector3 froWhereGettingHit)
        {
            GetHit(damagePoints, froWhereGettingHit, true);
        }
        #endregion 
    }
}
