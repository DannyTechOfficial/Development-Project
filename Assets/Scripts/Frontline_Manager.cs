using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class Frontline_Manager : MonoBehaviour
{

    private static Frontline_Manager _instance;

    public GameObject Player;
    public GameObject playerScoreObject, GameOverObject;
    
    public float moveSpeed;
    public int Health;
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
    public GameObject Collider1, Collider2, Collider3, Collider4, Collider5, Collider6;
    public Collisions C1, C2, C3, C4, C5, C6; 
        
    
    public static Frontline_Manager Instance
    {
        get
        {
            if (_instance is null)
            Debug.LogError("game manager null");
            return _instance;
        }
    }

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
        
        
    }

    // Update is called once per frame
    void Update()
    {
        PController.moveSpeed = moveSpeed;
        PController.health = Health;
        PController.dead = isDead;

        if (Convert.ToInt32(playerScore.text) == 100)
        {
            Debug.Log("You have completed the demo ! !");
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

    private void Awake() {
        _instance = this;
    }

    public void reachedMission(Collision2D other)
    {
        Debug.Log("Object Collectable and is Checkpoint");
        other.gameObject.SetActive(false);
        intPlayerScore += other.gameObject.GetComponent<Collisions>().points;
        playerScore.text = Convert.ToString(intPlayerScore);
        Debug.Log(playerScore);
        missionCount += 1;
        loadMission();
    }

    public void loadMission()
    {
        Debug.Log("LOADING MISSION !");
        switch (missionCount)
        {
            case 1:
                Collider1.transform.Translate(15, 0, 0);
                break;
            case 2:
                Collider2.transform.Translate(-16, 0, 0);
                break;
            case 3:
                Debug.Log("3");
                break;
        }
    }
}
