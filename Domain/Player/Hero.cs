using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Moviments))]
[RequireComponent(typeof(Attacks))]
[RequireComponent(typeof(PlayerInput))] // Garante que o componente PlayerInput esteja no objeto
public class Hero : MonoBehaviour
{
    private Moviments moviments;
    private Attacks attacks;
    private PlayerInput playerInput;
    
    private InputAction attackAction;
    private InputAction moveAction;
    private InputAction jumpAction;

    void Start()
    {
        moviments = GetComponent<Moviments>();
        attacks = GetComponent<Attacks>();
        playerInput = GetComponent<PlayerInput>();
        
        // Cache das ações para melhor performance
        attackAction = playerInput.actions["Attack"];
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        // 1. Verifica Ataque
        // Isso vai disparar com o Botão West (Quadrado), Left Click ou Enter, como no seu print
        if (attackAction.WasPressedThisFrame()) attacks.Execute();
        // if (jumpAction.WasPressedThisFrame() && !attacks.isAttacking) moviments.ProcessJump();
        if (!attacks.isAttacking)
        {
            Vector2 inputVector = moveAction.ReadValue<Vector2>();
            // Passamos o X e o Y do Vector2 para o seu método de movimento
            moviments.ProcessMove(inputVector.x, inputVector.y);
        }
        else moviments.ProcessMove(0, 0); // Para o personagem durante o ataque
        
    }
}