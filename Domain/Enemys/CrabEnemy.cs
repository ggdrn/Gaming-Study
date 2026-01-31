using UnityEngine;

public class CrabEnemy : Enemy
{
    void Start()
    {
        damage = 10;
        enemyName = "Carangueijo";
        life = 90;
    }
    void Update()
    {
        Follow();
    }

}
