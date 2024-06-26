using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoyStick_Player : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public float diff = 1;
    public GameObject player;
    public Transform Player_2;
    public GamePlay_Canvas GamePlayCanvas;
    public GameObject Coin;
    private GameObject collidedCoin;
    public Npc_player Enemy;
    public float Enemy_Position;
    public float Player_Position;
    public float Enemy_postion_onexit;
    public float Player_postion_onexit;
    public float Distance;
    public Enemy_AnimationController Enemy_AnimationController;
    public bool IsAnimationOver = false;
    public float player_health = 5;
    public Rigidbody2D player_RB;
    public AnimationController Player_Animation;
    public GamePlay_Canvas Canvas;
    public GoombaWalk Goomba;
   

    public Vector3 currentTargetPosition;


    public float minY = -2f;
    public float maxY = 2f; 

    private void Start()
    {
        player_RB = GetComponent<Rigidbody2D>();
    }

    void Update()   
    {
        float clampedY = Mathf.Clamp(transform.position.y, float.MinValue, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    

        if (fixedJoystick.Horizontal > 0.1f)
        {
            Quaternion newRotation = Quaternion.Euler(0, 0, 0);
            player.transform.rotation = newRotation;
            Vector3 newPos = new Vector3(fixedJoystick.Horizontal, 0);
            transform.position += newPos * diff * Time.deltaTime;
        }

        else if (fixedJoystick.Horizontal < -0.1f)
        {

            Quaternion newRotation = Quaternion.Euler(0, 180, 0);
            player.transform.rotation = newRotation;
            Vector3 newPos = new Vector3(fixedJoystick.Horizontal, 0);
            transform.position += newPos * diff * Time.deltaTime;
        }
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Coin")) 
        {
            GamePlayCanvas.CoinCount();

            collidedCoin = col.gameObject;
            collidedCoin.SetActive(false);
       
        }

        if(col.gameObject.CompareTag("Attack"))
        {
         //  var temp = col.gameObject.GetComponent<Npc_player>(); 
         //   if (temp != null)
         //   {
         //       Enemy = temp;
         //   }
           Enemy_Position = Enemy.transform.position.x;
           Player_Position = player.transform.position.x;

            if(Player_Position > Enemy_Position )
            {
                Quaternion newRotation_1 = Quaternion.Euler(0, 0, 0);
                Enemy.transform.rotation = newRotation_1;
                EnemymovetoPlayer();
            }
            else
            {
                Quaternion NewRotation_2 = Quaternion.Euler(0, 180, 0);
                Enemy.transform.rotation = NewRotation_2;
                EnemymovetoPlayer();
            }

        }
          
        if(col.gameObject.CompareTag("Castle"))
        {
            Canvas.OnGameWin();
        }

       
       
    }


    public void OnTriggerStay2D(Collider2D col2)
    {

        Enemy_Position = Enemy.transform.position.x;
        Player_Position = player.transform.position.x;

        if (Player_Position > Enemy_Position)
        {
            Quaternion newRotation_1 = Quaternion.Euler(0, 0, 0);
            Enemy.transform.rotation = newRotation_1;
            EnemymovetoPlayer();
        }
        else
        {
            Quaternion NewRotation_2 = Quaternion.Euler(0, 180, 0);
            Enemy.transform.rotation = NewRotation_2;
            EnemymovetoPlayer();
        }
        if (col2.gameObject.CompareTag("Attack"))
        { 
            Distance = Player_2.transform.position.x - Enemy.transform.position.x;
            
            if (Distance < 0.1)
            {
                Debug.Log("attack");
                Enemy_AnimationController.Enemy_Attack();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Attack"))
        {
            Enemy.currentTargetPosition = Enemy.Tg1;
            Quaternion NewRotation_2 = Quaternion.Euler(0, 180, 0);
            Enemy.transform.rotation = NewRotation_2;
        }


    }
    public void EnemymovetoPlayer()
    {
        Enemy.currentTargetPosition = Player_2;
    }

    public void AttackAnimationOver()
    {
        IsAnimationOver = true;
    }

    public void OnEnemyAttack()
    {
         
        if(player_health == 0)
        {
            player_RB.constraints = RigidbodyConstraints2D.FreezeAll;
            Player_Animation.Player_dead(); 
           

        }

        else
        {
            player_health -= 1;
            Debug.Log("player" + player_health);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("GameOver"))
        {
            Canvas.GameOverEnable();
            player.SetActive(false);
        }

        if(collision.gameObject.CompareTag("Goomba"))
        {
            Canvas.healthCount();
            Player_Animation.Player_Hurt();

        }
    }

    public void Playerdead()
    {
        player.SetActive(false);
        Canvas.GameOverEnable();
    }

    
}
