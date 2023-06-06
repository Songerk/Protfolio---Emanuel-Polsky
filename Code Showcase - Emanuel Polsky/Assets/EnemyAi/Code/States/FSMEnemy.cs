using GarmentButton.AI.Alert;
using UnityEngine;
using UnityEngine.AI;

namespace GarmentButton.AI.StateMechine
{
    public enum stateHandles { Idel, Crazy, HiddenAndRunningBackToShip, Dragged, Fallen }
    public class FSMEnemy : MonoBehaviour
    {
        #region States Variables
        [SerializeField] State[] _states = new State[0];
        private State _currentState;

        [HideInInspector]
        public bool IsBeingDraged;

        #endregion


        #region Required Reffrences
        [HideInInspector]
        public Transform PlayerTransform;
        public HandelAlert Alert;
        public NavMeshAgent Agent;
        public Animator AnimatorBody;
        public Animator AnimatorDuckSmallBody;

        [HideInInspector] public NavMeshPath AgentPath;
        [HideInInspector] public Transform CurrentHidePosition;

        #endregion


        // Start is called before the first frame update
        void Start()
        {
            //CheckForPlayer a = new CheckForPlayer();
            SetUp();
            if (_states.Length == 0)
                _states = GetComponents<State>();
            for (int i = 0; i < _states.Length; i++)
            {
                _states[i].Bind(this);
            }
            _currentState = _states[0];

            _currentState.OnEnterState();
        }

        // Update is called once per frame
        void Update()
        {
            _currentState.UpdateState();
        }

        public void SwitchState(stateHandles toSwitch)
        {
            _currentState.OnExitState();
            _currentState = _states[(int)toSwitch];
            _currentState.OnEnterState();
        }
        private void SetUp()
        {
/*            if (Agent == null)
                Agent = GetComponent<NavMeshAgent>();
            Agent.updatePosition = false;*/
        }

        #region External Changes
        public void GetHit()
        {
            SwitchState(stateHandles.Fallen);
        }
        #endregion
    }
}