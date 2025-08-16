using UnityEngine;
using System.Collections;

namespace StarterAssets
{
    public class RollHandler
    {
        private readonly ThirdPersonController _controller;
        private readonly CharacterController _charController;
        private readonly Animator _animator;
        private readonly GroundCheckHandler _groundCheck;

        public bool IsRolling { get; private set; } = false;

        public RollHandler(
            ThirdPersonController controller,
            CharacterController charController,
            Animator animator,
            GroundCheckHandler groundCheck)
        {
            _controller = controller;
            _charController = charController;
            _animator = animator;
            _groundCheck = groundCheck;
        }

        public void CheckRoll()
        {
            if (_controller.Input.roll && !IsRolling && _groundCheck.IsGrounded)
            {
                _controller.Input.roll = false;
                _controller.StartCoroutine(RollRoutine());
            }
        }

        private IEnumerator RollRoutine()
        {
            IsRolling = true;
            _animator.SetTrigger("Roll");

            Vector3 direction = _controller.transform.forward;

            float timer = 0f;
            while (timer < _controller.RollDuration)
            {
                _charController.Move(direction * (_controller.RollSpeed * Time.deltaTime));
                timer += Time.deltaTime;
                yield return null;
            }

            IsRolling = false;
        }
    }
}