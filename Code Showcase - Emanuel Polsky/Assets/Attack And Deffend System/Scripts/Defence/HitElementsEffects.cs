using UnityEngine;

namespace Attack
{
    public class HitElementsEffects : MonoBehaviour
    {
        #region Reference Variable 
        [SerializeField] private HitDetection _hitDetectionScript;
        [Header("Defend")]
        [SerializeField] private ParticleSystem _particalsForDefend;
        [SerializeField] private AudioClip _clipForDefend;

        [Header("Counter")]
        [SerializeField] private ParticleSystem _particalsForCounter;
        [SerializeField] private AudioClip _clipForCounter;

        [Header("GotHalfHit")]
        [SerializeField] private ParticleSystem _particalsForHalfHit;
        [SerializeField] private AudioClip _clipForHalfHit;

        [Header("GotFullHit")]
        [SerializeField] private ParticleSystem _particalsForFullHit;
        [SerializeField] private AudioClip _clipForFullHit;
        #endregion
        #region Subscription

        private void OnEnable()
        {
            _hitDetectionScript.Defended += DefendEffect;
            _hitDetectionScript.CounteredTheAttack += CounterEffect;
            _hitDetectionScript.GotHalfHit += HalfHitEffect;
            _hitDetectionScript.GotHit += FullEffect;

        }
        private void OnDisable()
        {
            _hitDetectionScript.Defended -= DefendEffect;
            _hitDetectionScript.CounteredTheAttack -= CounterEffect;
            _hitDetectionScript.GotHalfHit -= HalfHitEffect;
            _hitDetectionScript.GotHit -= FullEffect;
        }
        #endregion

        #region Effects Functions
        private void DefendEffect() => PlayEffect(_particalsForDefend, _clipForDefend);
        private void CounterEffect() => PlayEffect(_particalsForCounter, _clipForCounter);
        private void HalfHitEffect(Vector3 Null) => PlayEffect(_particalsForHalfHit, _clipForHalfHit);
        private void FullEffect(Vector3 Null) => PlayEffect(_particalsForFullHit, _clipForFullHit);
        private void PlayEffect(ParticleSystem partical, AudioClip clip)
        {
            if (partical != null)
                partical.Play();
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        #endregion
    }
}
