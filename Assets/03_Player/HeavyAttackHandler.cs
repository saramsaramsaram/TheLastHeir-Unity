using UnityEngine;
using System.Collections;

namespace StarterAssets
{
    public class HeavyAttackHandler
    {
        private readonly ThirdPersonController _controller;
        private readonly Animator _animator;

        public bool IsHeavyAttacking { get; private set; } = false;

        public HeavyAttackHandler(ThirdPersonController controller, Animator animator)
        {
            _controller = controller;
            _animator = animator;
        }

        public void CheckHeavyAttack()
        {
            if (_controller.Input.heavyAttack && !IsHeavyAttacking)
            {
                _controller.Input.heavyAttack = false;
                _controller.StartCoroutine(HeavyAttackRoutine());
            }
        }

        private IEnumerator HeavyAttackRoutine()
        {
            IsHeavyAttacking = true;
            _animator.SetTrigger("HeavyAttack");
            yield return new WaitForSeconds(1f);
            IsHeavyAttacking = false;
        }
    }
}