using GarmentButton.AI.Alert;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GarmentButton.AI.Alert
{
    public class NoticeMeter : MonoBehaviour
    {

        #region enum State
        private enum AlertFillAmoutRepresentation { Empty, Filling, Full };

        private AlertFillAmoutRepresentation _fillAmoutState;
        #endregion

        #region References Needed
        [Header("UI Variables References")]
        [SerializeField] private HandelAlert _alertScript;
        private Transform _targetWatching;

        #endregion

        #region UI Variables References
        [Header("UI Variables References")]
        [SerializeField] private Image _imageAlret;
        [SerializeField] private Transform _canvasTrans;
        [SerializeField] private CanvasGroup _canvasGroup;

        #endregion

        #region Speed Variables
        [Header("Speed Variables")]
        [SerializeField] private float _speedChangeImage;
        [SerializeField] private Ease _easeMovement = Ease.OutSine;
        #endregion


        #region On and Off Functions
        private void OnEnable()
        {
            _alertScript.AlertChanged += NoticePlayerMeter;
        }
        private void OnDisable()
        {
            _alertScript.AlertChanged -= NoticePlayerMeter;
        }

        #endregion
        private void NoticePlayerMeter()
        {
            var CurrentAlertNumber = _alertScript.CurrentAlert;

            bool isCurrentAlertAlmostEmpty = CurrentAlertNumber < 0.15f;
            bool isCurrentAlertIsFilling = CurrentAlertNumber > 0.15f && CurrentAlertNumber < 0.85f;
            bool isCurrentAlertAlmostFull = CurrentAlertNumber > 0.85f;


            _fillAmoutState =
                isCurrentAlertAlmostEmpty ? AlertFillAmoutRepresentation.Empty :
                isCurrentAlertIsFilling ? AlertFillAmoutRepresentation.Filling :
                isCurrentAlertAlmostFull ? AlertFillAmoutRepresentation.Full : 0;

            switch (_fillAmoutState)
            {
                case AlertFillAmoutRepresentation.Empty:
                    _imageAlret.fillAmount = 0f;
                    break;
                case AlertFillAmoutRepresentation.Filling:
                    _imageAlret.color = Color.yellow;
                    _imageAlret.DOFillAmount(CurrentAlertNumber, _speedChangeImage).SetEase(_easeMovement);
                    break;
                case AlertFillAmoutRepresentation.Full:
                    _imageAlret.color = Color.red;
                    _imageAlret.fillAmount = 1f;
                    break;
            }

        }
        private void LateUpdate()
        {

            CanvasRotate(_targetWatching);

        }
        private void CanvasRotate(Transform player)
        {
            if (player != null)
                return;
            var distance = Vector3.Distance(player.position, transform.position);
            float remainAlpha = 1 - distance / 15f;
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, remainAlpha, 0.5f);
            _canvasTrans.LookAt(_canvasTrans.position + player.rotation * Vector3.forward, player.rotation * Vector3.up);


        }
    }
}