using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GarmentButton.AI.Abilities
{
    [CreateAssetMenu(menuName = "EnemyAi/Detection")]
    public class CheckIfSomeoneIsHiddingInPosition : ScriptableObject
    {
        [SerializeField] private float _radiusSphere = 3.57f;
        [SerializeField] private LayerMask _enemyLayer;
        public bool IsThereSomeOneElseHidding(Vector3 positionOfHidding, Collider collider)
        {
            var hits = Physics.OverlapSphere(positionOfHidding, _radiusSphere, _enemyLayer, QueryTriggerInteraction.Ignore);
            if (hits.Length > 0)
                for (int i = 0; i < hits.Length; i++)
                    if (hits[i] != collider)
                        return true;


            return false;
        }

#if UNITY_EDITOR
        public void DrawGizmo(Transform Hideposition)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Hideposition.position, _radiusSphere);
        }
#endif
    }
}
