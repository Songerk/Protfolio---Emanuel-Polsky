using System;
using UnityEngine;


namespace Attack
{


    [CreateAssetMenu(menuName = "Elements/Attack Type", fileName = "Attack Type Data")]
    public class AttackTypeData : ScriptableObject
    {
        #region Variables
        [SerializeField] private AttackType CurrentType;
        public event Action<AttackType> ChangeTypeEvent;
        public AttackType[] AllowToUse;
        #endregion

        #region Public Functions
        public void ChangeType()
        {
            var arrayElements = ShuffleArray(AllowToUse);
            for (int i = 0; i < arrayElements.Length; i++)
            {
                if (arrayElements[i] != CurrentType)
                {
                    CurrentType = arrayElements[i];
                    return;
                }
                    
            }
        }
        public void ChangeType(AttackType type)
        {
            for (int i = 0; i < AllowToUse.Length; i++)
            {
                if (AllowToUse[i] == type)
                {
                    CurrentType = type;
                    return;
                }

            }
            ChangeTypeEvent?.Invoke(type);
        }
        public AttackType GetElementType()
        {
            return CurrentType;
        }
        #endregion

        #region Private Functions Of Check Resistence
        public DamgeType CheckIfResistance(AttackType giverType)
        {
            switch (CurrentType)
            {
                case AttackType.Water:
                    return WaterResistence(giverType);
                case AttackType.Fire:
                    return FireResistence(giverType);
                case AttackType.Ice:
                    return IceResistence(giverType);
                default: return DamgeType.Modern;
            }
        }
        private DamgeType WaterResistence(AttackType giverType)
        {
            switch (giverType)
            {
                case AttackType.Water:
                    return DamgeType.Modern;
                case AttackType.Fire:
                    return DamgeType.weak;
                case AttackType.Ice:
                    return DamgeType.Modern;
                default: return DamgeType.Modern;


            }
        }
        private DamgeType FireResistence(AttackType giverType)
        {
            switch (giverType)
            {
                case AttackType.Water:
                    return DamgeType.Strong;
                case AttackType.Fire:
                    return DamgeType.Modern;
                case AttackType.Ice:
                    return DamgeType.weak;
                default: return DamgeType.Modern;


            }
        }
        private DamgeType IceResistence(AttackType giverType)
        {
            switch (giverType)
            {
                case AttackType.Water:
                    return DamgeType.weak;
                case AttackType.Fire:
                    return DamgeType.Strong;
                case AttackType.Ice:
                    return DamgeType.Modern;
                default: return DamgeType.Modern;


            }
        }

        #endregion

        #region Generic Function
        private T[] ShuffleArray<T>(T[] array)
        {
            System.Random random = new System.Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i + 1);
                T temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }

            return array;
        }

        #endregion
    }
    #region Enums
    public enum AttackType { Water, Fire, Ice }
    public enum DamgeType { weak, Modern, Strong }
    #endregion
}