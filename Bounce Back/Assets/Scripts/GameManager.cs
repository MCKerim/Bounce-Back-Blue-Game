using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    private int score;
    private int highscore;
    private int lives = 3;
    public bool gameOver;
    public bool paused;
    
    [SerializeField] private GameUIHandler gameUIHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;

    [SerializeField] private AudioMixerSnapshot pausedSnapshot;
    [SerializeField] private AudioMixerSnapshot unpausedSnapshot;

    [SerializeField] private GameObject sceneTransition;
    [SerializeField] private AudioSource audioSource;

    private int coinsFromOneRound;

    private float chanceToPlayAd = 0.4f;

    [SerializeField] private GameObject doubleCoinsButton;

    private bool loadScreenIsActive = true;

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);

        LeanTween.scaleY(sceneTransition, 0, 0.5f).setOnComplete(LoadScreenFinished);
        AdManager.current.doubleCoinsAdFinished += DoubleCoinsFromOneRound;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            ContinueGame();
        }
    }
    
    private void LoadScreenFinished()
    {
        loadScreenIsActive = false;
    }

    public void GetDamage(int damage, bool showDamageEffect)
    {
        if (gameOver) { return; }

        lives -= damage;
        if(showDamageEffect)
        {
            audioSource.Play();
            gameUIHandler.DamageEffectIn();
        }
        gameUIHandler.UpdateLivesText(lives);

        if (lives <= 0)
        { 
            GameOver();
        }
        else
        {
            StartCoroutine(cameraController.ScreenShake(0.1f, 0.5f));
        }
    }

    public void CheadButtonGetExtraLives()
    {
        lives += 10;
        gameUIHandler.UpdateLivesText(lives);
    }

    public void CheadButtonGetScore()
    {
        score += 10;
        gameUIHandler.UpdateScoreText(score);
    }

    public void IncreaseScore(int points)
    {
        score += points;
        gameUIHandler.UpdateScoreText(score);
    }

    public void IncreasCoinsFromOneRound(int newCoins)
    {
        coinsFromOneRound += newCoins;
    }

    public void ShowScreenShake()
    {
        StartCoroutine(cameraController.ScreenShake(0.05f, 0.2f));
    }

    public void PauseGame()
    {
        if (paused || gameOver || loadScreenIsActive) { return; }
        paused = true;

        Lowpass(true);
        playerController.AllowInput(false);
        gameUIHandler.ShowPausePanel();
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        if (!paused) { return; }
        paused = false;

        Lowpass(false);
        playerController.AllowInput(true);
        gameUIHandler.HidePausePanel();
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        unpausedSnapshot.TransitionTo(0.01f);
        SaveSystem.current.SaveStats();
        LeanTween.scaleY(sceneTransition, 1.1f, 0.5f).setOnComplete(LoadGame);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenü()
    {
        unpausedSnapshot.TransitionTo(0.01f);
        SaveSystem.current.SaveStats();
        LeanTween.scaleY(sceneTransition, 1.1f, 0.5f).setOnComplete(LoadMainMenü);
    }

    private void LoadMainMenü()
    {
        SceneManager.LoadScene("MainMenü");
    }

    public void GameOver()
    {
        if (gameOver) { return; }
        gameOver = true;

        if (paused) { ContinueGame();}

        StartCoroutine(cameraController.ScreenShake(0.1f, 1f));

        playerController.AllowInput(false);

        playerController.Die();

        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            gameUIHandler.NewHigscore();
        }
        else if(PlayerPrefs.GetInt("noAds", 0) != 1)
        {
            float randomFloat = Random.Range(0f, 1f);
            if(randomFloat <= chanceToPlayAd)
            {
                LeanTween.delayedCall(2f, AdManager.current.PlayInterstitialAd);
            }
        }

        Lowpass(true);

        gameUIHandler.UpdateFinalScoreText(score);
        gameUIHandler.UpdateHighScoreText(highscore);

        if(coinsFromOneRound > 0)
        {
            gameUIHandler.UpdateCoinsText(coinsFromOneRound);

            doubleCoinsButton.SetActive(true);
        }
        else
        {
            doubleCoinsButton.SetActive(false);
        }

        gameUIHandler.ShowGameOverPanel();
    }

    public void DoubleCoinsButonClicked()
    {
        AdManager.current.PlayAd("doubleCoins");
        doubleCoinsButton.SetActive(false);
    }

    public void DoubleCoinsFromOneRound()
    {
        coinsFromOneRound *= 2;
        Debug.Log("coinsFromOneRound: " + coinsFromOneRound);
        gameUIHandler.UpdateCoinsText(coinsFromOneRound);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            PauseGame();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            PauseGame();
        }
    }

    private void Lowpass(bool active)
    {
        if(active)
        {
            pausedSnapshot.TransitionTo(0.01f);
        }
        else
        {
            unpausedSnapshot.TransitionTo(0.01f);
        }
    }

    public int GetScore()
    {
        return score;
    }

    private void OnDestroy()
    {
        AdManager.current.doubleCoinsAdFinished -= DoubleCoinsFromOneRound;
    }

    // HAHA funktioniert
    //private void OnApplicationQuit()
    //{
    //    Application.OpenURL("https://linktr.ee/MCKerim");
    //}
}