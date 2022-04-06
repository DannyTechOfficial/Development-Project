using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frontline_Reader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class Mission
    {
        public string name;
        public string missionText;

        public int damage;
        public int durability;
        public int time;
        public int points;
    }

    [System.Serializable]
    public class MissionList
    {
        public Mission[] mission;
    }

    public MissionList missionList = new MissionList();

    // Start is called before the first frame update
    void Start()
    {
        missionList = JsonUtility.FromJson<MissionList>(textJSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
