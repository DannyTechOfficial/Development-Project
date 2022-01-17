using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
    
    public void StartLevel(string scenename) 
    {
        SceneManager.LoadScene(scenename);
    }

    public void ExitGane()
    {
        Application.Quit();
    }
}
