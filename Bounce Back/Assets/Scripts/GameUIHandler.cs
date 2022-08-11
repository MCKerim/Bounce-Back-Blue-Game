using UnityEngine;
using TMPro;
using System.Collections;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    public LeanTweenType gameOverFadeIn;

    [SerializeField] private TextMeshProUGUI scoreText;
    private GameObject scoreTextObject;
    public LeanTweenType scoreScaleIn;
    public LeanTweenType scoreScaleOut;

    [SerializeField] private TextMeshProUGUI livestext;
    private GameObject livestextObject;

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject finalScoreTextObject;
    public LeanTweenType finalScoreScaleIn;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject highScoreTextObject;
    public LeanTweenType highscoreScaleIn;

    [SerializeField] private GameObject newHighScoreTextObject;
    public LeanTweenType newHighscoreScaleIn;
    [SerializeField] private GameObject newHighscoreParticle1;
    [SerializeField] private GameObject newHighscoreParticle2;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private GameObject coinsTextObject;
    public LeanTweenType coinsTextScaleIn;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private CanvasGroup damageEffectCanvas;
    public LeanTweenType damageInEase;
    public LeanTweenType damageOutEase;

    [SerializeField] private AudioSource audioSource;

    private string developerCode = "Kachelofen";
    [SerializeField] private GameObject developerSettingsPanel;

    private bool showCoinsText;

    [SerializeField] private GameObject difficultyTextObject;
    public LeanTweenType difficultyTextScaleIn;
    public LeanTweenType difficultyTextFadeOut;
    public LeanTweenType difficultyTextMoveY;

    private int coinsTextCurrentValue;
    private int coinsTextTargetValue;

    private float finalScoreTextCurrentValue;
    private float finalScoreTextTargetValue;

    [SerializeField] private AnimationCurve coinsTextAnimCurve;

    // Start is called before the first frame update
    void Start()
    {
        scoreTextObject = scoreText.gameObject;
        livestextObject = livestext.gameObject;
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.SetText("Score: " + newScore);

        if(!LeanTween.isTweening(scoreTextObject))
        {
            LeanTween.scale(scoreTextObject, new Vector3(1.5f, 1.5f, 1), 0.25f).setEase(scoreScaleIn).setOnComplete(ScaleScoreTextBack);
        }
    }

    private void ScaleScoreTextBack()
    {
        LeanTween.scale(scoreTextObject, new Vector3(1f, 1f, 1), 0.5f).setEase(scoreScaleOut).setDelay(0.125f);
    }

    public void UpdateLivesText(int newLives)
    {
        livestext.SetText("Lives: " + newLives);

        if(!LeanTween.isTweening(livestextObject))
        {
            LeanTween.scale(livestextObject, new Vector3(1.5f, 1.5f, 1), 0.25f).setEase(scoreScaleIn).setOnComplete(ScaleLivesTextBack);
        }
    }

    public void OpenPanel(GameObject newPanel)
    {
        pausePanel.SetActive(false);
        newPanel.SetActive(true);
    }

    public void EnterDeveloperCode(string code)
    {
        if(code == developerCode)
        {
            Debug.Log("Code true");
            OpenPanel(developerSettingsPanel);
        }
        else
        {
            Debug.Log("wrong code");
        }
    }

    public void GoBackToPausePanel(GameObject newPanel)
    {
        newPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void ScaleLivesTextBack()
    {
        LeanTween.scale(livestextObject, new Vector3(1f, 1f, 1), 0.5f).setEase(scoreScaleOut).setDelay(0.125f);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void GiveUp()
    {
        HidePausePanel();
        gameManager.GameOver();
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        gameOverPanel.SetActive(true);
        LeanTween.alphaCanvas(gameOverPanel.GetComponent<CanvasGroup>(), 1f, 2).setEase(gameOverFadeIn).setDelay(1).setOnComplete(MakeGameOverInteractable);
        LeanTween.scale(finalScoreTextObject, new Vector3(1, 1, 1), 1f).setEase(finalScoreScaleIn).setDelay(1.3f);
        LeanTween.scale(highScoreTextObject, new Vector3(1, 1, 1), 1f).setEase(highscoreScaleIn).setDelay(2f);

        if(showCoinsText)
        {
            LeanTween.scale(coinsTextObject, new Vector3(1, 1, 1), 1f).setEase(coinsTextScaleIn).setDelay(1.5f);
        }
    }

    private void MakeGameOverInteractable()
    {
        gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void UpdateFinalScoreText(int finalScore)
    {
        //finalScoreText.SetText("Score: " + finalScore);

        finalScoreText.SetText("Score: " + finalScoreTextCurrentValue);
        finalScoreTextTargetValue = finalScore;

        StartCoroutine(FinalScoreTextAnim());
    }

    IEnumerator FinalScoreTextAnim()
    {
        yield return new WaitForSeconds(1);

        while (finalScoreTextCurrentValue < finalScoreTextTargetValue)
        {
            float x = finalScoreTextCurrentValue / finalScoreTextTargetValue;
            float waitTime = coinsTextAnimCurve.Evaluate(x);
            waitTime *= 1;
            yield return new WaitForSeconds(waitTime);

            finalScoreTextCurrentValue++;
            finalScoreText.SetText("Score: " + finalScoreTextCurrentValue);
        }
    }

    public void UpdateHighScoreText(int highscore)
    {
        highScoreText.SetText("Highscore: " + highscore);
    }

    public void UpdateCoinsText(int newCoins)
    {
        //StopCoroutine("CoinsTextAnim");

        //coinsText.SetText("Coins: +" + coinsTextCurrentValue);
        //coinsTextTargetValue = newCoins;

        //showCoinsText = true;

        //StartCoroutine("CoinsTextAnim");

        StopCoroutine("CoinsTextAnim3");

        coinsText.SetText("Coins: +" + coinsTextCurrentValue);
        coinsTextTargetValue = newCoins;

        showCoinsText = true;

        StartCoroutine("CoinsTextAnim3");
    }

    IEnumerator CoinsTextAnim()
    {
        yield return new WaitForSeconds(1);

        while (coinsTextCurrentValue < coinsTextTargetValue)
        {
            yield return new WaitForSeconds(0.1f);
            coinsTextCurrentValue++;
            coinsText.SetText("Coins: +" + coinsTextCurrentValue);
            Debug.Log("current: " + coinsTextCurrentValue + " Target: " + coinsTextTargetValue);
        }
    }

    IEnumerator CoinsTextAnim2()
    {
        yield return new WaitForSeconds(1);
        for(float t=0; t <= 1f; t += 0.05f)
        {
            float y = coinsTextAnimCurve.Evaluate(t);
            float newValue = coinsTextTargetValue * y;
            coinsTextCurrentValue = (int) newValue;

            coinsText.SetText("Coins: +" + coinsTextCurrentValue);
            Debug.Log("current: " + coinsTextCurrentValue + " Target: " + coinsTextTargetValue);

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CoinsTextAnim3()
    {
        yield return new WaitForSeconds(1);

        while (coinsTextCurrentValue < coinsTextTargetValue)
        {
            float x = (float)coinsTextCurrentValue / coinsTextTargetValue;

            float waitTime = coinsTextAnimCurve.Evaluate(x);
            waitTime *= 1;
            yield return new WaitForSeconds(waitTime);

            coinsTextCurrentValue++;
            coinsText.SetText("Coins: +" + coinsTextCurrentValue);
            Debug.Log("Current: " + coinsTextCurrentValue + " Target: " + coinsTextTargetValue);
            Debug.Log("Wait Time: " + waitTime);
        }
    }

    public void ShowDifficultyText()
    {
        difficultyTextObject.transform.localPosition = new Vector3(0, 150, 0);
        difficultyTextObject.transform.localScale = new Vector3(0, 0, 0);

        LeanTween.scale(difficultyTextObject, new Vector3(1, 1, 1), 0.5f).setEase(difficultyTextScaleIn);
        LeanTween.alphaCanvas(difficultyTextObject.GetComponent<CanvasGroup>(), 1, 0.5f).setEase(difficultyTextFadeOut);

        LeanTween.moveLocalY(difficultyTextObject, difficultyTextObject.transform.localPosition.y + 500, 2f).setEase(difficultyTextMoveY).setDelay(0.1f);
        LeanTween.alphaCanvas(difficultyTextObject.GetComponent<CanvasGroup>(), 0, 1.5f).setEase(difficultyTextFadeOut).setDelay(0.5f);
    }

    public void NewHigscore()
    {
        LeanTween.scale(newHighScoreTextObject, new Vector3(1, 1, 1), 1f).setEase(highscoreScaleIn).setDelay(2.5f);
        LeanTween.delayedCall(2.7f, ShowHigscoreParticle1);
        LeanTween.delayedCall(3f, ShowHigscoreParticle2);

        LeanTween.delayedCall(3.5f, ShowHigscoreParticle1);
        LeanTween.delayedCall(3.8f, ShowHigscoreParticle2);
    }

    private void ShowHigscoreParticle1()
    {
        newHighscoreParticle1.GetComponent<ParticleSystem>().Play();
        var audioSource = newHighscoreParticle1.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    private void ShowHigscoreParticle2()
    {
        newHighscoreParticle2.GetComponent<ParticleSystem>().Play();
        var audioSource = newHighscoreParticle2.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    public void DamageEffectIn()
    {
        LeanTween.alphaCanvas(damageEffectCanvas, 1f, 0.1f).setEase(damageInEase).setOnComplete(DamageEffectOut);
    }

    public void DamageEffectOut()
    {
        LeanTween.alphaCanvas(damageEffectCanvas, 0f, 0.3f).setEase(damageOutEase).setDelay(0.2f);
    }
}
