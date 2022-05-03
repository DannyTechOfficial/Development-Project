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
    public GameObject frontline_Manager;
    //Blank references to Missions & Config
    public MissionList missionList = new MissionList();
    public ConfigList configList = new ConfigList();
    public GameObject Player;
    public GameObject playerScoreObject, GameOverObject, MissionTitleObject, MissionTextObject;
    public float moveSpeed, multiplierSpeed, maxScore;

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
    //public Collisions C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12; 
        
    // Start is called before the first frame update
    void Start()
    {
        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        print("Streaming Assets Path: " + Application.streamingAssetsPath);
        allFiles = directoryInfo.GetFiles("*.*");
        if (SceneManager.GetActiveScene().name == "Start Menu")
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
        else
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

            loadMission();
            /*
            C1 = Mission1.GetComponent<Collisions>();
            C2 = Mission2.GetComponent<Collisions>();;
            C3 = Mission3.GetComponent<Collisions>();;
            C4 = Mission4.GetComponent<Collisions>();;
            C5 = Mission5.GetComponent<Collisions>();;
            C6 = Mission6.GetComponent<Collisions>();

            C7 = Pothole1.GetComponent<Collisions>();
            C8 = Pothole2.GetComponent<Collisions>();
            C9 = Pothole3.GetComponent<Collisions>();
            C10 = RClosed1.GetComponent<Collisions>();
            C11 = RClosed2.GetComponent<Collisions>();
            C12 = RClosed3.GetComponent<Collisions>();
            */


        }
    }

    // Update is called once per frame
    void Update()
    {
        PController.moveSpeed = moveSpeed;
        PController.health = Health;
        PController.dead = isDead;
        if (Convert.ToInt32(playerScore.text) >= maxScore)
        {
            GameOverText.text = "Your reached the Max Score !   -   Well Done ! !";
        }
    }

    void FixedUpdate()
    {
        if (Countdown)
        {
            MissionTitle.text = missionList.mission[activeMission].name;
            MissionMessage.text = missionList.mission[activeMission].missionText;
            seconds += 1;
        }
        if (seconds == 800)
        {
            Countdown = false;
            seconds = 0;
            MissionTitle.text = null;
            MissionMessage.text = null;
        }
        if (isDead)
        {
            moveSpeed = 0;
            GameOverText.text = "You Crashed The Ambulance ! - Press ESC to go to menu.";
        }
        if (SceneManager.GetActiveScene().name == "Start Menu")
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

    public void reachedMission(Collider2D other)
    {
        other.gameObject.transform.Translate(0.0f, 50.0f, 0.0f);
        intPlayerScore += other.gameObject.GetComponent<Collisions>().points;
        playerScore.text = Convert.ToString(intPlayerScore);
        i += 1;
        activeMission += 1;
        //moveSpeed += 1.5f;
        if (activeMission < missionList.mission.Length) loadMission(); else {activeMission = 0; loadMission();}
    }
    public void loadMission()
    {
        Debug.Log("LOADING MISSION !");
        randomNumber = Random.Range(1,13);

        multiplierSpeed =  missionList.mission[activeMission].difficulty;

        if(int.Parse(playerScore.text) < maxScore)
        {
            
            switch (randomNumber)
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
    public class Mission
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
    public class MissionList
    {
        public Mission[] mission;
    }

    [System.Serializable]
    public class Config
    {
        public float highscore;
        public string map;
        public float speed;
        public int health;
    }

    [System.Serializable]
    public class ConfigList
    {
        public Config[] config;
    }

}
