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

    }

    void SwitchCameras()
    {
        bool hasTarget = targeter.SelectTarget();

        if (hasTarget && targeter.currentTarget != null)
        {
            // 1. Define o alvo na Target Camera
            targetCamera.LookAt = targeter.currentTarget.transform;

            // 2. Avisa o Animator para trocar de estado (Ex: parâmetro bool 'IsLocked')
            // Certifique-se de que no State-Driven Camera, a 'Target Camera' responda a esse estado
            if (cameraStateAnimator != null) cameraStateAnimator.SetBool("IsLocked", true);
        }
        else
        {
            // Se não houver alvo, volta para a FreeLook
            targetCamera.LookAt = null;
            if (cameraStateAnimator != null) cameraStateAnimator.SetBool("IsLocked", false);
        }
    }
}