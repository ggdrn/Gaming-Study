using UnityEngine;

public class MimicEnemy : Enemy
{
    public int attackTime; 
    public int radiusAttack; 

    void Start()
    {
        damage = 10;
        enemyName = "Mímico";
        life = 50;
        attackTime = 2000;
        radiusAttack = 1;
    }
    public override void attacak()
    {
       
    }

    public override void death()
    {
       
    }

    public override void movement()
    {
        
    }

  
}
