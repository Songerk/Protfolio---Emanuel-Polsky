using GarmentButton.AI.Alert;
using System.Collections;
using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    public class HiddenAndRunningBackToShip : State
    {
        [SerializeField] private EnemyDuckStats _stats;

        #region Variables For Begging Escape To Ship

        private float _timerToRunBackToShip;
        private bool _startedToRun;

        #endregion

        #region Finite State Machine Function
        public override void OnEnterState()
        {
            _startedToRun = false;
        }
        public override void OnExitState()
        {

        }
        public override void UpdateState()
        {
            base.UpdateState();
            if (!_startedToRun)
                CheckIfToRunBackToShip();
            if (MyFSM.Alert.AlertState == StateOfAlert.Alert)
                MoveAround();
        }
        protected override void checkTransition()
        {
            if (MyFSM.Alert.AlertState == StateOfAlert.Fleeing)
            {
                MyFSM.PlayerTransform = MyFSM.Alert.ReciveTarget();
                MyFSM.SwitchState(stateHandles.Crazy);
            }
        }
        #endregion

        void CheckIfToRunBackToShip()
        {


            if (_timerToRunBackToShip > _stats.WhenToRunBackToShip)
            {
                int RandomNumber = Random.Range(0, 2);
                MyFSM.Agent.SetDestination(_stats.SpaceShipPositions[RandomNumber]);
                _startedToRun = true;
                _timerToRunBackToShip = 0f;
            }
            else
                _timerToRunBackToShip += Time.deltaTime;
        }

        void MoveAround()
        {
            _timerToRunBackToShip = 0;
            _startedToRun = false;
            var moveTo = MyFSM.CurrentHidePosition.position.GettOffSetHide(MyFSM.PlayerTransform.position);
            transform.LookAt(MyFSM.PlayerTransform);
            MyFSM.Agent.SetDestination(moveTo);
        }
    }
}