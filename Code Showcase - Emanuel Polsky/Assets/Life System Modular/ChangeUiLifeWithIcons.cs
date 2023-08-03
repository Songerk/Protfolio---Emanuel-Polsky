using GarmentButton.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace LifeMechanics
{


    public class ChangeUiLifeWithIcons : MonoBehaviour
    {
        #region Reference Variable
        [SerializeField] private Life _lifeScript;
        [Tooltip("Life images (Order is matters)/ if it a meter than fill only one image")]
        [SerializeField] private GameObject _lifeContainer;
        [SerializeField] private GameObject _lifePreFab;
        [SerializeField] private List<LifeIcon> _lifeImages;
        [SerializeField] private AudioClip soundForMinusLife;
        #endregion

        #region Private Variables
        [SerializeField] private bool _removeIcon = false;
        [Tooltip("The speed of the fade out&in ui life")]
        [SerializeField] private float _speedChangeUI;
        private int _currentMaxLife;

        //Pooling
        private ObjectPool<PoolObject> objectPool;

        #endregion

        #region Subscription 
        private void OnEnable()
        {
            _lifeScript.LifeChanged += UpdateUi;
            objectPool = new ObjectPool<PoolObject>(_lifePreFab);
        }
        private void OnDisable()
        {
            _lifeScript.LifeChanged -= UpdateUi;
        }
        #endregion
        #region Public Functions
        public void UpdateUi(float currentLife, float MaxLife, bool addLife)
        {
            if (MaxLife > _currentMaxLife)
            {
                var HowMuchBeinHit = MaxLife - currentLife;
                SetAllChildren((int)MaxLife, (int)HowMuchBeinHit);
                _currentMaxLife = (int)MaxLife;
                return;
            }
            if (addLife)
            {
                for (int i = 0; i < currentLife; i++)
                {
                    if (_lifeImages[i].GetState() == LifeIconState.Empty)
                    {
                        if (_removeIcon)
                        {
                            objectPool.Pull();
                            Debug.Log(_lifeImages[i].gameObject.name);
                        }
                        _lifeImages[i].ChangeToFull();
                    }

                }
            }
            else
            {
                for (int i = _lifeImages.Count - 1; i > -1; i--)
                    if (_lifeImages[i].GetState() == LifeIconState.Full)
                    {
                        if (soundForMinusLife != null)
                            AudioSource.PlayClipAtPoint(soundForMinusLife, transform.position);
                        _lifeImages[i].ChangeToEmpty();
                        if (_removeIcon)
                        {
                            var pool = _lifeImages[i].GetComponent<PoolObject>();
                            pool.ReturnToPool();
                            
                        }
                        return;
                    }
            }
        }
        #endregion
        #region Private Functions
        private void SetAllChildren(int MaxLife, int beinHit)
        {
            if (_lifeContainer == null)
                FindLifeContainer();
            if (MaxLife > _lifeImages.Count)
            {
                var HowMuchToAdd = MaxLife - _lifeImages.Count;
                for (int i = 0; i < HowMuchToAdd; i++)
                {
                    var imageObject = objectPool.Pull(_lifeContainer.transform);
                    _lifeImages.Add(imageObject.GetComponent<LifeIcon>());
                }
                for (int i = 0; i < _lifeImages.Count; i++)
                {
                    _lifeImages[i].ChangeToFull();
                }
                if (_removeIcon) return;
                for (int i = _lifeImages.Count - 1; i > 0; i--)
                {
                    if (beinHit > 0)
                    {
                        _lifeImages[i].ChangeToEmpty();
                        beinHit--;

                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        private void FindLifeContainer()
        {
            _lifeContainer = GameObject.FindGameObjectWithTag("LifeContainer");
        }

        #endregion
    }
}

