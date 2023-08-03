using UnityEngine;

namespace Attack
{


    public class EnemyMangeAttackType : MonoBehaviour
    {
        #region Reference Variables
        [SerializeField] private AttackTypeData _attackTypeData;
        [SerializeField] private HitDetection _hitDetection;

        [SerializeField] private ElementMaterials _elementMaterials;

        [SerializeField] private ParticleSystemRenderer _partical;
        [SerializeField] private ParticleSystemRenderer _particalSystemToShowChangeRender;
        [SerializeField] private ParticleSystem _particalSystemToShowChange;
        #endregion

        #region Private Variables
        [SerializeField] private int _precentegeToHappend = 50;

        [SerializeField] private bool _showChangeElement;

        [SerializeField] private bool _debug;
        #endregion

        #region On And Off Subscription
        private void OnEnable()
        {
            _hitDetection.GotHit += ChangeType;
            _hitDetection.GotHalfHit += ChangeType;
            ParticalChange(_attackTypeData.GetElementType());
        }
        private void OnDisable()
        {
            _hitDetection.GotHit -= ChangeType;
            _hitDetection.GotHalfHit -= ChangeType;
        }
        #endregion

        #region Private Functions
        private void ChangeType(Vector3 positione)
        {
            if (_debug) Debug.Log("Thinks if chaning type");
            if (_debug) Debug.Log($"_attackTypeData.AllowToUse.Length {_attackTypeData.AllowToUse.Length} ");


            if (_attackTypeData.AllowToUse.Length == 0)
                return;
            var willChangeElement = HasChaneToHappend(_precentegeToHappend);
            if (_debug) Debug.Log($"will it happend {willChangeElement}");
            if (!willChangeElement)
                return;

            ForceChangeType();

        }
        private void ForceChangeType()
        {
            var type = _attackTypeData.GetElementType();
            _attackTypeData.ChangeType();
            var newType = _attackTypeData.GetElementType();
            if (type != newType)
                ParticalChange(newType);
        }

        private void ParticalChange(AttackType type)
        {
            Material mat = _partical.material;
            switch (type)
            {
                case AttackType.Water:
                    var changeMat = Instantiate(_elementMaterials.MaterialWater);
                    _partical.material = changeMat;
                    mat = changeMat;
                    break;
                case AttackType.Ice:
                    var changeMatIce = Instantiate(_elementMaterials.MaterialIce);
                    _partical.material = changeMatIce;
                    mat = changeMatIce;
                    break;
            }
            if (_showChangeElement)
            {
                _particalSystemToShowChangeRender.material = mat;
                _particalSystemToShowChange.Play();
            }
        }
        private bool HasChaneToHappend(int percentageChance)
        {
            int randomNumber = Random.Range(0, 101);

            if (randomNumber < percentageChance)
                return true;
            return false;
        }

        #endregion

    }
}
