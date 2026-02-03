using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(Moviments))]
[RequireComponent(typeof(Attacks))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private Moviments moviments;
    private Attacks attacks;
    private PlayerInput playerInput;

    private InputAction attackAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lockOnToLAction;
    private InputAction lockOnToRAction;
    public static Player Instance;
    public Targeter targeter;
    public InputAction targeterAction;
    [Header("Cinemachine Settings")]
    public CinemachineCamera targetCamera; // Arraste a 'Target Camera' aqui
    public Animator cameraStateAnimator; // Animator que controla o State-Driven Camera

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        moviments = GetComponent<Moviments>();
        attacks = GetComponent<Attacks>();
        playerInput = GetComponent<PlayerInput>();
        targeter = GetComponentInChildren<Targeter>();
        // INICIALIZAÇÃO DA CÂMERA:
        // Busca o Animator no objeto "State-Driven Camera" dentro do Player
        cameraStateAnimator = GetComponentInChildren<CinemachineStateDrivenCamera>().GetComponent<Animator>();
        // Busca a Target Camera especificamente pelo nome ou tipo nos filhos
        targetCamera = transform.Find("State-Driven Camera/Target Camera").GetComponent<CinemachineCamera>();
        // Cache das ações para melhor performance
        attackAction = playerInput.actions["Attack"];
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        targeterAction = playerInput.actions["LockOn"];
        lockOnToLAction = playerInput.actions["LockOnToL"];
        lockOnToRAction = playerInput.actions["LockOnToR"];
    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame()) attacks.Execute();
        if (jumpAction.WasPressedThisFrame() && !attacks.isAttacking) moviments.ProcessJump();
        if (targeterAction.WasPressedThisFrame()) SwitchCameras();
        if (!attacks.isAttacking)
        {
            Vector2 inputVector = moveAction.ReadValue<Vector2>();
            // Passamos o X e o Y do Vector2 para o seu método de movimento
            moviments.ProcessMove(inputVector.x, inputVector.y);
        }
        else moviments.ProcessMove(0, 0); // Para o personagem durante o ataque
        if (!targeter.currentTarget && cameraStateAnimator.GetBool("IsLocked")) cameraStateAnimator.SetBool("IsLocked", false);
        // Só permite troca se existir um alvo atual
        if (targeter.currentTarget != null) SwitchTargetsOnLock();

    }

    void SwitchCameras()
    {
        bool hasTarget = targeter.SelectTarget();
        if (hasTarget)
        {
            targetCamera.LookAt = targeter.currentTarget.transform;
            if (cameraStateAnimator != null) cameraStateAnimator.SetBool("IsLocked", true);
        }
        else
        {
            // Se não houver alvo, volta para a FreeLook
            targetCamera.LookAt = null;
            if (cameraStateAnimator != null) cameraStateAnimator.SetBool("IsLocked", false);
        }
    }
    void SwitchTargetsOnLock()
    {
        // LockOn para ESQUERDA
        if (lockOnToLAction.WasPressedThisFrame())
        {
            targeter.SwitchTarget(
                Targeter.LockOnDirection.ToLeft,
                Camera.main
            );
            targetCamera.LookAt = targeter.currentTarget.transform;
        }
        // LockOn para DIREITA
        if (lockOnToRAction.WasPressedThisFrame())
        {
            targeter.SwitchTarget(
                Targeter.LockOnDirection.ToRight,
                Camera.main
            );
            targetCamera.LookAt = targeter.currentTarget.transform;
        }
    }
}