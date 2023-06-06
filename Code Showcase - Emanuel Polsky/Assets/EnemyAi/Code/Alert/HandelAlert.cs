using Cysharp.Threading.Tasks;
using GarmentButton.AI.Abilities;
using GarmentButton.Player;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GarmentButton.AI.Alert
{
    public enum StateOfAlert { Idel, Alert, Fleeing, RunningToHideSpot, Hidding }       //Hiddin State Must Be Changed From outSide
    public class HandelAlert : MonoBehaviour
    {

        #region Reference Variables
        [Header("Reference Variables")]
        [Tooltip("Reference To Script with he will able to see")]
        [SerializeField] private FieldOfView _sight;

        [Tooltip("ScriptableObject Of the alert Stats")]
        [SerializeField] private AlertStats _alertStats;

        [Tooltip("From Where Is He Looking")]
        [SerializeField] private Transform _eyes;
        #endregion

        #region Alert Representation Variables

        public StateOfAlert AlertState { get; private set; }
        public float CurrentAlert { get; private set; }


        public event Action AlertChanged;

        #endregion


        #region Target Variables

        private PlayerData _targetData;
        private Transform _targetTransform;
        private float _distanceFromTarget;

        #endregion

        #region Private Variables

        [SerializeField] private float FrequncyOfLooking = 0.3f;
        //Variable for checking how much time has passed so he could start relaxing
        private float _timePassedSinceBeggingAlerted;

        private CancellationTokenSource _tokenSource;
        private CancellationToken _tokenToCancel;

        #endregion

        private void Awake()
        {
            _tokenSource = new CancellationTokenSource();
            _tokenToCancel = _tokenSource.Token;
        }

        private void OnEnable()
        {
            Viewing(_tokenToCancel).Forget();
        }
        private void OnDisable()
        {
            _tokenSource.Cancel();
        }

        #region Public Functions
        public async UniTaskVoid Viewing(CancellationToken token)
        {
            while (true)
            {

                await UniTask.Delay(TimeSpan.FromSeconds(FrequncyOfLooking), cancellationToken: token);

                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                var iSPlayerInSight = _sight.FindVisibleTargets(_eyes, out Transform Player);
                var isPlayerAround = iSPlayerInSight[0];
                var iSSeenPlayer = iSPlayerInSight[1];
                if (Player != null)
                {
                    if (_targetTransform == null)
                        SetTarget(Player);
                    else
                    {
                        var isTheSameTarget = GameObject.ReferenceEquals(_targetTransform.gameObject, Player.gameObject);
                        if (!isTheSameTarget)
                            SetTarget(Player);

                    }

                }

                if (iSSeenPlayer)
                    EnterFullAlert();
                else if (isPlayerAround)
                    PlayerIsAround();
                else
                    DetectPlayer();
            }


        }
        public void ForceState(StateOfAlert state)
        {
            AlertState = state;
            AlertChanged?.Invoke();
        }
        public Transform ReciveTarget()
        {
            return _targetTransform;
        }

        #endregion

        #region Private Functions
        private float CaculateDistanceFromPlayer(Transform target)
        {
            var distanceFromPlayer = Vector3.Distance(target.transform.position, transform.position);
            return distanceFromPlayer;
        }
        private void SetTarget(Transform targetCatch)
        {
            _targetTransform = targetCatch;
            _targetData = _targetTransform.GetComponent<PlayerData>();
        }

        private void CurrentAlertAction(float SpeedChange)
        {

            CurrentAlert += Time.deltaTime * (SpeedChange / 2f);
            CurrentAlert = Mathf.Clamp01(CurrentAlert);
            AlertChanged?.Invoke();

        }
        private void DetectPlayer()
        {
            bool isInCorrectState = AlertState != StateOfAlert.Idel;
            bool isPassedEnoghTimeToRelax = _timePassedSinceBeggingAlerted < _alertStats.HowMuchTimeItTakeToRelax;


            if (isInCorrectState && isPassedEnoghTimeToRelax)
            {
                _timePassedSinceBeggingAlerted += Time.deltaTime;
                return;
            }
            switch (AlertState)
            {
                case StateOfAlert.Fleeing:
                    ForceState(StateOfAlert.RunningToHideSpot);
                    break;
                case StateOfAlert.Hidding:
                    _timePassedSinceBeggingAlerted = 0;
                    ForceState(StateOfAlert.Alert);
                    break;
                case StateOfAlert.Alert:
                    _timePassedSinceBeggingAlerted = 0;
                    ForceState(StateOfAlert.Idel);
                    break;
                case StateOfAlert.Idel:
                    _timePassedSinceBeggingAlerted = 0;
                    CurrentAlertAction(-_alertStats.ChangeSpeedOfAlertRelaxing);
                    break;

            }

        }

        private void PlayerIsAround()
        {
            _distanceFromTarget = CaculateDistanceFromPlayer(_targetTransform);

            #region booleans for Target States
            bool isPlayerInTheCurrentRange = (_distanceFromTarget > _alertStats.DistanceToAlrtedAtOnec && _distanceFromTarget < _alertStats.RangeOfHearingTarget);
            bool isPlayerMakingNoise = !_targetData.IsCrouching && _targetData.CharacterTotalVelocty > _alertStats.MinimuVelocityToNotice;
            bool isPlayerNearbyEnemy = _distanceFromTarget < _alertStats.DistanceToAlrtedAtOnec;
            #endregion

            if (isPlayerInTheCurrentRange && isPlayerMakingNoise)
            {
                _timePassedSinceBeggingAlerted = 0;
                CurrentAlertAction(_alertStats.ChangeSpeedOfAlertAroundPlayer);
                CheckIfAlertMeterFull();
                if (AlertState != StateOfAlert.Fleeing)
                {
                    AlertState = StateOfAlert.Alert;
                }

            }
            else if (isPlayerNearbyEnemy && isPlayerMakingNoise)
            {
                CurrentAlertAction(_alertStats.ChangeSpeedOfAlertAroundPlayerVeryClose);
                CheckIfAlertMeterFull();
            }
            else
                DetectPlayer();

        }

        private void EnterFullAlert()
        {
            CurrentAlertAction(_alertStats.ChangeSpeedOfAlertWhenSeeingThePlayer);
            CheckIfAlertMeterFull();
            _timePassedSinceBeggingAlerted = 0;
        }
        private void CheckIfAlertMeterFull()
        {
            #region Booleans For States
            bool isInRestingState = AlertState == StateOfAlert.Hidding || AlertState == StateOfAlert.Idel;
            bool isRelaxing = isInRestingState && CurrentAlert > 0f;
            bool isHiddenAndRelaxed = CurrentAlert < 0.2f && AlertState == StateOfAlert.Hidding;
            bool isFullyAlerted = CurrentAlert > 0.85f && AlertState == StateOfAlert.Alert;
            #endregion


            if (isFullyAlerted)
                ForceState(StateOfAlert.Fleeing);
            if (isRelaxing)
            {
                ForceState(StateOfAlert.Alert);
                CurrentAlertAction(-_alertStats.ChangeSpeedOfAlertRelaxing);
                CheckIfAlertMeterFull();
            }
            else if (isHiddenAndRelaxed)
                ForceState(StateOfAlert.Idel);



        }
        #endregion

    }
}