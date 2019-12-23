using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text feedLevelIndicator;

    public Image stomach;
    public Sprite stomachHappy;
    public Sprite stomachSad;

    private void Start()
    {
        UpdateFeedLevelIndicator();
        PlayerEvents.Singleton.RegisterFeedLevelChangedActons(UpdateFeedLevelIndicator);
        PlayerEvents.Singleton.RegisterFeedLevelChangedActons(UpdateStomachImage);
    }

    void UpdateFeedLevelIndicator()
    {
        feedLevelIndicator.text = string.Format("{0}%", Mathf.Round(PlayerStates.Singleton.FeedLevel * 100));

    }

    void UpdateStomachImage()
    {
        if (PlayerStates.Singleton.FeedLevel < PlayerStates.Singleton.LowFeedLevel && stomach.sprite != stomachSad)
        {
            stomach.sprite = stomachSad;
        }
        else if (PlayerStates.Singleton.FeedLevel >= PlayerStates.Singleton.LowFeedLevel && stomach.sprite != stomachHappy)
        {
            stomach.sprite = stomachHappy;
        }
    }
}
