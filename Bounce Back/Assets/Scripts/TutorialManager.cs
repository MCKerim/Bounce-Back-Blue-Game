using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject tutorialCircle;
    [SerializeField] private GameObject standCircles;
    [SerializeField] private GameObject currentStandCircle;

    [SerializeField] private GameObject tutorialArrow;

    [SerializeField] private GameObject tutorialTabAnimation1;
    [SerializeField] private GameObject tutorialTabAnimation2;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Spawner spawner;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextBoxManager textBox;

    private string currentTutorialName = "";
    private GameObject currentEnemyObject;

    private int tutorialPart;

    private bool tutorialIsRunning;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("tutorialIsOn", 1) == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        textBox.TextBoxClosed += TutorialFinished;
        textBox.NextButtonClicked += ShowNextTutorialPart;

        CheckIfEnemyNeedsTutorial("Controlls", null);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorialName == null || tutorialIsRunning)
        {
            return;
        }

        //Wenn Enemy Destroyed wird bevor erklärt
        if (currentEnemyObject == null && currentTutorialName != "Controlls")
        {
            currentTutorialName = null;
            tutorialIsRunning = false;
        }

        if (currentTutorialName == "Controlls")
        {
            tutorialCircle.transform.position = playerController.currentStand.transform.position;
            tutorialArrow.transform.position = playerController.currentStand.transform.position;
            tutorialCircle.SetActive(true);
            tutorialArrow.SetActive(true);

            StartCoroutine(StartSection(2));
        }
        else if (currentTutorialName == "Enemy")
        {
            if (currentEnemyObject.transform.position.y <= 4)
            {
                StopAllEnemys();
                tutorialCircle.transform.position = currentEnemyObject.transform.position;
                tutorialCircle.SetActive(true);

                StartCoroutine(StartSection(0));
            }
        }
        else if (currentTutorialName == "Wall")
        {
   
            if (currentEnemyObject.transform.position.y <= 4)
            {
                StopAllEnemys();
                tutorialCircle.transform.position = currentEnemyObject.transform.position;
                tutorialCircle.SetActive(true);
                StartCoroutine(StartSection(0));
            }
        }
        else if (currentTutorialName == "Enemy Wall Mix")
        {
            if (currentEnemyObject.transform.position.y <= 4)
            {
                StopAllEnemys();
                tutorialCircle.transform.position = currentEnemyObject.transform.position;
                tutorialCircle.SetActive(true);
                StartCoroutine(StartSection(0));
            }
        }
        else if (currentTutorialName == "Not Touch Enemy")
        {
            if (currentEnemyObject.transform.position.y <= 4)
            {
                StopAllEnemys();
                tutorialCircle.transform.position = currentEnemyObject.transform.position;
                tutorialCircle.SetActive(true);
                StartCoroutine(StartSection(0));
            }
        }
        else if (currentTutorialName == "Coin")
        {
            if (currentEnemyObject.transform.position.y <= 4)
            {
                StopAllEnemys();
                tutorialCircle.transform.position = currentEnemyObject.transform.position;
                tutorialCircle.SetActive(true);
                StartCoroutine(StartSection(0));
            }
        }
        else if (currentTutorialName == "Extra Live")
        {
            if (currentEnemyObject.transform.position.y <= 4)
            {
                StopAllEnemys();
                tutorialCircle.transform.position = currentEnemyObject.transform.position;
                tutorialCircle.SetActive(true);
                StartCoroutine(StartSection(0));
            }
        }
    }

    public void CheckIfEnemyNeedsTutorial(string newSpawnedEnemyName, GameObject newSpawnedEnemyObject)
    {
        if (!SaveSystem.current.CheckIfEnemyIsDiscovered(newSpawnedEnemyName))
        {
            currentTutorialName = newSpawnedEnemyName;
            currentEnemyObject = newSpawnedEnemyObject;
        }
    }

    private void StopAllEnemys()
    {
        GameObject[] allEnemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject currentEnemy in allEnemys)
        {
            currentEnemy.GetComponent<IEnemyMovement>().StopMoving();
        }
    }

    private void StartAllEnemys()
    {
        GameObject[] allEnemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject currentEnemy in allEnemys)
        {
            currentEnemy.GetComponent<IEnemyMovement>().Startmoving();
        }
    }

    private void ShowNextTutorialPart()
    {
        tutorialPart++;
        if (currentTutorialName == "Wall" && tutorialPart == 1)
        {
            standCircles.SetActive(false);
            tutorialCircle.SetActive(false);
            tutorialArrow.SetActive(false);
            currentStandCircle.transform.position = playerController.currentStand.transform.position;
            currentStandCircle.SetActive(true);
            tutorialTabAnimation1.SetActive(false);
            tutorialTabAnimation2.SetActive(false);
        }
        else if (currentTutorialName == "Wall" && tutorialPart == 2)
        {
            standCircles.SetActive(true);
            tutorialTabAnimation1.SetActive(true);
            tutorialTabAnimation2.SetActive(true);

            if (playerController.currentStand.transform.position.x == -1.8f)
            {
                tutorialTabAnimation1.transform.localPosition = new Vector3(0, -0.025f, 0);
                tutorialTabAnimation2.transform.localPosition = new Vector3(0.018f, -0.025f, 0);
            }
            else if(playerController.currentStand.transform.position.x == 0f)
            {
                tutorialTabAnimation1.transform.localPosition = new Vector3(0.018f, -0.025f, 0);
                tutorialTabAnimation2.transform.localPosition = new Vector3(-0.018f, -0.025f, 0);
            }
            else if (playerController.currentStand.transform.position.x == 1.8f)
            {
                tutorialTabAnimation1.transform.localPosition = new Vector3(0, -0.025f, 0);
                tutorialTabAnimation2.transform.localPosition = new Vector3(-0.018f, -0.025f, 0);
            }

            tutorialCircle.SetActive(false);
            currentStandCircle.SetActive(false);
            tutorialArrow.SetActive(false);
        }
        else if (currentTutorialName == "Enemy Wall Mix" && tutorialPart == 1)
        {
            standCircles.SetActive(false);
            tutorialCircle.SetActive(false);
            tutorialArrow.SetActive(false);
            currentStandCircle.transform.position = playerController.currentStand.transform.position;
            currentStandCircle.SetActive(true);
            tutorialTabAnimation1.SetActive(false);
            tutorialTabAnimation2.SetActive(false);
        }
        else if (currentTutorialName == "Not Touch Enemy" && tutorialPart == 1)
        {
            standCircles.SetActive(false);
            tutorialCircle.SetActive(false);
            currentStandCircle.SetActive(false);
            tutorialArrow.SetActive(false);
            tutorialTabAnimation1.SetActive(false);
            tutorialTabAnimation2.SetActive(false);
        }
    }

    private void ShowTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }

    private void TutorialFinished()
    {
        tutorialPanel.SetActive(false);
        standCircles.SetActive(false);
        tutorialCircle.SetActive(false);
        currentStandCircle.SetActive(false);
        tutorialArrow.SetActive(false);
        tutorialTabAnimation1.SetActive(false);
        tutorialTabAnimation2.SetActive(false);

        SaveSystem.current.EnemyDiscovered(currentTutorialName);
        currentTutorialName = "";

        tutorialPart = 0;

        tutorialIsRunning = false;

        playerController.IsTutorialPlaying(false);

        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSection(float delay)
    {
        tutorialIsRunning = true;

        playerController.IsTutorialPlaying(true);
        spawner.AllowSpawing(false);
        yield return new WaitForSeconds(delay);
        ShowTutorialPanel();
        textBox.SetCurrentTutorialList(currentTutorialName);
    }

    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(2);
        StartAllEnemys();
        spawner.AllowSpawing(true);
    }
}
