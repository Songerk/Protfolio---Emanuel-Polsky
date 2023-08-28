using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Attack
{
    public class SlowTimeOnGetHit : MonoBehaviour
    {
        [SerializeField] private HitDetection _hitDetectionScript;
        [SerializeField] private float _targetTimeSpeed;
        [SerializeField] private float _durationOfSlowTime = 0.2f;
        [SerializeField] private float _durationOfReturning = 1;
        private Sequence _currentSequnce;
        private void OnEnable()
        {
            _hitDetectionScript.GotHit += SlowTimeEffect;
        }

        private void SlowTimeEffect(Vector3 vector)
        {
            CheckIfActiveTween();

            _currentSequnce = DOTween.Sequence();
            _currentSequnce.Pause();
            _currentSequnce.AppendCallback(() => { Time.timeScale = _targetTimeSpeed; });
            _currentSequnce.AppendInterval(_durationOfSlowTime).SetUpdate(true);
            _currentSequnce.Append((DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, _durationOfReturning)).SetUpdate(true));
            _currentSequnce.Play();


        }

        private void CheckIfActiveTween()
        {
            if (!_currentSequnce.IsActive()) return;
            _currentSequnce.Kill();
            Time.timeScale = 1f;

        }
    }
}
