using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    //Declaring initial variables & references for health heart
    public Sprite fullHeart, halfHeart, EmptyHeart;
    Image heartImage;

    private void Awake()  //On Awake retrieves component image
    {
        heartImage = GetComponent<Image>();    
    }

    public void SetHeartImage(HeartStatus status) //Sets heart image based on status
    {
        switch(status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = EmptyHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HeartStatus //Enum for heart status
{
    Empty = 0,
    Half = 1,
    Full = 2
}
