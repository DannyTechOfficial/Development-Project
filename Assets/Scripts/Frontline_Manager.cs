using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class Frontline_Manager : MonoBehaviour
{

    public GameObject Player;
    public GameObject playerScoreObject, GameOverObject;
    
    public float moveSpeed;
    public int Health, maxScore;
    public bool isDead;
    public int Difficulty;

    public bool PotholeSpawn;
    public bool TrafficSpawn;
    public bool RoadCloseSpawn;

    private int missionCount = 1;
    private UnityEngine.UI.Text playerScore;
    private UnityEngine.UI.Text GameOverText;
    private int intPlayerScore = 0;

    Ambulance_Controller PController;
    public GameObject Collider1, Collider2, Collider3, Collider4, Collider5, Collider6, Collider7, Collider8, Collider9, Collider10, Collider11, Collider12;
    public Collisions C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12; 
        

    // Start is called before the first frame update
    void Start()
    {
        //Implementation of Story from JSON needs implementing.
        PController = Player.GetComponent<Ambulance_Controller>();
        playerScore = playerScoreObject.GetComponent<UnityEngine.UI.Text>();
        GameOverText = GameOverObject.GetComponent<UnityEngine.UI.Text>();
        
        
        loadMission();

        C1 = Collider1.GetComponent<Collisions>();
        C2 = Collider2.GetComponent<Collisions>();;
        C3 = Collider3.GetComponent<Collisions>();;
        C4 = Collider4.GetComponent<Collisions>();;
        C5 = Collider5.GetComponent<Collisions>();;
        C6 = Collider6.GetComponent<Collisions>();

        C7 = Collider1.GetComponent<Collisions>();
        C8 = Collider2.GetComponent<Collisions>();;
        C9 = Collider3.GetComponent<Collisions>();;
        C10 = Collider4.GetComponent<Collisions>();;
        C11 = Collider5.GetComponent<Collisions>();;
        C12 = Collider6.GetComponent<Collisions>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        PController.moveSpeed = moveSpeed;
        PController.health = Health;
        PController.dead = isDead;

        if (Convert.ToInt32(playerScore.text) == maxScore)
        {
            GameOverText.text = "Your reached the Max Score !   -   Well Done ! !";
        }
    }

    void FixedUpdate() 
    {
        if (isDead)
        {
            moveSpeed = 0;
            GameOverText.text = "You Crashed The Ambulance ! - Press ESC to go to menu.";
        }
    }
    public void reachedMission(Collision2D other)
    {
        Debug.Log("Object Collectable and is Checkpoint");
        other.gameObject.transform.Translate(0.0f, 50.0f, 0.0f);
        intPlayerScore += other.gameObject.GetComponent<Collisions>().points;
        playerScore.text = Convert.ToString(intPlayerScore);
        Debug.Log(playerScore);
        missionCount += 1;
        moveSpeed += 1.5f;
        loadMission();
        if(missionCount == 6) missionCount = 0;
    }

    public void loadMission()
    {
        Debug.Log("LOADING MISSION !");
        switch (missionCount)
        {
            case 1:
                Collider1.transform.position = new Vector3(-15.61f, 11.92f, -1.0f);
                break;
            case 2:
                Collider2.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                break;
            case 3:
                Collider3.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                break;
            case 4:
                Collider4.transform.position = new Vector3(0f, 0f, -1.0f);
                break;
            case 5:
                Collider5.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                break;
            case 6:
                Collider6.transform.position = new Vector3(0f, 0f, -1.0f);
                break;
        }
    }
}
