using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyLibrarieManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyDescriptionText;
    [SerializeField] private Image enemyImage;

    [SerializeField] private GameObject panelForUndiscoveredEnemys;

    [SerializeField] private List<EnemyLibrarieScriptableObject> enemyLibrarieList;
    private int currentEnemy;

    private SaveSystem saveSystem;

    // Start is called before the first frame update
    void Start()
    {
        saveSystem = SaveSystem.current;

        saveSystem.TutorialResettet += DisplayEnemy;

        DisplayEnemy();
    }

    public void ShowNextEnemy()
    {
        currentEnemy++;
        if(currentEnemy >= enemyLibrarieList.Count)
        {
            currentEnemy = 0;
        }

        DisplayEnemy();
    }

    public void ShowPreviousEnemy()
    {
        currentEnemy--;
        if (currentEnemy < 0)
        {
            currentEnemy = enemyLibrarieList.Count-1;
        }

        DisplayEnemy();
    }

    private void DisplayEnemy()
    {
        EnemyLibrarieScriptableObject current = enemyLibrarieList[currentEnemy];

        if(saveSystem.CheckIfEnemyIsDiscovered(enemyLibrarieList[currentEnemy].enemyName))
        {

            enemyNameText.SetText(current.enemyNameInEnemyLibrarie);
            enemyDescriptionText.SetText(current.enemyDiscription);
            enemyImage.sprite = current.enemySprite;
            enemyImage.color = current.enemyColor;

            enemyNameText.color = Color.white;

            panelForUndiscoveredEnemys.SetActive(false);
        }
        else
        {
            enemyNameText.SetText("Undiscovered");
            enemyDescriptionText.SetText("Play more to discover this Enemy. The further you get, the harder it gets. Can you make it?");
            enemyImage.sprite = current.enemySprite;
            enemyImage.color = current.enemyColor;
            enemyNameText.color = Color.red;

            panelForUndiscoveredEnemys.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        saveSystem.TutorialResettet -= DisplayEnemy;
    }
}
