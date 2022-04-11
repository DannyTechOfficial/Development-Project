using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, EmptyHeart;
    Image hearImage;

    private void Awake() 
    {
        hearImage = GetComponent<Image>();    
    }

    public void SetHeartImage(HearStatus status)
    {
        switch(status)
        {
            case HearStatus.Empty:
                hearImage.sprite = EmptyHeart;
                break;
            case HearStatus.Half:
                hearImage.sprite = halfHeart;
                break;
            case HearStatus.Full:
                hearImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HearStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}
