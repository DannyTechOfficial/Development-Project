using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using UnityEngine.AddressableAssets;


using System;

public class Frontline_Manager : MonoBehaviour
{
    //public TextAsset textJSON;
    public MissionList missionList = new MissionList();
    public GameObject Player;
    public GameObject playerScoreObject, GameOverObject, MissionTitleObject, MissionTextObject;
    public float moveSpeed, multiplierSpeed;

    public int Health, maxHealth, maxScore;
    public bool isDead;
    public int Difficulty;

    public bool PotholeSpawn;
    public bool TrafficSpawn;
    public bool RoadCloseSpawn;

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
    public GameObject Collider1, Collider2, Collider3, Collider4, Collider5, Collider6, Collider7, Collider8, Collider9, Collider10, Collider11, Collider12;
    public Collisions C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12; 
        
    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        print("Streaming Assets Path: " + Application.streamingAssetsPath);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
        foreach (FileInfo file in allFiles)
        {
            if (file.Name.Contains("Missions"))
            {
                if (file.Name == "Missions.json.meta") break;
                Debug.Log("MISSIONS FOUND !!!");
                data = LoadJSONFile(file).ToString();
                Debug.Log(file.Directory.ToString() + "/" + file.Name);
                string dataAsJSON = File.ReadAllText(file.Directory.ToString() +"/" + file.Name);
                missionList = JsonUtility.FromJson<MissionList>(dataAsJSON);
                Debug.Log(missionList.mission[0]);
            }
        }
        Health = maxHealth;
        //missionList = JsonUtility.FromJson<MissionList>(textJSON.text);
        //Implementation of Story from JSON needs implementing.
        PController = Player.GetComponent<Ambulance_Controller>();
        playerScore = playerScoreObject.GetComponent<UnityEngine.UI.Text>();
        GameOverText = GameOverObject.GetComponent<UnityEngine.UI.Text>();
        MissionTitle = MissionTitleObject.GetComponent<UnityEngine.UI.Text>();
        MissionMessage = MissionTextObject.GetComponent<UnityEngine.UI.Text>();
        
        missionCount = (missionList.mission.Length);
        
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
    IEnumerator LoadJSONFile(FileInfo file)
    {
        string wwwColourPath = "file://" + file.FullName.ToString();
        WWW www = new WWW(wwwColourPath);
        missionList = JsonUtility.FromJson<MissionList>(www.text);
        yield return www;
        Debug.Log(missionList.mission[0]);
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
        if (isDead)
        {
            moveSpeed = 0;
            GameOverText.text = "You Crashed The Ambulance ! - Press ESC to go to menu.";
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
    IEnumerator inTime(float time)
    {
        yield return new WaitForSeconds(time);
        MissionTitle.text = null;
        MissionMessage.text = null;
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
                    Collider1.transform.position = new Vector3(-15.61f, 11.92f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));

                    break;
                case 2:
                    Collider2.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 3:
                    Collider3.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 4:
                    Collider4.transform.position = new Vector3(0f, 0f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 5:
                    Collider5.transform.position = new Vector3(15.89f, -7.45f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 6:
                    Collider6.transform.position = new Vector3(0f, 0f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 7:
                    Collider1.transform.position = new Vector3(-15.61f, -5.95f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));

                    break;
                case 8:
                    Collider2.transform.position = new Vector3(-11.6f, 1.5f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 9:
                    Collider3.transform.position = new Vector3(12.35f, 8.78f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 10:
                    Collider4.transform.position = new Vector3(8.37f, -8.57f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 11:
                    Collider5.transform.position = new Vector3(-0.26f, -2.98f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
                    break;
                case 12:
                    Collider6.transform.position = new Vector3(15.65f, 4.81f, -1.0f);
                    MissionTitle.text = missionList.mission[activeMission].name;
                    MissionMessage.text = missionList.mission[activeMission].missionText;
                    StartCoroutine(inTime(8));
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
    }

    [System.Serializable]
    public class MissionList
    {
        public Mission[] mission;
    }

}
