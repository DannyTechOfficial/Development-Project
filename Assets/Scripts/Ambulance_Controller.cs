using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ambulance_Controller : MonoBehaviour
{
    Rigidbody2D rb2d;
    public GameObject Manager;
    public float moveSpeed;
    public int health;

    public bool dead;

//Movement States
    private const string RIGHT = "right";
    private const string LEFT = "left";
    private const string UP = "up";
    private const string DOWN = "down";
    Frontline_Manager fManager;
    [SerializeField] private Animator AmbulanceAnimationController;


    string buttonPressed;

    // Start is called before the first frame update
    void Start()
    {
        fManager = Manager.GetComponent<Frontline_Manager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            buttonPressed = RIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            buttonPressed = LEFT;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            buttonPressed = DOWN;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            buttonPressed = UP;
        }
        else
        {
            buttonPressed = null;
        }
    }

    void FixedUpdate() 
    {
        if(buttonPressed == RIGHT)
        {
            rb2d.SetRotation(270);
            rb2d.velocity = new Vector2(moveSpeed, 0);
        }
        else if(buttonPressed == LEFT)
        {
            rb2d.SetRotation(90);
            rb2d.velocity = new Vector2(-moveSpeed, 0);
        }
        else if(buttonPressed == UP)
        {
            rb2d.SetRotation(0);
            rb2d.velocity = new Vector2(0, moveSpeed);
        }
        else if(buttonPressed == DOWN)
        {
            rb2d.SetRotation(180);
            rb2d.velocity = new Vector2(0, -moveSpeed);
            
        }

        if(rb2d.velocity != new Vector2(0, 0))
        {
            AmbulanceAnimationController.SetBool("Moving", true);
        }
        else if(rb2d.velocity == new Vector2(0, 0))
        {
            AmbulanceAnimationController.SetBool("Moving", false);
        }
        
        if(fManager.Health <= 0)
        {
            fManager.isDead = true;   
        }
        else if (fManager.Health >=  1)
        {
            fManager.isDead = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Collidable")
        {
            if ((other.gameObject.GetComponent<Collisions>()).Damage > 0)
            {
                fManager.Health -= (other.gameObject.GetComponent<Collisions>().Damage);
                Debug.Log("Ambulance Crashed! - Health: " + health);
                if (fManager.Health <= 0)
                {
                    fManager.isDead = true;
                }
            }
            else if((other.gameObject.GetComponent<Collisions>()).checkpoint && (other.gameObject.GetComponent<Collisions>()).collectable)
            {
                fManager.reachedMission(other);
            }
        }
    }

}
