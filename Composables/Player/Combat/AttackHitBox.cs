using UnityEngine;
// Scrript fica na arma que vai acertar o Objeto, não se esqueça que ela deve compor
public class AttackHitBox : MonoBehaviour
{
    private int enemyLayer;

    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            other.GetComponent<Enemy>().OnHit();
        }
    }
}