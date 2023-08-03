using UnityEngine;

namespace LifeMechanics
{
    [CreateAssetMenu(menuName = "Life Data")]
    public class LifeData : ScriptableObject
    {
        public int MaxLife;
        [Tooltip("Do you want him to heal by him self?")]
        public bool IsSelfHeal;
        [Tooltip("How much time to wait for self heal")]
        public float WhenToHeal = 60f;
        [Tooltip("How much to heal when he heals")]
        public int HowMuchToHeal = 1;
    }
}
