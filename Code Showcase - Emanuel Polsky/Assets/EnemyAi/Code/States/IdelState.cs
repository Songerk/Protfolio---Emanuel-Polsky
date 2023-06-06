using GarmentButton.AI.Alert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    public class IdelState : State
    {
        #region Finite State Machine Function
        public override void OnEnterState()
        {
            if (MyFSM.AnimatorBody != null)
                MyFSM.AnimatorBody.SetTrigger("Idel");


        }
        public override void OnExitState()
        {
        }
        public override void UpdateState()
        {
            base.UpdateState();
        }
        protected override void checkTransition()
        {
            if(MyFSM.Alert.AlertState == StateOfAlert.Fleeing)
            {
                MyFSM.PlayerTransform = MyFSM.Alert.ReciveTarget();
                MyFSM.SwitchState(stateHandles.Crazy);
            }
        }
        #endregion
    }
}
