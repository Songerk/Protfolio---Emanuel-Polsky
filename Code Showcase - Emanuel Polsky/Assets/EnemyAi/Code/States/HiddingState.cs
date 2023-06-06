using GarmentButton.AI.Alert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    public class HiddingState : State
    {

        #region Finite State Machine Function
        public override void OnEnterState()
        {


        }
        public override void OnExitState()
        {

        }
        public override void UpdateState()
        {
            base.UpdateState();
            if (MyFSM.Alert.AlertState == StateOfAlert.Alert)
                MoveAround();
        }
        protected override void checkTransition()
        {
            if (MyFSM.Alert.AlertState == StateOfAlert.Fleeing)
            {
                MyFSM.PlayerTransform = MyFSM.Alert.ReciveTarget();
                MyFSM.SwitchState(stateHandles.Crazy);
                transform.LookAt(MyFSM.PlayerTransform);
            }
        }

        void MoveAround()
        {
            var moveTo = MyFSM.CurrentHidePosition.position.GettOffSetHide(MyFSM.PlayerTransform.position);

            MyFSM.Agent.SetDestination(moveTo);
        }
        #endregion
    }
}
