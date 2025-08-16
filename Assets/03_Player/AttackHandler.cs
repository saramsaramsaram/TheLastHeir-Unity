using UnityEngine;
using System.Collections;

namespace StarterAssets
{
    public class AttackHandler
    {
        private readonly ThirdPersonController _controller;
        private readonly Animator _animator;

        public bool IsAttacking { get; private set; } = false;

        private int _attackCount = 0;
        private bool _canReceiveCombo = true;
        private bool _attackBuffered = false;

        public AttackHandler(ThirdPersonController controller, Animator animator)
        {
            _controller = controller;
            _animator = animator;
        }

        public void CheckAttack()
        {
            if (_controller.Input.attack && !IsAttacking)
            {
                _controller.Input.attack = false;
                _attackCount = 1;
                _controller.StartCoroutine(AttackRoutine());
            }
            else if (_controller.Input.attack && IsAttacking && _canReceiveCombo)
            {
                _controller.Input.attack = false;
                _attackCount++;
                _attackBuffered = true;
                _canReceiveCombo = false;
            }
        }

        private IEnumerator AttackRoutine()
        {
            IsAttacking = true;

            _animator.SetInteger("AttackCount", _attackCount);
            _animator.SetTrigger("Attack");

            _attackBuffered = false;

            yield return new WaitForSeconds(1.2f);

            IsAttacking = false;
            _canReceiveCombo = true;

            if (_attackBuffered)
            {
                _canReceiveCombo = false;
                _attackCount++;
                _controller.StartCoroutine(AttackRoutine());
                
            }
        }

        public void EnableCombo() => _canReceiveCombo = true;
        public void DisableCombo() => _canReceiveCombo = false;
    }
}