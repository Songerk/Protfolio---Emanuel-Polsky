using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace LifeMechanics
{


    public class Life : MonoBehaviour
    {
        #region Variables
        #region Life
        [Header("Life")]
        [Tooltip("Is LIFE")]
        private int _life;
        [SerializeField] private LifeData _lifeData;
        [SerializeField] private bool _dontSave;
        private float _timerToHeal;
        [Tooltip("Where to call when Dead to change to state dead")]
        private bool _isDead;
        [Tooltip("if you want UI Update")]
        [SerializeField] private IUiLife _updateUiLifeScript;
        #endregion

        #region Hit
        [Space(50)]
        [Header("Hit")]
        [Tooltip("Fill only if there is affect On the shader")]
        [SerializeField] private MeshRenderer _mesh;
        private Material _matrielOfUser;
        [Tooltip("If there is affect on the the shader what it name for it to active")]
        [SerializeField] private string _hitShaderEffect = "Hit";
        [Tooltip("How much time the shader affect will take")]
        [SerializeField] private float _timeWithEffect = 1f;
        [Tooltip("The partical effect to play when hit")]
        [SerializeField] private ParticleSystem _hitEffect;
        public event Action<float, float, bool> LifeChanged;        //Current life, Max Life, Is Life bien Add
        public event Action DeathEvent;
        #endregion

        #region Other
        [Space(50)]
        [Tooltip("Check if things Missing")]
        [SerializeField] private bool _isMissing = true;
        [Tooltip("Meant For testing other things")]
        [SerializeField] private bool _isInTestingMod = false;
        #endregion
        #endregion

        private void Start()
        {
            if (_isInTestingMod)
                return;
            if (_dontSave)
                _lifeData = Instantiate(_lifeData);
            
            StartFunctions();

        }

        private void StartFunctions()
        {
            _life = _lifeData.MaxLife;


            if (_mesh)
                _matrielOfUser = _mesh.material;                        // Get the Matriels from the mesh

            if (_lifeData.IsSelfHeal)                                   // can he heal himself
                StartCoroutine(Heal());                                 // like Update but only if he can heal himself

            LifeChanged?.Invoke(_life, _lifeData.MaxLife, true);


            if (_isMissing)
                Debug.Log("Missing Reffrence for:  " + CheckIfGotEveryThingNeeded());


        }
        public void ResetLife()
        {
            _life = _lifeData.MaxLife;
            LifeChanged?.Invoke(_life, _lifeData.MaxLife, true);
            _isDead = false;
        }


        public void GetDamage(int DamageNumber)                         // function to get hit
        {
            if (_isInTestingMod || _isDead)                             // if he kep getting hit after he became dead so he will not do anything
                return;


            _life -= DamageNumber;

            HitEffect();

            LifeChanged?.Invoke(_life, _lifeData.MaxLife, false);

            if (_life <= 0)                                  // intionle check if the damge that he got is more than the life he got
            {
                BecomeDead();
            }

        }
        public void GetMoreLife(int number, bool FullHeal)
        {
            _lifeData.MaxLife += number;
            if (FullHeal)
                _life = _lifeData.MaxLife;
            else
                _life = Mathf.Min(_life + number, _lifeData.MaxLife);
            LifeChanged?.Invoke(_life, _lifeData.MaxLife, true);
        }

        private void HitEffect()
        {
            if (_hitEffect != null)
                _hitEffect.Play();                                          // play partical effect
            if (_mesh != null)                                          // Check if has mesh for shader effect
                StartCoroutine(ShaderEffect());
        }
        private IEnumerator ShaderEffect() // play hit effect on Shader
        {
            _matrielOfUser.SetInt(_hitShaderEffect, 1);
            yield return new WaitForSeconds(_timeWithEffect);
            _matrielOfUser.SetInt(_hitShaderEffect, 0);
        }
        private IEnumerator Heal()
        {
            _timerToHeal = 0f;
            while (true)
            {
                if (_life < _lifeData.MaxLife)
                {

                    _timerToHeal += Time.deltaTime;
                    if (_timerToHeal > _lifeData.WhenToHeal)
                    {
                        _timerToHeal = 0f;

                        _life += _lifeData.HowMuchToHeal;
                        if (_updateUiLifeScript != null)
                            _updateUiLifeScript.UpdateUi(_life, _lifeData.MaxLife, true);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
        private void BecomeDead()
        {
            StopCoroutine(Heal());                                    // stop all when dying
            _isDead = true;                                         // become Dead when hit is over his life
            DeathEvent?.Invoke();
        }
        private string CheckIfGotEveryThingNeeded()
        {

            if (_hitEffect)
            {
                return _hitEffect.ToString();
            }
            else if (_mesh == null)
            {
                return ("Mesh but only if you want effect");
            }
            else
            {
                return ("Everything is checked");
            }

        }
    }
}

//Copy Rights are reserved for Emanuel