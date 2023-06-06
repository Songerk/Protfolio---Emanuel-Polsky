using UnityEngine;
using UnityEngine.AI;

namespace GarmentButton.AI.StateMechine
{
    public abstract class State : MonoBehaviour
    {
        protected FSMEnemy MyFSM;


        public virtual void OnEnterState()
        {

        }
        public virtual void OnExitState()
        {

        }
        public virtual void UpdateState()
        {

            checkTransition();
        }

        protected virtual void checkTransition()
        {

        }
        public void Bind(FSMEnemy tobind)
        {
            MyFSM = tobind;
        }


    }
}