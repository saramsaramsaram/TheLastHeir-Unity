using UnityEngine;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("References")]
        public StarterAssetsInputs Input;
        public GameObject CinemachineCameraTarget;

        [Header("Movement")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        [Header("Jump")]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;

        [Header("Roll")]
        public float RollSpeed = 4.0f;
        public float RollDuration = 0.8f;

        [Header("Grounded")]
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Camera")]
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;

        [Header("Audio")]
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        private MovementHandler _movement;
        private JumpHandler _jump;
        private RollHandler _roll;
        private AttackHandler _attack;
        private HeavyAttackHandler _heavyAttack;
        private CameraHandler _cameraHandler;
        private GroundCheckHandler _groundCheck;

        private Animator _animator;
        private CharacterController _charController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _charController = GetComponent<CharacterController>();

            _groundCheck = new GroundCheckHandler(this, _animator);
            _jump = new JumpHandler(this, _charController, _animator, _groundCheck);
            _movement = new MovementHandler(this, _charController, _animator, _jump);
            _roll = new RollHandler(this, _charController, _animator, _groundCheck);
            _attack = new AttackHandler(this, _animator);
            _heavyAttack = new HeavyAttackHandler(this, _animator);
            _cameraHandler = new CameraHandler(this);
        }
        
        private void Update()
        {
            
            _groundCheck.CheckGrounded();
            _jump.ApplyGravityAndJump();
            _roll.CheckRoll();
            _heavyAttack.CheckHeavyAttack();
            _attack.CheckAttack();

            if (!_roll.IsRolling && !_attack.IsAttacking && !_heavyAttack.IsHeavyAttacking)
                _movement.Move();
        }

        private void LateUpdate()
        {
            if (!_roll.IsRolling && !_attack.IsAttacking)
                _cameraHandler.RotateCamera();
        }
        
        public void EnableCombo() => _attack.EnableCombo();
        public void DisableCombo() => _attack.DisableCombo();

        public void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
            {
                int index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(
                    FootstepAudioClips[index],
                    transform.TransformPoint(Vector3.zero),
                    FootstepAudioVolume
                );
            }
        }

        public void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && LandingAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    LandingAudioClip,
                    transform.TransformPoint(Vector3.zero),
                    FootstepAudioVolume
                );
            }
        }
    }
}
