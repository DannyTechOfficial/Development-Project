using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //Declaring initial variables & references for health bar
    public GameObject HeartPrefab;
    public Frontline_Manager fManager;
    public Ambulance_Controller aController;
    List<HealthHeart> hearts = new List<HealthHeart>(); //Creates instance of list HeathHeart

    private void OnEnable() { //On enable redraws hearts and monitors health
        Ambulance_Controller.onPlayerHit += DrawHearts;
        Debug.Log("Event Triggered1");
    }

    private void OnDisable() { //On disable removes hearts and stops monitoring heart health
        Ambulance_Controller.onPlayerHit -= DrawHearts; 
        Debug.Log("Event Triggered2");   
    }

    public void Start() //When started hearts are drawn on screen
    {
        DrawHearts();
    }

    public void DrawHearts() //Draw hearts function
    {
        ClearHearts(); //Clears existing hearts

        //How many hearts needed
        float maxHealthRemainder = fManager.maxHealth % 2;
        int heartsToMake = (int)((fManager.maxHealth / 2) + maxHealthRemainder);
        for(int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(fManager.Health - (i*2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart() //Creates empty heart
    {
        GameObject newHeart = Instantiate(HeartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts() //Clears hearts off screen
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

}
