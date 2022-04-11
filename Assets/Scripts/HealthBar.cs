using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject HeartPrefab;
    public Frontline_Manager fManager;
    public Ambulance_Controller aController;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable() {
        Ambulance_Controller.onPlayerHit += DrawHearts;
        Debug.Log("Event Triggered1");
    }

    private void OnDisable() {
        Ambulance_Controller.onPlayerHit -= DrawHearts; 
        Debug.Log("Event Triggered2");   
    }

    public void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

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
            hearts[i].SetHeartImage((HearStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(HeartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HearStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

}
