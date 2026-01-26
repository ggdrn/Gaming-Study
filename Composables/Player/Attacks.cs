using UnityEngine;

public class Attacks : MonoBehaviour
{
    public float attackCooldown = 0.53f;
    public bool isAttacking { get; private set; } // Outras classes podem ler, mas não alterar

    private Animator animator;
    public ParticleSystem slashFx;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Execute()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            slashFx.Play();
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}