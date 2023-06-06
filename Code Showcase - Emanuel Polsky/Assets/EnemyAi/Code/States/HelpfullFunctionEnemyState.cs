using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    public static class HelpfullFunctionEnemyState
    {
        public static Vector3 GettOffSetHide(this Vector3 hidingPosition, Vector3 enemyPosition)
        {
            var direction = (hidingPosition - enemyPosition).normalized;
            var _specificHidePlace = hidingPosition + direction * 1.5f;
            return _specificHidePlace;
        }
    }
}
