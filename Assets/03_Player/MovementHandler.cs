using UnityEngine;

namespace StarterAssets
{
    public class MovementHandler
    {
        private readonly ThirdPersonController _controller;
        private readonly CharacterController _charController;
        private readonly Animator _animator;
        private readonly JumpHandler _jump;

        private float _speed;
        private float _animationBlend;
        private float _rotationVelocity;
        private float _targetRotation = 0f;

        public MovementHandler(
            ThirdPersonController controller,
            CharacterController charController,
            Animator animator,
            JumpHandler jumpHandler)
        {
            _controller = controller;
            _charController = charController;
            _animator = animator;
            _jump = jumpHandler;
        }

        public void Move()
        {
            float targetSpeed = _controller.Input.sprint ? _controller.SprintSpeed : _controller.MoveSpeed; 
            if (_controller.Input.move == Vector2.zero)
                targetSpeed = 0f;

            float currentHorizontalSpeed = new Vector3(_charController.velocity.x, 0f, _charController.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _controller.Input.move.magnitude;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _controller.SpeedChangeRate);
            else
                _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _controller.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_controller.Input.move.x, 0f, _controller.Input.move.y).normalized;
            if (_controller.Input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(_controller.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _controller.RotationSmoothTime);
                _controller.transform.rotation = Quaternion.Euler(0f, rotation, 0f);  
            }

            Vector3 targetDirection = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;
            _charController.Move(
                targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0f, _jump.VerticalVelocity, 0f) * Time.deltaTime
            );

            _animator.SetFloat("Speed", _animationBlend);
            _animator.SetFloat("MotionSpeed", inputMagnitude);
        }
    }
}
