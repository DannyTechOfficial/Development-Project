using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Start : MonoBehaviour
{
    public Frontline_Manager fManager;
    public UnityEngine.UI.Text MenuTitle;
    int sceneCount;
    string[] scenes;
    // Start is called before the first frame update
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = SceneUtility.GetScenePathByBuildIndex(i);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("escape"))
        {
            if(SceneManager.GetActiveScene().name == "Start Menu")
            {
                Application.Quit();
            } 
            else
            {
                SceneManager.LoadScene("Start Menu");
            }
            
        }
    }
    
    public void StartLevel() 
    {
        foreach (string scene in scenes)
        {
            Debug.Log(scene);
            if (scene.Contains(fManager.configList.config[0].map + ".unity"))
            {
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log(scene);

            }

        }
        MenuTitle.text = "Map Unavailable - Please Change Config File!";
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
