using Cysharp.Threading.Tasks;
using GarmentButton.AI.Abilities;
using GarmentButton.AI.Alert;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GarmentButton.AI.StateMechine
{
    public class EscapeToHideSpotDuckState : State
    {
        [Tooltip("For Testing")]
        [SerializeField] private bool _testingMod;

        #region Required Reffrences

        [Tooltip("The Genral Enemy Data")]
        [SerializeField] private EnemyDuckStats _stats;
        [SerializeField] private Collider _collider;
        [SerializeField] private CheckIfSomeoneIsHiddingInPosition _checkHidddingScript;



        #endregion

        #region Destinations Variables
        private Transform _currentHidePlace;
        private Vector3 _coordinatesHidePlace;
        private Vector3 _destinationTarget;
        private bool _isReachHiddingPlace;
        #endregion


        #region Token Variables
        private CancellationTokenSource _tokenSource;
        private CancellationToken _tokenToCancel;

        #endregion

        private void OnDisable()
        {
            _tokenSource?.Cancel();
        }

        #region Finite State Machine Function
        public override void OnEnterState()
        {
            if (MyFSM.AnimatorBody != null)
                MyFSM.AnimatorBody.SetFloat("MoveSpeed", _stats.SpeedCharacter);

            MyFSM.Agent.speed = _stats.SpeedCharacter * 6f;
            MyFSM.Agent.acceleration = _stats.SpeedCharacter * 3f;
            CreateNewTokenCancelation();

            EscapeSequnce(_tokenToCancel).Forget();




        }
        public override void OnExitState()
        {
            _tokenSource?.Cancel();
        }
        public override void UpdateState()
        {

        }
        protected override void checkTransition()
        {
            if (!MyFSM.IsBeingDraged)
                return;
            MyFSM.SwitchState(stateHandles.Dragged);
        }
        #endregion

        #region Escape Sequnce
        public async UniTaskVoid EscapeSequnce(CancellationToken token)
        {

                await GotSpooked(token);
                while (!_isReachHiddingPlace)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                    while (MyFSM.Alert.AlertState == StateOfAlert.Fleeing)
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();
                        if (!await RunningAwayFromPlayer(token))
                            await RunToBackUpSpotIfReachedDeadEnd(token);
                    }
                    Debug.Log("Finish Running");
                    await RunToHideSpot(token);

                    MyFSM.CurrentHidePosition = _currentHidePlace;
                    MyFSM.SwitchState(stateHandles.HiddenAndRunningBackToShip);
                }
        }

        async UniTask GotSpooked(CancellationToken token)
        {
            _isReachHiddingPlace = false;

            MyFSM.Agent.SetDestination(transform.position);

            await UniTask.Delay(TimeSpan.FromSeconds(_stats.HowMuchTimeItTakesToRun), cancellationToken: token);

            transform.LookAt(MyFSM.PlayerTransform, transform.up);

            if (MyFSM.AnimatorBody != null)
            {
                MyFSM.AnimatorBody.SetTrigger("Suprise");
                MyFSM.AnimatorDuckSmallBody.SetTrigger("Suprise");
            }

            MyFSM.Agent.velocity = Vector2.zero;

            await UniTask.Delay(TimeSpan.FromSeconds(_stats.HowMuchTimeItTakesToSpook), cancellationToken: token); // Wait for animation to finish

        }
        async UniTask<bool> RunningAwayFromPlayer(CancellationToken token)
        {
            while (MyFSM.Alert.AlertState == StateOfAlert.Fleeing)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                Debug.Log("is Running Away For Player");
                var isThereWhereToRun = IsThereWhereToRunFromPlayer();

                if (!isThereWhereToRun)
                    return false;

                MoveZigZag();

                await UniTask.Yield();
            }
            return true;
        }
        async UniTask RunToBackUpSpotIfReachedDeadEnd(CancellationToken token) // run to chosen position if he reach limit world
        {
            Debug.Log("Started Running To BackUp Spot");
            var numberPostion = Random.Range(0, _stats.BackUpPosition.Length);

            var backUpPosition = _stats.BackUpPosition[numberPostion];

            RunToPosition(backUpPosition, false);

            while (!CheckIfReached())
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                Debug.Log("Check If Running To BackUpSpot");
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            }


            SetClosesHideSpot();
        }
        async UniTask RunToHideSpot(CancellationToken token)
        {
            SetClosesHideSpot();

            while (!CheckIfReached())
            {
                if (MyFSM.Alert.AlertState == StateOfAlert.Fleeing)
                    return;
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            }

            if (MyFSM.AnimatorBody != null)
            {
                if (_currentHidePlace.GetChild(0).CompareTag("Rock"))
                    MyFSM.AnimatorBody.SetTrigger("Down");
                else
                    MyFSM.AnimatorBody.ResetTrigger("Down");
            }


            MyFSM.Alert.ForceState(StateOfAlert.Hidding);
            _isReachHiddingPlace = true;
        }

        #endregion

        #region Calculations
        void ChooseRandomHidePlace()
        {
            var numberPostion = Random.Range(0, _stats.HidePositions.Length);
            _currentHidePlace = _stats.HidePositions[numberPostion];
        }

        void RunToPosition(Vector3 runTo, bool runToHideSpot)
        {

            MyFSM.AgentPath = new NavMeshPath();
            MyFSM.Agent.CalculatePath(runTo, MyFSM.AgentPath);

            if (MyFSM.AgentPath.status == NavMeshPathStatus.PathPartial)
                MyFSM.Agent.SetDestination(transform.right * 4f);

            else
                MyFSM.Agent.SetDestination(runTo);

        }
        void RoutineFindNewHideSpot()
        {
            ChooseRandomHidePlace();
            CheckIsSomeOneIsHiddingThere(ChooseRandomHidePlace);

            _coordinatesHidePlace = _currentHidePlace.position.GettOffSetHide(MyFSM.PlayerTransform.position);
            RunToPosition(_coordinatesHidePlace, true);
        }
        void CheckIsSomeOneIsHiddingThere(Action findNewHideSpot)
        {
            int numberOfAttempts = 10;
            bool isSomeElseHiddingThere = _checkHidddingScript.IsThereSomeOneElseHidding(_currentHidePlace.position, _collider);
            while (numberOfAttempts > 0 && isSomeElseHiddingThere)
            {
                numberOfAttempts--;
                findNewHideSpot?.Invoke();
                isSomeElseHiddingThere = _checkHidddingScript.IsThereSomeOneElseHidding(_currentHidePlace.position, _collider);
            }
        }
        void SetClosesHideSpot()
        {
            FindClosetHide();

            if (_currentHidePlace == null)
                ChooseRandomHidePlace();

            _coordinatesHidePlace = _currentHidePlace.position.GettOffSetHide(MyFSM.PlayerTransform.position);
            RunToPosition(_coordinatesHidePlace, true);
        }
        bool IsThereWhereToRunFromPlayer()
        {
            var direction = (transform.position - MyFSM.PlayerTransform.position).normalized; // Run The Oppsite Way

            var distanceRun = Random.Range(_stats.MinDistanceRunningFromPlayer, _stats.MaxDistanceRunningFromPlayer); // Choose Random Distance

            if (Physics.Raycast(transform.position, transform.forward, distanceRun, _stats.EndWorldObjectsLayer, QueryTriggerInteraction.Collide)) // Check If the direction he supposed to run is in to Limit of world, If it is set new Point to run to
                return false;

            _destinationTarget = transform.position + direction * distanceRun; // Caculate The Destintion to run to

            return true;
        }
        void FindClosetHide()
        {
            var hits = Physics.OverlapSphere(transform.position, _stats.RadioSphereToDetecCloseHideSpot, _stats.HideLayer);
            if (hits.Length > 0)
            {
                if (hits.Length > 1)
                {
                    var hidePosition = hits.OrderBy(Hits => (Hits.transform.position - transform.position).sqrMagnitude);
                    _currentHidePlace = hidePosition.FirstOrDefault().transform;
                    int numberOfTries = 0;
                    bool isSomeElseHiddingThere = _checkHidddingScript.IsThereSomeOneElseHidding(_currentHidePlace.position, _collider);
                    while (isSomeElseHiddingThere && numberOfTries > hits.Length - 1)
                    {
                        numberOfTries++;
                        _currentHidePlace = hidePosition.Skip(numberOfTries).FirstOrDefault().transform;
                        isSomeElseHiddingThere = _checkHidddingScript.IsThereSomeOneElseHidding(_currentHidePlace.position, _collider);
                    }
                }
                else
                    _currentHidePlace = hits[0].transform;
            }
            else
                ChooseRandomHidePlace();
        }

        bool CheckIfReached()
        {
            if (!MyFSM.Agent.pathPending)
                if (MyFSM.Agent.remainingDistance <= MyFSM.Agent.stoppingDistance)
                    if (!MyFSM.Agent.hasPath || MyFSM.Agent.velocity.sqrMagnitude == 0f)
                        return true;
            return false;
        }


        #endregion

        #region ZigZag Movement
        // it's better to use separate delta's instead of sharing Time.realtimeSinceStartup
        // because you can easily end up with a bunch of enemies oscillating in the same motion
        private float _zigZagDelta;

        [SerializeField] private float _zigZagDistance;
        Vector3 ZigZagStrafe()
        {
            // using sinus to generate zigzag between -1 and 1 , multiplying with some magnitude
            float t = Mathf.Sin(_zigZagDelta) * _zigZagDistance;
            // this is in local space
            Vector3 zigZagDisplacementLocal = Vector3.right * t;
            // this is now in world space
            Vector3 zigZagDisplacementWorld = transform.TransformDirection(zigZagDisplacementLocal);

            return zigZagDisplacementWorld;
        }
        void MoveZigZag()
        {
            _zigZagDelta += Time.deltaTime;

            Vector3 movementPos = _destinationTarget;
            movementPos += ZigZagStrafe(); // add the offset from the zigzag
            RunToPosition(movementPos, false);
        }

        #endregion




        private void CreateNewTokenCancelation()
        {
            _tokenSource = new CancellationTokenSource();
            _tokenToCancel = _tokenSource.Token;
        }
        private void OnTriggerEnter(Collider other) // Incase The player Will shoot nearby the enemy
        {
            if (other.CompareTag("Bullet"))
            {
                RoutineFindNewHideSpot();
            }
        }
        private void OnDrawGizmosSelected()     //Gizmo Of Detection Closes Hide Spot
        {
            if (_stats == null)
                return;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _stats.RadioSphereToDetecCloseHideSpot);
        }
    }


}
