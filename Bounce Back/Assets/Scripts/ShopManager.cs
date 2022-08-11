using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<PlayerBall> allPlayerBalls;
    private int currentPlayerBallId = 0;

    private SaveSystem saveSystem;

    [SerializeField] private TextMeshProUGUI playerBallNameText;
    [SerializeField] private TextMeshProUGUI playerBallDescriptionText;
    [SerializeField] private TextMeshProUGUI playerBallPriceText;
    [SerializeField] private Image playerBallImage;
    [SerializeField] private Image playerBallbackground;

    [SerializeField] private TextMeshProUGUI infoText;

    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private LeanTweenType priceTextShake;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color tooExpensiveColor;

    private Vector3 infoTextPos;

    // Start is called before the first frame update
    void Start()
    {
        saveSystem = SaveSystem.current;
        saveSystem.StatsResettet += DisplayPlayerBall;

        AdManager.current.adForCoinsAdFinished += RewardPlayerWithCoins;

        infoTextPos = infoText.transform.position;

        DisplayPlayerBall();
    }

    public void ShowNextPlayerBall()
    {
        currentPlayerBallId++;

        if(currentPlayerBallId > allPlayerBalls.Count-1)
        {
            currentPlayerBallId = 0;
        }

        DisplayPlayerBall();
    }

    public void ShowPreviousPlayerBall()
    {
        currentPlayerBallId--;

        if(currentPlayerBallId < 0)
        {
            currentPlayerBallId = allPlayerBalls.Count-1;
        }

        DisplayPlayerBall();
    }

    private void DisplayPlayerBall()
    {
        PlayerBall current = allPlayerBalls[currentPlayerBallId];

        playerBallNameText.SetText(current.ballName);
        playerBallDescriptionText.SetText(current.description);

        if (current.ballName == "Highscore Ball")
        {
            playerBallPriceText.SetText("Highscore: 320");
        }
        else
        {
            playerBallPriceText.SetText("Price: " + current.price);
        }
        
        playerBallImage.sprite = current.sprite;

        if (saveSystem.GetSelectedPlayerBallSprite() == current.sprite)
        {
            infoText.SetText("Selected");
            infoText.color = selectedColor;
            playerBallbackground.color = selectedColor;
        }
        else if (saveSystem.IsUnlocked(current.ballName))
        {
            infoText.SetText("Unlocked");
            infoText.color = unlockedColor;
            playerBallbackground.color = unlockedColor;
        }
        else
        {
            infoText.SetText("Locked");
            infoText.color = lockedColor;
            playerBallbackground.color = lockedColor;
        }

        coinsText.SetText("Coins: " + saveSystem.GetCoins());
    }

    public void ClickedOnBall()
    {
        if(saveSystem.IsUnlocked(allPlayerBalls[currentPlayerBallId].ballName))
        {
            SelectPlayerBall();
        }
        else if(allPlayerBalls[currentPlayerBallId].ballName == "Highscore Ball")
        {
            if(PlayerPrefs.GetInt("highscore") >= 320)
            {
                saveSystem.Unlock(allPlayerBalls[currentPlayerBallId].ballName);
                infoText.SetText("Unlocked");
                infoText.color = unlockedColor;
                playerBallbackground.color = unlockedColor;
                SelectPlayerBall();
            }
            else
            {
                //<size=80>
                infoText.SetText("Highscore Not High Enough");
                infoText.color = tooExpensiveColor;
                playerBallbackground.color = tooExpensiveColor;

                if (!LeanTween.isTweening(infoText.gameObject))
                {
                    LeanTween.moveX(infoText.gameObject, infoTextPos.x + 0.1f, 0.5f).setEase(priceTextShake);
                    LeanTween.moveX(infoText.gameObject, infoTextPos.x - 0.1f, 0.5f).setEase(priceTextShake).setOnComplete(ResetInfoTextPos);
                }
            }
        }
        else if(saveSystem.GetCoins() >= allPlayerBalls[currentPlayerBallId].price)
        {
            BuyPlayerBall();
            SelectPlayerBall();
        }
        else
        {
            infoText.SetText("Not Enough Coins");
            infoText.color = tooExpensiveColor;
            playerBallbackground.color = tooExpensiveColor;

            if(!LeanTween.isTweening(infoText.gameObject))
            {
                LeanTween.moveX(infoText.gameObject, infoTextPos.x + 0.1f, 0.5f).setEase(priceTextShake);
                LeanTween.moveX(infoText.gameObject, infoTextPos.x - 0.1f, 0.5f).setEase(priceTextShake).setOnComplete(ResetInfoTextPos);
            }
        }
    }

    private void ResetInfoTextPos()
    {
        infoText.transform.position = infoTextPos;
    }

    public void BuyPlayerBall()
    {
        saveSystem.ChangeCoins(-allPlayerBalls[currentPlayerBallId].price);
        coinsText.SetText("Coins: " + saveSystem.GetCoins());
        saveSystem.Unlock(allPlayerBalls[currentPlayerBallId].ballName);

        infoText.SetText("Unlocked");
        infoText.color = unlockedColor;
        playerBallbackground.color = unlockedColor;
    }

    public void SelectPlayerBall()
    {
        saveSystem.SelectPlayerBall(allPlayerBalls[currentPlayerBallId].ballName);

        infoText.SetText("Selected");
        infoText.color = selectedColor;
        playerBallbackground.color = selectedColor;
    }

    public void AdForCoinsButtonClicked()
    {
        AdManager.current.PlayAd("adForCoins");
    }

    public void RewardPlayerWithCoins()
    {
        saveSystem.ChangeCoins(20);
        coinsText.SetText("Coins: " + saveSystem.GetCoins());
    }

    private void OnDestroy()
    {
        saveSystem.StatsResettet -= DisplayPlayerBall;

        AdManager.current.adForCoinsAdFinished -= RewardPlayerWithCoins;
    }
}
