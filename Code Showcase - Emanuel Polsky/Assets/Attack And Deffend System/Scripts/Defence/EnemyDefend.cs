using UnityEngine;

namespace Attack
{

    public class EnemyDefend : MonoBehaviour, IDefense
    {
        #region reference Variables
        [SerializeField] private ScriptableDefenseState DefenseState;
        [SerializeField] private AttackTypeData _currenctType;
        #endregion
        #region private Variables
        [SerializeField] private bool _debug;
        #endregion

        #region Public Functions
        public DefenseType CheckDefenseState(AttackType type, int howManyFrameToCounterLasting)
        {
            switch (_currenctType.CheckIfResistance(type))
            {
                case DamgeType.weak:
                    if(_debug) Debug.Log("WeakAttack");
                    return DefenseType.Safe;
                case DamgeType.Modern:
                    if (_debug) Debug.Log("ModernAttack");
                    return DefenseType.HalfSafe;
                case DamgeType.Strong:
                    if (_debug) Debug.Log("StrongAttack");
                    return DefenseType.Vulnerable;
                default: return DefenseType.HalfSafe;
            }
        }
        public void StartDefenseState(){}
        public void ResetDefense(){}
        #endregion

    }
}
