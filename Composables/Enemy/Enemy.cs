using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Renderer))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int life;
    public string enemyName;
    public float damage;
    public float detectionRadius = 4f;
    public float rotationSpeed = 10f;
    public GameObject hitFx;
    public Animator animator;
    public Renderer enemyRenderer;

    protected Color originalColor;
    protected bool isPlayerVisible;
    private Collider enemyCollider;

    // Mudamos para protected virtual para que os filhos (CrabEnemy) possam usar
    // No Enemy.cs
    protected virtual void Awake() // Mude para protected virtual
    {
        // Procura no objeto principal ou nos filhos (comum em modelos 3D importados)
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (enemyRenderer == null) enemyRenderer = GetComponentInChildren<Renderer>();
        enemyCollider = GetComponent<Collider>();
        if (enemyRenderer != null) originalColor = enemyRenderer.material.color;
    }
    void Reset()
    {
        animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
    }

    public void OnHit()
    {
        if (animator != null) animator.SetTrigger("hit");

        StartCoroutine(HitFlash());
        if (hitFx != null)
        {
            float height = (enemyCollider != null) ? enemyCollider.bounds.extents.y : 1f;
            Vector3 spawnPos = transform.position + new Vector3(0f, height, 0f);
            GameObject hitEffect = Instantiate(hitFx, spawnPos, transform.rotation);
            Destroy(hitEffect, 2f);
        }
    }

    IEnumerator HitFlash()
    {
        if (enemyRenderer == null) yield break;
        for (int i = 0; i < 2; i++)
        {
            enemyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            enemyRenderer.material.color = originalColor; // Volta para a cor real dele
            yield return new WaitForSeconds(0.2f);
        }
    }
    public virtual void Attack() { }

    public virtual void Move() { }

    public virtual void Death() { }

    public virtual void Follow()
    {
        if (Player.Instance == null) return;

        if (!isPlayerVisible)
        {
            float distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
            if (distance <= detectionRadius)
            {
                isPlayerVisible = true;
            }
        }
        else
        {
            RotateToPlayer();
        }
    }

    protected void RotateToPlayer()
    {
        if (Player.Instance == null) return;
        Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}