using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int life;
    public string enemyName;
    public float damage;
    public bool isPlayerVisible;

    public float detectionRadius;
    public float rotationSpeed;

    void Awake()
    {
        detectionRadius = 4f;
        rotationSpeed = 10f;
    }
    public virtual void Attacl()
    {
        
    }

    public virtual void Move()
    {
        
    }

    public virtual void Death()
    {
        
    }

    public virtual void OnHit()
    {
        
    }
    public virtual void Follow()
    {
        if (!isPlayerVisible)
        {
            float distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
            if (distance <= detectionRadius)
            {
                isPlayerVisible = true;
            }
        }else
        {
            RotateToPlayer();
        }
    }

    void RotateToPlayer()
    {
        Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
        direction.y = 0f;
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
