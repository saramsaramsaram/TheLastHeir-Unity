using UnityEngine;

namespace StarterAssets
{
    public class JumpHandler
    {
        private readonly ThirdPersonController _controller;
        private readonly CharacterController _charController;
        private readonly Animator _animator;
        private readonly GroundCheckHandler _groundCheck;

        private float _verticalVelocity;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        public float VerticalVelocity => _verticalVelocity;

        public JumpHandler(
            ThirdPersonController controller,
            CharacterController charController,
            Animator animator,
            GroundCheckHandler groundCheck)
        {
            _controller = controller;
            _charController = charController;
            _animator = animator;
            _groundCheck = groundCheck;

            _jumpTimeoutDelta = _controller.JumpTimeout;
            _fallTimeoutDelta = _controller.FallTimeout;
        }

        public void ApplyGravityAndJump()
        {
            if (_groundCheck.IsGrounded)
            {
                _fallTimeoutDelta = _controller.FallTimeout;

                _animator.SetBool("Jump", false);
                _animator.SetBool("FreeFall", false);

                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;

                if (_controller.Input.jump && _jumpTimeoutDelta <= 0f)
                {
                    _verticalVelocity = Mathf.Sqrt(_controller.JumpHeight * -2f * _controller.Gravity);
                    _animator.SetBool("Jump", true);
                    _controller.Input.jump = false;
                }

                if (_jumpTimeoutDelta > 0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = _controller.JumpTimeout;

                if (_fallTimeoutDelta >= 0f)
                    _fallTimeoutDelta -= Time.deltaTime;
                else
                    _animator.SetBool("FreeFall", true);
            }

            if (_verticalVelocity > _controller.Gravity)
                _verticalVelocity += _controller.Gravity * Time.deltaTime;
        }
    }
}
