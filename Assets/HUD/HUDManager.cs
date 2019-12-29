using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text livesCount;
    public Image reaperBackground;
    public Gradient reaperGradient;
    Color reaperBackgroundColor;

    public Text feedIndicator;
    public Image stomach;
    public Sprite stomachHappy;
    public Sprite stomachSad;
    public Image stomachBackground;
    public Gradient stomachGradient;
    Color stomachBackgroundColor;

    public Text staminaIndicator;
    public Image staminaBackground;
    public Image stamina;
    public Gradient staminaGradient;
    Color staminaBackgroundColor;

    public Text score;

    private void Awake()
    {
        PlayerEvents.Singleton.RegisterFeedLevelChangedActons(UpdateFeedLevelIndicator);
        PlayerEvents.Singleton.RegisterFeedLevelChangedActons(UpdateStomachSprite);
        PlayerEvents.Singleton.RegisterFeedLevelChangedActons(UpdateStomachBackgroundText);

        PlayerEvents.Singleton.RegisterLifeRemovedActions(UpdateLivesCountText);
        PlayerEvents.Singleton.RegisterPlayerDiedActions(UpdateLivesCountText);

        PlayerEvents.Singleton.RegisterStaminaChangedActions(UpdateStaminaIndicator);
        PlayerEvents.Singleton.RegisterStaminaChangedActions(UpdateStaminaBackgroundImage);

        PlayerEvents.Singleton.RegisterScoreChangedActions(UpdateScore);
    }

    #region Score
    void UpdateScore()
    {
        score.text = PlayerStates.Singleton.Score.ToString();
    }
    #endregion

    #region Stamina
    void UpdateStaminaIndicator()
    {
        staminaIndicator.text = string.Format("{0}%", Mathf.Round(PlayerStates.Singleton.Stamina * 100));
    }

    void UpdateStaminaBackgroundImage()
    {
        staminaBackgroundColor = staminaGradient.Evaluate(PlayerStates.Singleton.Stamina);
        staminaBackground.color = staminaBackgroundColor;
    }
    #endregion

    #region Lives
    void UpdateLivesCountText()
    {
        livesCount.text = PlayerStates.Singleton.GetLives().ToString();
        reaperBackgroundColor = reaperGradient.Evaluate((1.0f / PlayerStates.Singleton.GetMaxLives()) * PlayerStates.Singleton.GetLives());
        reaperBackground.color = reaperBackgroundColor;
    }
    #endregion

    #region Stomach
    void UpdateStomachBackgroundText()
    {
        stomachBackgroundColor = stomachGradient.Evaluate(PlayerStates.Singleton.FeedLevel);
        stomachBackground.color = stomachBackgroundColor;
    }

    void UpdateFeedLevelIndicator()
    {
        feedIndicator.text = string.Format("{0}%", Mathf.Round(PlayerStates.Singleton.FeedLevel * 100));
    }

    void UpdateStomachSprite()
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
    #endregion
}
