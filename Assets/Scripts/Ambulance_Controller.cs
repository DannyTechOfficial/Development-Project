using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ambulance_Controller : MonoBehaviour
{
    //Sets properties & references for Controller
    Rigidbody2D rb2d;
    AudioSource Sound;
    Animator Animate;
    public static event Action onPlayerHit;
    public AudioClip Idle, Moving;
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
        //Retrieves fManager & additional components.
        fManager = Manager.GetComponent<Frontline_Manager>();
        rb2d = GetComponent<Rigidbody2D>();
        Sound = GetComponent<AudioSource>();
        Animate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for key inputs and outputs controls based on key inputs.
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
        // Sets velocities based on buttons pressed and rotates.
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
        //Based on if ambulance was moving or not changes the animation that are active.
        if(rb2d.velocity != new Vector2(0, 0))
        {
            AmbulanceAnimationController.SetBool("Moving", true);
            Animate.Play("AmbulanceOnCall");
        }
        else if(rb2d.velocity == new Vector2(0, 0))
        {
            AmbulanceAnimationController.SetBool("Moving", false);
            Animate.Play("AmbulanceIdle");
        }
        //Changes manager states for ambulance.
        if(fManager.Health <= 0)
        {
            fManager.isDead = true;   
        }
        else if (fManager.Health >=  1)
        {
            fManager.isDead = false;
        }
    }

    private void onIdle() // OnIdle called play audio idle.
    {
        Sound.clip = Idle;
        Sound.Play();
    }
    private void onMoving() // OnMoving called play audio moving.
    {
        Sound.clip = Moving;
        Sound.Play();
    }
    private void OnTriggerEnter2D(Collider2D other) //Function called when collider is triggered.
    {
        Debug.Log("Collission");
        if (other.gameObject.tag == "Collidable") // Checks if game object is collidable
        {
            if ((other.gameObject.GetComponent<Collisions>()).Damage > 0) //Checks damage is above 0
            {
                fManager.Health -= (other.gameObject.GetComponent<Collisions>().Damage);
                onPlayerHit?.Invoke();
                Debug.Log("Ambulance Crashed! - Health: " + health);
                if (fManager.Health <= 0)
                {
                    fManager.isDead = true;
                }
            }
            else if((other.gameObject.GetComponent<Collisions>()).checkpoint && (other.gameObject.GetComponent<Collisions>()).collectable) //If collectable and checkpoint then runs reachedmission called on fManager
            {
                fManager.reachedMission(other);
            }
        }
    }

}
