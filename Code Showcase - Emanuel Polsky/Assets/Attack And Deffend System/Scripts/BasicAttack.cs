using System.Linq;
using UnityEngine;

namespace Attack
{


    public class BasicAttack: MonoBehaviour
    {
        #region Enum
        public enum AttackType { TowardsDirection, Around }
        #endregion

        #region Reference Variable
        [SerializeField] private Transform _transformCenterBody;
        [SerializeField] private AttackTypeData _attackTypeData;
        [SerializeField] private DirectionAttack _directionAttack;
        #endregion
        #region Private Variables
        [SerializeField] private AttackType _attackType;
        [SerializeField] private float _capsuleSize;
        [SerializeField] private float _boxCastDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private int _howManyFrameLastingCounter = 5;
        [SerializeField] private float offsetPosition = -1.5f;
        [SerializeField] private int _howMuchDamge = 1;
        private IHit _mineHitScriptAttack;
        #endregion

        #region Start
        private void Start()
        {
            _mineHitScriptAttack = GetComponent<IHit>();
            if (_mineHitScriptAttack == null)
                Debug.Log("Missing HitDetectionReffrence & The attack Wouldnt Work  " + "GameObject Name:   " + gameObject.name);
        }
        #endregion

        #region Public Functions
        public void Attack()
        {
            var diraction = _transformCenterBody.forward;
            if (_directionAttack != null)
                diraction = SelectDirection();

            Collider[] colliders = GetAllColliderInThePath(_attackType, diraction);

            if (colliders.Length > 0f)
                AttackAction(colliders);
        }
        #endregion
        #region Private Functions
        private void AttackAction(Collider[] hitted)
        {
            for (int i = 0; i < hitted.Length; i++)
            {
                var isTheSameobjectHit = GameObject.ReferenceEquals(gameObject, hitted[i].gameObject);
                if (!isTheSameobjectHit)
                {
                    var hitReciver = hitted[i].GetComponent<IHit>();
                    if (hitReciver != null)
                        hitReciver.Hit(_attackTypeData.GetElementType(), _howMuchDamge, _transformCenterBody.position, _howManyFrameLastingCounter, _mineHitScriptAttack);
                }

            }
        }
        private Collider[] GetAllColliderInThePath(AttackType attackType, Vector3 diraction)
        {
            switch (attackType)
            {
                case AttackType.TowardsDirection:
                    RaycastHit[] hitted = Physics.SphereCastAll(_transformCenterBody.position + (diraction * offsetPosition), _capsuleSize, diraction, _boxCastDistance, _layerHitable);
                    return hitted.Select(hit => hit.collider).ToArray();
                case AttackType.Around:
                    return Physics.OverlapSphere(_transformCenterBody.position, _capsuleSize, _layerHitable);
                    default: return null;
            }
        }
        private Vector3 SelectDirection()
        {
            if (_directionAttack != null)
            {
                switch (_directionAttack.direction)
                {
                    case DirectionAttack.Direction.Forawd:
                        return _transformCenterBody.forward;
                    case DirectionAttack.Direction.Up:
                        return _transformCenterBody.up;
                    case DirectionAttack.Direction.Down:
                        return -_transformCenterBody.up;
                    default: return _transformCenterBody.forward;
                }
            }
            else
            {
                return _transformCenterBody.forward;
            }
        }
        private void OnDrawGizmos()
        {
            var direction = _transformCenterBody.forward;
            if (Application.isPlaying)
                direction = SelectDirection();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_transformCenterBody.position + (direction * offsetPosition), _capsuleSize);
            Gizmos.DrawLine(_transformCenterBody.position + (direction * offsetPosition), _transformCenterBody.position + (direction * _boxCastDistance));
        }
        #endregion
    }
}

