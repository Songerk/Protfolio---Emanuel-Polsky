using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    #region Name For Asset
    public class DefendInterafces{}
    #endregion

    #region Interafaces

    public interface IHit
    {
        void Hit(AttackType type, int damagePoints, Vector3 froWhereGettingHit, int howManyFrameToCounterLasting, IHit attackerHitScript = null);

        void CounterAttack(int damagePoints, Vector3 froWhereGettingHit);
        void ConfirmedTheHit(Vector3 Direction);
    }
    public interface IDefense
    {
        DefenseType CheckDefenseState(AttackType type, int howManyFrameToCounterLasting = 0);
        void StartDefenseState();
        void ResetDefense();
    }
    #endregion
    public enum DefenseType { Vulnerable, HalfSafe , Safe, CounterAttack }

}
