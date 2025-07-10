using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float velocity = 5f;
    public float sprintAdittion = 3.5f;
    public float jumpForce = 18f;
    public float jumpTime = 0.85f;
    public float gravity = 9.8f;

    public float rollSpeed = 10f;
    public float rollDuration = 1.4f;
    public float rollCooldown = 1f;

    private float jumpElapsedTime = 0;

    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isRolling = false;
    private bool isBlocking = false;
    private bool isAttacking = false;

    private float rollTimer = 0f;
    private float rollCooldownTimer = 0f;
    private Vector3 rollDirection;

    private float inputHorizontal;
    private float inputVertical;
    private bool inputJump;
    private bool inputCrouch;
    private bool inputSprint;
    private bool inputRoll;
    private bool inputBlock;

    private Animator animator;
    private CharacterController cc;

    private float rollEndBuffer = 0.1f;
    private float rollEndBufferTimer = 0f;

    private int attackCount = 0;
    private float lastAttackTime = 0f;
    public float attackResetTime = 5f;

    private AnimatorStateInfo currentState;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        inputBlock = Input.GetKey(KeyCode.F);

        if (inputBlock)
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }

        if (!isRolling && !isBlocking && !isAttacking)
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");
            inputJump = Input.GetAxis("Jump") == 1f;
            inputSprint = Input.GetAxis("Fire3") == 1f;
            inputCrouch = Input.GetKeyDown(KeyCode.C);
        }
        else
        {
            inputHorizontal = 0f;
            inputVertical = 0f;
            inputJump = false;
            inputSprint = false;
            inputCrouch = false;
        }

        inputRoll = Input.GetKeyDown(KeyCode.Q);

        if (inputCrouch && !isRolling && !isBlocking && !isAttacking)
        {
            isCrouching = !isCrouching;
        }

        if (cc.isGrounded && animator != null)
        {
            isBlocking = false;
            animator.SetBool("crouch", isCrouching);
            animator.SetBool("run", cc.velocity.magnitude > 0.9f);
            isSprinting = cc.velocity.magnitude > 0.9f && inputSprint;
            animator.SetBool("sprint", isSprinting);

            cc.center = new Vector3(0, 0.5f, 0);
            cc.height = 1f;

            rollEndBufferTimer = 0f;
        }

        if (!isRolling)
        {
            cc.center = new Vector3(0, 0.87f, 0);
            cc.height = 1.55f;
        }

        if (rollEndBufferTimer > 0f)
            rollEndBufferTimer -= Time.deltaTime;

        if (animator != null)
        {
            bool isInAir = !cc.isGrounded && !isRolling && rollEndBufferTimer <= 0f;
            animator.SetBool("air", isInAir);
            animator.SetBool("block", isBlocking);
        }

        if (inputJump && cc.isGrounded && !isRolling && !isBlocking && !isAttacking)
        {
            isJumping = true;
        }

        if (inputRoll && !isRolling && rollCooldownTimer <= 0f && !isJumping && !isBlocking && !isAttacking)
        {
            StartRoll();
        }

        if (Input.GetMouseButtonDown(0) && !isRolling && !isBlocking)
        {
            attackCount++;
            if (attackCount > 3) attackCount = 1;

            animator.SetInteger("AttackCount", attackCount);
            animator.SetTrigger("Attack");

            lastAttackTime = Time.time;

            isAttacking = true;
        }

        if (attackCount > 0 && Time.time - lastAttackTime >= attackResetTime)
        {
            attackCount = 0;
            animator.SetInteger("AttackCount", 0);
        }

        currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (isAttacking && !currentState.IsTag("Attack"))
        {
            isAttacking = false;
        }

        HeadHittingDetect();
    }

    void FixedUpdate()
    {
        if (rollCooldownTimer > 0f)
            rollCooldownTimer -= Time.deltaTime;

        if (isRolling)
        {
            cc.Move(rollDirection * (rollSpeed * Time.deltaTime));
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                isRolling = false;
                rollEndBufferTimer = rollEndBuffer;
            }
            return;
        }

        float velocityAdittion = 0;
        if (isSprinting) velocityAdittion = sprintAdittion;
        if (isCrouching) velocityAdittion = -(velocity * 0.5f);

        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.3f, jumpElapsedTime / jumpTime) * Time.deltaTime;
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        directionY -= gravity * Time.deltaTime;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveForward = forward * directionZ;
        Vector3 moveRight = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(moveForward.x + moveRight.x, moveForward.z + moveRight.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = moveForward + moveRight;

        Vector3 movement = verticalDirection + horizontalDirection;
        cc.Move(movement);
    }

    void StartRoll()
    {
        isRolling = true;
        rollTimer = rollDuration;
        rollCooldownTimer = rollCooldown;
        rollDirection = transform.forward;

        if (animator != null)
            animator.CrossFade("Roll", 0f);
    }

    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
