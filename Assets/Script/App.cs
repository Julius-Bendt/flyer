using UnityEngine;
using System.IO;
using Juto.UI;
using UnityEngine.Events;
using Juto;
using System.Collections.Generic;

public class App : Singleton<App>
{
    // (Optional) Prevent non-singleton constructor use.
    protected App() { }


    [Header("References")]
    public Settings settings;
    public Upgrades upgrades;
    public PlayerVar playerVar;
    public UIManager uiManager;
    public Spawner spawner;
    public PlayerController player;
    public ShakeBehavior shake;
    public UpgradeDatabase upgradeDatabase;
    public AudioDB audioDB;
    public MusicPlayer musicPlayer;
    public UIAnimation animations;
    public PoolManager pool;


    private int score;
    public int enemiesKilled, moneyEarned, highestStreak;
    public List<Vector3> streak = new List<Vector3>();
    public float streakCooldown = 0;

    public bool isPlaying = true;

    public bool isNewGame = false;


    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            uiManager.UpdateGameScore(score,value);
            score = value;
        }
    }

    private void Start()
    {
        Load();

        //Find references
        uiManager = FindObjectOfType<UIManager>();
        spawner = FindObjectOfType<Spawner>();
        player = FindObjectOfType<PlayerController>();
        shake = FindObjectOfType<ShakeBehavior>();
        upgradeDatabase = FindObjectOfType<UpgradeDatabase>();
        audioDB = FindObjectOfType<AudioDB>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        animations = FindObjectOfType<UIAnimation>();
        pool = FindObjectOfType<PoolManager>();
        uiManager.SwitchUI(UIManager.UIType.menu, false);

        streak = new List<Vector3>();
    }

    public void Update()
    {
        if(streakCooldown < 0)
        {
            if (streak.Count > highestStreak)
                highestStreak = streak.Count;

            streak.Clear();
        }
        else
        {
            streakCooldown -= Time.deltaTime;
        }
    }

    void OnApplicationFocus(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
        else
        {
            Load();
        }
    }

    public void StopGame()
    {
        isPlaying = false;

        if(playerVar.highscore < Score)
            playerVar.highscore = Score;

        if (playerVar.highest_streak < highestStreak)
            playerVar.highest_streak = highestStreak;

        if(Score > 750)
            moneyEarned += (Score / upgrades.scoreToMoney);

        playerVar.money += moneyEarned;
        playerVar.moneyEarned += moneyEarned;
        playerVar.enemiesKilled += enemiesKilled;

        playerVar.timesPlayed++;

        uiManager.OnGameEnd();

        //Delete old objects
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            pool.Destroy(e.gameObject);
        }

        foreach (bomb b in FindObjectsOfType<bomb>())
        {
            pool.Destroy(b.gameObject);
        }

        foreach (Bullet b in FindObjectsOfType<Bullet>())
        {
            pool.Destroy(b.gameObject);
        }

        
    }

    public void StartGame()
    {
        isPlaying = true;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            pool.Destroy(enemy.gameObject); //Delete cosmetic enemies
        }

        player.StartGame();
    }

    public void ResetGameVars()
    {
        //Reset last game var
        Score  = highestStreak = enemiesKilled = moneyEarned = 0;
        streak.Clear();
    }



    public void Save()
    {
        //Get Json string
        string settingsJson = JsonUtility.ToJson(settings);
        string upgradesJson = JsonUtility.ToJson(upgrades);
        string playerVarJson = JsonUtility.ToJson(playerVar);

        //Save to file
        File.WriteAllText(Application.persistentDataPath + "/settings.json", settingsJson);
        File.WriteAllText(Application.persistentDataPath + "/upgrades.json", upgradesJson);
        File.WriteAllText(Application.persistentDataPath + "/player.json", playerVarJson);


    }

    public void Load()
    {
        settings = new Settings();
        upgrades = new Upgrades();
        playerVar = new PlayerVar();

        if(File.Exists(Application.persistentDataPath + "/settings.json") && File.Exists(Application.persistentDataPath + "/upgrades.json"))
        {
            //Load json
            string settingsJson = File.ReadAllText(Application.persistentDataPath + "/settings.json");
            string upgradesJson = File.ReadAllText(Application.persistentDataPath + "/upgrades.json");
            string playerJson = File.ReadAllText(Application.persistentDataPath + "/player.json");

            //update vars
            settings = JsonUtility.FromJson<Settings>(settingsJson);
            upgrades = JsonUtility.FromJson<Upgrades>(upgradesJson);
            playerVar = JsonUtility.FromJson<PlayerVar>(playerJson);

        }
        else
        {
            isNewGame = true;
        }
    }

    private void OnDestroy()
    {
        Save();
    }
}