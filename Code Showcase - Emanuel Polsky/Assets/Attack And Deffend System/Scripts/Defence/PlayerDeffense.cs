using UnityEngine;

namespace Attack
{



    public class PlayerDeffense : MonoBehaviour, IDefense
    {
        #region reference variables
        [SerializeField] private ScriptableDefenseState _defenseState;
        #endregion
        #region private variables
        private DefenseType _currentDefencseType;
        [SerializeField] private bool _autoDefend = false;
        private int _frameLastActiveDefense = int.MinValue;
        private int _fixedFrame;
        #endregion

        private void FixedUpdate()
        {
            _fixedFrame++;
        }
        #region public Functions
        public DefenseType CheckDefenseState(AttackType type, int howManyFrameToCounterLasting)
        {
            switch (_currentDefencseType)
            {
                case DefenseType.Vulnerable:
                    if (_autoDefend)
                        _currentDefencseType = DefenseType.CounterAttack;
                    break;
                case DefenseType.Safe:
                    if (isHeSafe())
                        if (CheckCounter(howManyFrameToCounterLasting))
                            _currentDefencseType = DefenseType.CounterAttack;
                    break;
            }
            return _currentDefencseType;
        }
        public void StartDefenseState()
        {
            if (_fixedFrame > _frameLastActiveDefense + _defenseState.DefenseFrameCooldown)
            {
                _currentDefencseType = DefenseType.Safe;
                _frameLastActiveDefense = _fixedFrame;
            }

        }
        public void ResetDefense()
        {
            _currentDefencseType = DefenseType.Vulnerable;
        }
        #endregion
        #region private Functions

        private bool isHeSafe()
        {
            bool isAbleToCounter = _defenseState.AllowDefense;
            bool isRightTimingFor = _fixedFrame < _frameLastActiveDefense + _defenseState.DefenseFrameActive;
            if (isAbleToCounter && isRightTimingFor)
                return true;

            _currentDefencseType = DefenseType.Vulnerable;
            return false;
        }

        private bool CheckCounter(int howManyFrameToCounterLasting)
        {
            bool isAbleToCounter = _defenseState.AllowToCounterAttack;
            bool isRightTimingFor = _fixedFrame < _frameLastActiveDefense + howManyFrameToCounterLasting;
            if (isAbleToCounter && isRightTimingFor)
                return true;

            return false;
        }

        #endregion
    }
}
