using UnityEngine;

public class FlyEnemy : Enemy
{
    public float flySpeed;
    public int flyTime;
    public int flyMoviment;

     public void Start()
    {
        damage = 10;
        enemyName = "Mosca";
        life = 50;
        flySpeed = 4f;
        flyTime = 1;
        flyMoviment = 2;

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
