using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadScreenManager : MonoBehaviour
{
    public Text gameOverText;
    public Image whiteScreenOverlay;

    string[] catFacts =
    {
        "A cat’s nose is as unique as a human’s fingerprint.",
        "A cat cannot see directly under its nose.",
        "Cats can rotate their ears 180 degrees."
    };

    private float alpha = 0f;
    private float alphaText = 0f;

    void Start()
    {
        PlayerEvents.Singleton.RegisterPlayerDiedActions(SetGameOverText);
        PlayerEvents.Singleton.RegisterPlayerDiedActions(InvokeWhiteOverlay);
        PlayerEvents.Singleton.RegisterPlayerDiedActions(InvokeGameOverText);
    }
    void SetGameOverText()
    {
        gameOverText.text = catFacts[Random.Range(0, catFacts.Length)];
    }

    void InvokeWhiteOverlay()
    {
        InvokeRepeating("ShowWhiteScreenOverlay", 0.25f, 0.00005f);
    }
    void InvokeGameOverText()
    {
        InvokeRepeating("ShowGameOverText", 0.6f, 0.00005f);
    }

    void ShowWhiteScreenOverlay()
    {
        alpha += 0.002f;
        whiteScreenOverlay.color = new Color(whiteScreenOverlay.color.r, whiteScreenOverlay.color.g, whiteScreenOverlay.color.b, alpha);

        if (alpha >= 1f)
        {
            CancelInvoke("ShowWhiteScreenOverlay");
        }
    }

    void ShowGameOverText()
    {
        alphaText += 0.002f;
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alphaText);

        if (alphaText >= 1f)
        {
            CancelInvoke("ShowGameOver");
        }
    }
}
