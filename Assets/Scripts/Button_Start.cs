using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Start : MonoBehaviour
{
    //Sets properties for Button_Start
    public Frontline_Manager fManager;
    public UnityEngine.UI.Text MenuTitle;
    int sceneCount;
    string[] scenes;

    // Start is called before the first frame update
    void Start()
    {
        //Retrieves all scenes information
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
        if(Input.GetKey("escape")) //If escape key is pressed
        {
            if(SceneManager.GetActiveScene().name == "Start Menu") //If scene is Main Menu then quit game.
            {
                Application.Quit();
            } 
            else //Else loads the start menu.
            {
                SceneManager.LoadScene("Start Menu");
            }
            
        }
    }
    
    public void StartLevel() 
    {
        foreach (string scene in scenes) //Filters through all scenes to check which one is specified in the configuration file.
        {
            Debug.Log(scene);
            if (scene.Contains(fManager.configList.config[0].map + ".unity")) 
            {
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log(scene); //Logs scene name if not available.

            }

        }
        MenuTitle.text = "Map Unavailable - Please Change Config File!";
    }

    public void ExitGame() //Quits application when selected on main menu
    {
        Application.Quit();
    }

}
