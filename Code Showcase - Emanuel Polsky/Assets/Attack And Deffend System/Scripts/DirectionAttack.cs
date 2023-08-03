using GarmentButton.InputControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class DirectionAttack : MonoBehaviour
    {
        public enum Direction { Forawd, Up, Down }
        public Direction direction;
        [SerializeField] private TardovInputController _input;
        [SerializeField] private Animator _animator;
        [SerializeField] private float thresholdForUpAndDown = 0.3f;

        [SerializeField] private bool _debug;

        int[] _forwardAttacks;
        int[] _upwardsAttacks;
        int[] _downwardAttacks;
        private void Start()
        {
            SetArries();
;       }
        public void AttackFunction()
        {
            direction = GetDirection();
            if(_debug) Debug.Log($"Play Animations    {direction} ");
            switch (direction)
            {
                case Direction.Forawd:
                    _animator.Play(_forwardAttacks[Random.Range(0, _forwardAttacks.Length - 1)]);
                    break;
                case Direction.Up:
                    _animator.Play(_upwardsAttacks[Random.Range(0, _upwardsAttacks.Length - 1)]);
                    break;
                case Direction.Down:
                    _animator.Play(_downwardAttacks[Random.Range(0, _downwardAttacks.Length - 1)]);
                    break;
            }

        }
        
        private Direction GetDirection()
        {
            Vector2 direction = _input.FrameInput.Move.normalized;
            if (direction.y > thresholdForUpAndDown)
                return Direction.Up;
            else if (direction.y < -thresholdForUpAndDown)
                return Direction.Down;
            else
                return Direction.Forawd;
        }
        void SetArries()
        {
            _forwardAttacks = new int[4];
            _forwardAttacks[0] = Attack;
            _forwardAttacks[1] = Attack2;
            _forwardAttacks[2] = Attack3;
            _forwardAttacks[3] = Attack4;

            _upwardsAttacks = new int[1];
            _upwardsAttacks[0] = Attack1Up;
/*            _upwardsAttacks[1] = Attack2Up;
            _upwardsAttacks[2] = Attack3Up;
            _upwardsAttacks[3] = Attack4Up;*/

            _downwardAttacks = new int[1];
            _downwardAttacks[0] = Attack1Down;
/*            _downwardAttacks[1] = Attack2Down;
            _downwardAttacks[2] = Attack3Up;
            _downwardAttacks[3] = Attack4Up;*/
        }


        [Header("ForawdAttacks")]
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Attack2 = Animator.StringToHash("Attack2");
        private static readonly int Attack3 = Animator.StringToHash("Attack3");
        private static readonly int Attack4 = Animator.StringToHash("Attack4");

        [Header("UpawdAttacks")]
        private static readonly int Attack1Up = Animator.StringToHash("AttackUp");
        private static readonly int Attack2Up = Animator.StringToHash("AttackUp2");
        private static readonly int Attack3Up = Animator.StringToHash("AttackUp3");
        private static readonly int Attack4Up = Animator.StringToHash("AttackUp4");

        [Header("UpawdAttacks")]
        private static readonly int Attack1Down = Animator.StringToHash("AttackDown");
        private static readonly int Attack2Down = Animator.StringToHash("AttackDown2");
        private static readonly int Attack3Down = Animator.StringToHash("AttackDown3");
        private static readonly int Attack4Down = Animator.StringToHash("AttackDown4");

    }
}
