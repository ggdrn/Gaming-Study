using UnityEngine;

public class Moviments : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    public float jumpCooldown = 0.80f;
    public bool isJumping { get; private set; } // Outras classes podem ler, mas não alterar



    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    // Recebe o input e processa a movimentação
    public void ProcessMove(float horizontal, float vertical)
    {
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        if (input.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * input.z) + (camRight * input.x);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Atualiza a animação de andar/correr
        animator.SetFloat("Speed", input.magnitude);
    }

    private float verticalVelocity = 0f;
    private float jumpForce = 10f;
    private float gravity = -9.81f;

    public void ProcessJump()
    {
        if (!isJumping)
        {
            animator.SetTrigger("Jump");
            isJumping = true;
            verticalVelocity = jumpForce;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void Update()
    {
        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            Vector3 jumpMovement = Vector3.up * verticalVelocity * Time.deltaTime;
            controller.Move(jumpMovement);
        }
    }

    private void ResetJump()
    {
        isJumping = false;
    }
}
