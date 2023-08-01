using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace GarmentButton.AI.Abilities
{
    [CreateAssetMenu(menuName = "EnemyAi/Alert/Field Of View")]
    public class FieldOfView : ScriptableObject
    {
        #region View Veriables
        [Tooltip("How Far He Will Sense")]
        [SerializeField] private float _viewRadius = 21f;
        [Tooltip("How Wide He Will See")]
        [Range(0, 360)]
        [SerializeField] private float _viewAngle = 100f;
        #endregion

        #region Layer Variables
        [Tooltip("On what he should focus and count as Target")]
        [SerializeField] private LayerMask _targetMask;
        [Tooltip("What will Block Him for seeing")]
        [SerializeField] private LayerMask _obstacleMask;
        #endregion


        public bool[] FindVisibleTargets(Transform fromWhere, out Transform Player)
        {
            var iSPlayerInSight = new bool[2] { false, false };
            Collider[] targetsInViewRadius = Physics.OverlapSphere(fromWhere.position, _viewRadius, _targetMask, QueryTriggerInteraction.Ignore);
            Player = null;
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                iSPlayerInSight[0] = true;
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - fromWhere.position).normalized;
                if (Vector3.Angle(fromWhere.forward, dirToTarget) < _viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(fromWhere.position, target.position);
                    if (!Physics.Raycast(fromWhere.position, dirToTarget, dstToTarget, _obstacleMask, QueryTriggerInteraction.Ignore))
                        iSPlayerInSight[1] = true;


                }
                Player = targetsInViewRadius[0].transform;
            }
            return iSPlayerInSight;
        }
#if UNITY_EDITOR
        public void DrawGizmosForSight(Transform transformFrom)
        {
            Handles.color = Color.yellow;

            Handles.DrawSolidArc(transformFrom.position, Vector3.up, transformFrom.forward , _viewAngle / 2, _viewRadius);
            Handles.DrawSolidArc(transformFrom.position, Vector3.up, transformFrom.forward, -_viewAngle /2, _viewRadius);


        }
        public void DrawGizmosForHearing(Vector3 position, float radius, Color color)
        {
            Handles.color = color;
            Handles.DrawSolidArc(position, Vector3.up, Vector3.forward, 360, radius);
            //Gizmos.DrawWireSphere(position, radius);
        }
        public void DrawLineToTarget(Transform eyes, Transform target)
        {
            Handles.color = Color.red;
            Handles.DrawLine(eyes.position, target.position);
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, Transform eyes)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += eyes.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
#endif
    }
}