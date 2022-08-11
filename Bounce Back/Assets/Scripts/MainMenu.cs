using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private CanvasGroup currentPanel;
    private CanvasGroup newPanel;

    [SerializeField] private CanvasGroup startPanel;

    [SerializeField] private LeanTweenType alphaFade;

    [SerializeField] private GameObject resetStatsPanel;
    [SerializeField] private LeanTweenType resetStatsScaleIn;
    [SerializeField] private LeanTweenType resetStatsScaleOut;

    [SerializeField] private GameObject resetTutorialPanel;

    [SerializeField] private GameObject sceneTransition;

    [SerializeField] private GameObject noAdsButton;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        currentPanel = startPanel;

        if(PlayerPrefs.GetInt("noAds", 0) == 0)
        {
            noAdsButton.SetActive(true);
        }
        else
        {
            noAdsButton.SetActive(false);
        }

        LeanTween.scaleY(sceneTransition, 0, 0.5f);
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentPanel != startPanel)
        {
            OpenPanel(startPanel);
        }
    }

    public void StartGame()
    {
        SaveSystem.current.SaveStats();
        LeanTween.scaleY(sceneTransition, 1.1f, 0.5f).setOnComplete(LoadGame);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenPanel(CanvasGroup panel)
    {
        currentPanel.blocksRaycasts = false;
        newPanel = panel;
        FadeOut();
    }

    private void FadeOut()
    {
        LeanTween.alphaCanvas(currentPanel, 0f, 0.5f).setEase(alphaFade).setOnComplete(Activate);
    }

    private void Activate()
    {
        currentPanel.gameObject.SetActive(false);
        currentPanel = newPanel;
        currentPanel.gameObject.SetActive(true);

        LeanTween.alphaCanvas(newPanel, 1f, 0.5f).setEase(alphaFade).setDelay(0.1f).setOnComplete(MakeInteractable);
    }

    private void MakeInteractable()
    {
        currentPanel.blocksRaycasts = true;
    }

    public void CheatCoins()
    {
        SaveSystem.current.CheatCoins();
    }

    public void unlockEnemy()
    {
        SaveSystem.current.EnemyDiscovered("Enemy");
    }

    public void Save()
    {
        SaveSystem.current.SaveStats();
    }

    public void ClearError()
    {
        SaveSystem.current.ClearErrorText();
    }

    public void ShowResetStatsPanel()
    {
        resetStatsPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        resetStatsPanel.SetActive(true);
        LeanTween.scale(resetStatsPanel, new Vector3(1, 1, 1), 1f).setEase(resetStatsScaleIn);
    }

    public void ResetStats()
    {
        SaveSystem.current.ResetStats();
        PlayerPrefs.SetInt("highscore", 0);
        HideResetStatsPanel();
    }

    public void HideResetStatsPanel()
    {
        LeanTween.scale(resetStatsPanel, new Vector3(0.1f, 0.1f, 0.1f), 1f).setEase(resetStatsScaleOut).setOnComplete(DeactivateResetStatsPanel);
    }

    private void DeactivateResetStatsPanel()
    {
        resetStatsPanel.SetActive(false);
    }

    public void ShowResetTutorialPanel()
    {
        resetTutorialPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        resetTutorialPanel.SetActive(true);
        LeanTween.scale(resetTutorialPanel, new Vector3(1, 1, 1), 1f).setEase(resetStatsScaleIn);
    }

    public void ResetTutorialProgress()
    {
        SaveSystem.current.ResetDiscoveredEnemys();
        HideResetTutorialPanel();
    }

    public void HideResetTutorialPanel()
    {
        LeanTween.scale(resetTutorialPanel, new Vector3(0.1f, 0.1f, 0.1f), 1f).setEase(resetStatsScaleOut).setOnComplete(DeactivateResetTutorialPanel);
    }

    private void DeactivateResetTutorialPanel()
    {
        resetTutorialPanel.SetActive(false);
    }
}
