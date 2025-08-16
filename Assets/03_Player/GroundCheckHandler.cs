using UnityEngine;

namespace StarterAssets
{
    public class GroundCheckHandler
    {
        private readonly ThirdPersonController _controller;
        private readonly Animator _animator;

        public bool IsGrounded { get; private set; } = true;

        public GroundCheckHandler(ThirdPersonController controller, Animator animator)
        {
            _controller = controller;
            _animator = animator;
        }

        public void CheckGrounded()
        {
            Vector3 pos = _controller.transform.position + Vector3.down * _controller.GroundedOffset;
            IsGrounded = Physics.CheckSphere(pos, _controller.GroundedRadius, _controller.GroundLayers);
            _animator.SetBool("Grounded", IsGrounded);
        }
    }
}