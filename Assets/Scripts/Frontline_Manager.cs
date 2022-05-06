using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


using System;

public class Frontline_Manager : MonoBehaviour
{
    //Sets properties & references for Manager
    public GameObject frontline_Manager;
    //Blank references to Missions & Config
    public MissionList missionList = new MissionList();
    public ConfigList configList = new ConfigList();
    public GameObject Player;
    public GameObject playerScoreObject, GameOverObject, MissionTitleObject, MissionTextObject;
    public float moveSpeed, multiplierSpeed, maxScore;

    //Various variables for different objects and features.
    public int Health, maxHealth;
    public bool isDead;
    public int Difficulty;

    public bool PotholeSpawn;
    public bool TrafficSpawn;
    public bool RoadCloseSpawn;

    private bool Countdown;
    private int seconds = 0;
    private int missionCount = 1;
    private UnityEngine.UI.Text playerScore;
    private UnityEngine.UI.Text GameOverText;
    private UnityEngine.UI.Text MissionTitle;
    private UnityEngine.UI.Text MissionMessage;
    private int intPlayerScore = 0;
    private int i = 1;
    private int activeMission = 0;
    private int randomNumber;
    private string text, data;
    Ambulance_Controller PController;
    FileInfo[] allFiles;
    DirectoryInfo directoryInfo;
    public GameObject Mission1, Mission2, Mission3, Mission4, Mission5, Mission6, Pothole1, Pothole2, Pothole3, RClosed1, RClosed2, RClosed3;
        
    // Start is called before the first frame update
    void Start()
    {
        //Loads configuation files based on active scene.
        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        print("Streaming Assets Path: " + Application.streamingAssetsPath);
        allFiles = directoryInfo.GetFiles("*.*");
        if (SceneManager.GetActiveScene().name == "Start Menu") //Loads config json if scene is Start Menu
        {
            foreach (FileInfo file in allFiles)
            {
                Debug.Log(file.Name);
                //Loads config from Config File.
                if (file.Name == "Config.json")
                {
                    Debug.Log("CONFIG FOUND !!!");
                    Debug.Log(file.Directory.ToString() + "/" + file.Name);
                    string dataAsJSON = File.ReadAllText(file.Directory.ToString() + "/" + file.Name);
                    configList = JsonUtility.FromJson<ConfigList>(dataAsJSON);
                    Debug.Log(configList.config[0]);
                }
            }

        }
        else //Loads both json files if scene is anything else.
        {
            foreach (FileInfo file in allFiles)
            {
                Debug.Log(file.Name);
                if (file.Name == "Missions.json")
                {
                    Debug.Log("MISSIONS FOUND !!!");
                    Debug.Log(file.Directory.ToString() + "/" + file.Name);
                    string dataAsJSON = File.ReadAllText(file.Directory.ToString() + "/" + file.Name);
                    missionList = JsonUtility.FromJson<MissionList>(dataAsJSON);
                    Debug.Log(missionList.mission[0]);
                }
                //Loads config from Config File.
                if (file.Name == "Config.json")
                {
                    Debug.Log("CONFIG FOUND !!!");
                    Debug.Log(file.Directory.ToString() + "/" + file.Name);
                    string dataAsJSON = File.ReadAllText(file.Directory.ToString() + "/" + file.Name);
                    configList = JsonUtility.FromJson<ConfigList>(dataAsJSON);
                    Debug.Log(configList.config[0]);
                }
            }
            //Sets values based on data in json files.
            maxHealth = configList.config[0].health;
            Health = maxHealth;
            moveSpeed = configList.config[0].speed;
            //missionList = JsonUtility.FromJson<MissionList>(textJSON.text);
            //Implementation of Story from JSON needs implementing.
            PController = Player.GetComponent<Ambulance_Controller>();
            playerScore = playerScoreObject.GetComponent<UnityEngine.UI.Text>();
            GameOverText = GameOverObject.GetComponent<UnityEngine.UI.Text>();
            MissionTitle = MissionTitleObject.GetComponent<UnityEngine.UI.Text>();
            MissionMessage = MissionTextObject.GetComponent<UnityEngine.UI.Text>();
            maxScore = configList.config[0].highscore;

            missionCount = (missionList.mission.Length);

            loadMission(); //Starts mission loading.

        }
    }

    // Update is called once per frame
    void Update()
    {
        //Updates every frame PController variables with manager variables.
        PController.moveSpeed = moveSpeed;
        PController.health = Health;
        PController.dead = isDead;
        if (Convert.ToInt32(playerScore.text) >= maxScore) //Checks for max score is reached.
        {
            GameOverText.text = "Your reached the Max Score !   -   Well Done ! !";
        }
    }

