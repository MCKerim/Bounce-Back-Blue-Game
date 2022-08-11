using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private List<PlayerBall> allPlayerBalls;

    [SerializeField] private TextMeshProUGUI errorText;

    private List<string> unlockedPlayerBalls;

    private string selectedPlayerBall;

    private int coins;

    private List<string> discoveredEnemys;
    private bool allEnemysDiscovered;
    [SerializeField] private int allTutorialsCount;

    public event Action StatsResettet;
    public event Action TutorialResettet;

    public static SaveSystem current;
    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
            DontDestroyOnLoad(gameObject);

            StatsData data = LoadStats();
            if (data != null)
            {
                unlockedPlayerBalls = data.unlockedPlayerBalls;
                selectedPlayerBall = data.selectedPlayerBall;
                coins = data.coins;
                discoveredEnemys = data.discoveredEnemys;
                errorText.SetText(selectedPlayerBall + " Coins:"  + coins + " " + " " + unlockedPlayerBalls[0]);
            }
            else
            {
                FirstTimeSave();
            }
        }
    }

    public bool AreAllEnemysDiscovered()
    {
        ///////////////////////////////////////       CHECH ALL TUTORIALS COUNT IF == TUTORIALS ////////
        return discoveredEnemys.Count == allTutorialsCount;
    }

    public int GetCoins()
    {
        return coins;
    }

    public void ChangeCoins(int value)
    {
        coins += value;
    }

    private void OnApplicationQuit()
    {
        SaveStats();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveStats();
        }
    }

    public bool IsUnlocked(string targetPlayerBall)
    {
        return unlockedPlayerBalls.Contains(targetPlayerBall);
    }

    public void Unlock(string targetPlayerBall)
    {
        if (!IsUnlocked(targetPlayerBall))
        {
            unlockedPlayerBalls.Add(targetPlayerBall);
        }
    }

    public void SelectPlayerBall(string targetPlayerBall)
    {
        if (IsUnlocked(targetPlayerBall))
        {
            selectedPlayerBall = targetPlayerBall;
        }
    }

    public Sprite GetSelectedPlayerBallSprite()
    {
        foreach(PlayerBall currentBall in allPlayerBalls)
        {
            if(currentBall.ballName == selectedPlayerBall)
            {
                return currentBall.sprite;
            }
        }
        return null;
    }

    private void FirstTimeSave()
    {
        unlockedPlayerBalls = new List<string>();
        Unlock(allPlayerBalls[0].ballName);
        SelectPlayerBall(allPlayerBalls[0].ballName);

        coins = 0;

        discoveredEnemys = new List<string>();
        errorText.SetText("First Time");
        SaveStats();
    }

    public void ResetStats()
    {
        unlockedPlayerBalls.Clear();
        Unlock(allPlayerBalls[0].ballName);
        SelectPlayerBall(allPlayerBalls[0].ballName);

        coins = 0;

        StatsResettet?.Invoke();

        errorText.SetText("Stats Resettet");
        Debug.Log("Stats Resetet.");
    }

    public void ResetDiscoveredEnemys()
    {
        discoveredEnemys.Clear();

        TutorialResettet?.Invoke();
        errorText.SetText("Enemys Cleared");
        Debug.Log("Discovered Enemys Resetet.");
    }

    public bool CheckIfEnemyIsDiscovered(string enemyNameToCheck)
    {
        return discoveredEnemys.Contains(enemyNameToCheck);
    }

    public void EnemyDiscovered(string newDiscoveredEnemy)
    {
        if(newDiscoveredEnemy != "")
        {
            discoveredEnemys.Add(newDiscoveredEnemy);
        }
    }

    public void SaveStats()
    {
        errorText.SetText("Try to save..");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/statsBounceBack.MCKerimData";
        FileStream stream = new FileStream(path, FileMode.Create);

        StatsData data = new StatsData(unlockedPlayerBalls, selectedPlayerBall, coins, discoveredEnemys);

        formatter.Serialize(stream, data);
        stream.Close();
        errorText.SetText("Saved in: " + path);
        Debug.Log("Data Saved.");
    }

    public StatsData LoadStats()
    {
        string path = Application.persistentDataPath + "/statsBounceBack.MCKerimData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StatsData data = formatter.Deserialize(stream) as StatsData;
            stream.Close();
            errorText.SetText("Save File Found in: " + path);
            return data;
        }
        else
        {
            Debug.Log("Save File not found in " + path);
            errorText.SetText("Save File not Found" + path);
            return null;
        }
    }

    public void ClearErrorText()
    {
        errorText.SetText("");
    }

    public void CheatCoins()
    {
        coins += 100;
        errorText.SetText("Coins: " + coins);
    }
}

[System.Serializable]
public class StatsData
{
    public List<string> unlockedPlayerBalls;
    public string selectedPlayerBall;

    public int coins;

    public List<string> discoveredEnemys;

    public StatsData(List<string> _unlockedPlayerBalls, string _selectedPlayerBall, int _coins, List<string> _discoveredEnemys)
    {
        unlockedPlayerBalls = _unlockedPlayerBalls;
        selectedPlayerBall = _selectedPlayerBall;
        coins = _coins;
        discoveredEnemys = _discoveredEnemys;
    }
}
