using System.Collections;
using UnityEngine;

namespace GarmentButton.AI.Alert
{
    [CreateAssetMenu(menuName = "EnemyAi/Alert/Alert Total Data", fileName = "Alert Total Data")]
    public class AlertStats : ScriptableObject
    {
        #region Speed Change Variables

        [Header("Speed Change Alert")]
        [Tooltip("How Fast He Will Hear The Player Around")]
        [Range(0, 10)]
        public float ChangeSpeedOfAlertAroundPlayer = 0.67f;

        [Tooltip("How Fast He Will Hear The Player Close To Him")]
        [Range(0, 10)]
        public float ChangeSpeedOfAlertAroundPlayerVeryClose = 7.84f;

        [Tooltip("How Fast He Will See The Player ")]
        [Range(0, 30)]
        public float ChangeSpeedOfAlertWhenSeeingThePlayer = 25.61f;

        [Tooltip("How Fast He Will Relax")]
        [Range(0, 10)]
        public float ChangeSpeedOfAlertRelaxing = 2f;

        [Tooltip("Minmun Speed Target Will Need To Move To Be Heard")]
        public float MinimuVelocityToNotice;

        [Tooltip("The Range Between To Hear The Target")]
        public float RangeOfHearingTarget = 5f;

        [Tooltip("The Range He Will Notice Target Quickly")]
        public float DistanceToAlrtedAtOnec = 2f;

        [Tooltip("How Fast The Enemy Will Relayx")]
        public float HowMuchTimeItTakeToRelax;
        #endregion

    }
}