    void FixedUpdate() //Updates Once a second
    {
        if (Countdown) //If countdown is true
        {
            //Displays mission text
            MissionTitle.text = missionList.mission[activeMission].name;
            MissionMessage.text = missionList.mission[activeMission].missionText;
            //Adds 1 to counter
            seconds += 1;
        }
        if (seconds == 800) //If counter reaches 800 then clear mission text and reset.
        {
            Countdown = false;
            seconds = 0;
            MissionTitle.text = null;
            MissionMessage.text = null;
        }
        if (isDead) //If player marked as dead.
        {
            moveSpeed = 0;
            GameOverText.text = "You Crashed The Ambulance ! - Press ESC to go to menu.";
        }
        if (SceneManager.GetActiveScene().name == "Start Menu") //Refreshes config data every second so player can update file in-game.
        {
            foreach (FileInfo file in allFiles)
            {
                //Loads config from Config File.
                if (file.Name == "Config.json")
                {
                    Debug.Log("CONFIG FOUND !!!");
                    Debug.Log(file.Directory.ToString() + "/" + file.Name);
                    string dataAsJSON = File.ReadAllText(file.Directory.ToString() + "/" + file.Name);
                    configList = JsonUtility.FromJson<ConfigList>(dataAsJSON);
                    Debug.Log(configList.config[0]);
                }
            }
        }
    }

    public void reachedMission(Collider2D other) //If mission is reached
    {
        //moves previous mission, sets properties of next mission and starts to load.
        other.gameObject.transform.Translate(0.0f, 50.0f, 0.0f);
        intPlayerScore += other.gameObject.GetComponent<Collisions>().points;
        playerScore.text = Convert.ToString(intPlayerScore);
        i += 1;
        activeMission += 1;
        //moveSpeed += 1.5f;
        if (activeMission < missionList.mission.Length) loadMission(); else {activeMission = 0; loadMission();}
    }
    public void loadMission() //On loading mission/
    {
        Debug.Log("LOADING MISSION !");
        randomNumber = Random.Range(1,13); //Decides on random location for mission.

        multiplierSpeed =  missionList.mission[activeMission].difficulty; //Retrieves mission difficulty and changes multiplier for traffic speed.

        if(int.Parse(playerScore.text) < maxScore) //If max score isn't reacehed.
        {
            
            switch (randomNumber) //Loads random location and sets mission based on next one in configuation list.
            {
                case 1:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission1.transform.position = new Vector3(-15.61f, 11.92f, -1.0f);
                    Countdown = true;

                    break;
                case 2:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission2.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                    Countdown = true;
                    break;
                case 3:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission3.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                    Countdown = true;
                    break;
                case 4:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission4.transform.position = new Vector3(0f, 0f, -1.0f);
                    Countdown = true;
                    break;
                case 5:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission5.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                    Countdown = true;
                    break;
                case 6:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission6.transform.position = new Vector3(0f, 0f, -1.0f);
                    Countdown = true;
                    break;
                case 7:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission1.transform.position = new Vector3(-15.61f, -5.95f, -1.0f);
                    Countdown = true;

                    break;
                case 8:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission2.transform.position = new Vector3(-11.6f, 1.5f, -1.0f);
                    Countdown = true;
                    break;
                case 9:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission3.transform.position = new Vector3(12.35f, 8.78f, -1.0f);
                    Countdown = true;
                    break;
                case 10:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission4.transform.position = new Vector3(8.37f, -8.57f, -1.0f);
                    Countdown = true;
                    break;
                case 11:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission5.transform.position = new Vector3(-0.26f, -2.98f, -1.0f);
                    Countdown = true;
                    break;
                case 12:
                    if (missionList.mission[activeMission].potholes) { Pothole1.SetActive(true); Pothole2.SetActive(true); Pothole3.SetActive(true); }
                    else { Pothole1.SetActive(false); Pothole2.SetActive(false); Pothole3.SetActive(false); }
                    if (missionList.mission[activeMission].contstruction) { RClosed1.SetActive(true); RClosed2.SetActive(true); RClosed3.SetActive(true); }
                    else { RClosed1.SetActive(false); RClosed2.SetActive(false); RClosed3.SetActive(false); }

                    Mission6.transform.position = new Vector3(15.65f, 4.81f, -1.0f);
                    Countdown = true;
                    break;
            }   
        }
    }

    [System.Serializable]
    public class Mission //Class template for Mission
    {
        public string name;
        public string missionText;

        public int damage;
        public int difficulty;
        public int points;

        public bool potholes;
        public bool contstruction;
    }

    [System.Serializable]
    public class MissionList //Class template for Mission List
    {
        public Mission[] mission;
    }

    [System.Serializable]
    public class Config //Class template for config
    {
        public float highscore;
        public string map;
        public float speed;
        public int health;
    }

    [System.Serializable]
    public class ConfigList //Class template for config list.
    {
        public Config[] config;
    }

}
