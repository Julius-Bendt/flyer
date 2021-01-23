using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Juto.UI;
using Juto.Standard;

public class UIManager : MonoBehaviour {

    public GameObject menu, game,gameover, upgrades, settings;
    private GameObject lastObj;

    private UIAnimationFramework effects;

    [Header("Fade options")]
    public Image fadeImage;
    public Color fadeColor;
    private const float FADETIME = 0.5f;

    [Header("Menu")]
    public TextMeshProUGUI menuStatistic;

    [Header("Game")]
    public TextMeshProUGUI gameAmmo;
    public TextMeshProUGUI gameScore;

    [Header("Gameover")]
    public TextMeshProUGUI gameoverScore;
    public TextMeshProUGUI gameoverMoney, gameoverEnemiesKilled, gameoverHighestStreak;

    [Header("Upgrade")]
    public TextMeshProUGUI upgradeMoney;

    [Header("Settings")]
    public Image settingsSFX;
    public Image settingsMusic, settingsShake;
    public Color settingsEnabled, settingsDisabled;

    [Header("Tutorial")]
    public GameObject tutorial;

    [Header("Streak text")]
    public GameObject streakText;


    //Animation properties
    private UIType openNext = UIType.none, last = UIType.none;
    private bool fade;

    [HideInInspector]
    public bool fading, animationActive;

    private void Start()
    {
        effects = GetComponent<UIAnimationFramework>();

        //Update upgrades

        //Update Settings
       
    }

    public enum UIType
    {
        none,
        menu,
        game,
        gameover,
        upgrades,
        skins,
        settings,
        tutorial
    };

    public void OnGameStart()
    {
      
    }

    public void OnGameEnd()
    {
        SwitchUI(UIType.gameover);
    }

    //Second in animation row
    //UI updates should go in here
    private void FadeInOver()
    {
        if (lastObj != null)
            lastObj.SetActive(false);

        switch (openNext)
        {
            case UIType.menu:
                menu.SetActive(true);
                lastObj = menu;
                OnMenuUI();
                break;
            case UIType.game:
                game.SetActive(true);
                lastObj = game;
                OnGameUI();
                break;
            case UIType.gameover:
                gameover.SetActive(true);
                lastObj = gameover;
                OnGameoverUI();
                break;
            case UIType.upgrades:
                OnUpgradeUI();
                upgrades.SetActive(true);
                lastObj = upgrades;
                break;
            case UIType.settings:
                OnSettingsUI();
                settings.SetActive(true);
                lastObj = settings;
                break;
            case UIType.tutorial:
                OnTutorialUI();
                tutorial.SetActive(true);
                lastObj = tutorial;
                break;
        }

        effects.Fade(fadeImage, Color.clear, FADETIME,0, FadeOutOver);
    }

    //Actually do what ever the buttons should do.
    //last in animation row
    //what happens when the button is pressed should go here. ect play-btn pressed, start game 
    private void FadeOutOver()
    {
        App.Instance.animations.Open(openNext.ToString(),0);
    }



    //First in animation row
    public void SwitchUI(UIType type,bool _fade = true)
    {
        fade = _fade;
        last = openNext;
        App.Instance.musicPlayer.switchClip(type);
        openNext = type;

        if (last != UIType.none)
            App.Instance.animations.Close(last.ToString(),0); // HideUIAnimationOver(); gets called by the animations
        else
            HideUIAnimationOver();
        
    }

    //Called by UI manager
    public void HideUIAnimationOverCheck(UIAnimation.UIAnimationEvent anim_event)
    {
        if (!anim_event.opened)
            HideUIAnimationOver();
    }

    public void StartGameoverTextEffect(UIAnimation.UIAnimationEvent _event)
    {
        if(_event.opened && _event.name == UIType.gameover.ToString())
        {
            float time = 2f;

            int add = (100 * App.Instance.player.Health + ((App.Instance.player.Health - 1) * 50));
            App.Instance.Score += add;
            string score = (App.Instance.Score > App.Instance.playerVar.highscore) ? "New highscore" : "Score";
            effects.TextValue(gameoverScore, score + ": {0}", 0, App.Instance.Score, time);
            effects.TextValue(gameoverMoney, "Money Earned: {0}", 0, App.Instance.moneyEarned, time * 0.9f);
            effects.TextValue(gameoverEnemiesKilled, "Enemies Killed: {0}", 0, App.Instance.enemiesKilled, time * 0.5f);
            effects.TextValue(gameoverHighestStreak, "Highest Streak: {0}", 0, App.Instance.highestStreak, time * 0.45f);
        }
    }

    public void MakeStreakText()
    {;
        if(App.Instance.streak.Count > 2)
        {
            Vector3[] draw = new Vector3[App.Instance.streak.Count];
            App.Instance.streak.CopyTo(draw);

            for (int i = 0; i < draw.Length; i++)
            {
                
                Vector3 pos = draw[i];

                if (pos == Vector3.zero) // Dont show already shown streak texts
                    continue;
                else
                {
                    GameObject t = App.Instance.pool.Instantiate(streakText);
                    t.GetComponent<StreakText>().SetPos(pos, i);

                    App.Instance.streak.Remove(pos);
                    App.Instance.streak.Add(Vector3.zero);
                }
            }

        }
    }

    public void HideUIAnimationOver()
    {
        if (effects == null)
            effects = GetComponent<UIAnimationFramework>();

        if (fade)
            effects.Fade(fadeImage, fadeColor, FADETIME,0,FadeInOver);
        else
            FadeInOver();
    }

    #region Button events

    public void StartGame()
    {
        if(App.Instance.isNewGame)
        {
            SwitchUI(UIType.tutorial);
            App.Instance.isNewGame = false;
        }
        else
            SwitchUI(UIType.game);
        AudioController.PlaySound(App.Instance.audioDB.click);
    }

    public void OpenUpgrades()
    {
        SwitchUI(UIType.upgrades);
        AudioController.PlaySound(App.Instance.audioDB.click);
    }

    public void OpenSettings()
    {
        SwitchUI(UIType.settings);
        AudioController.PlaySound(App.Instance.audioDB.click);
    }

    public void ToMenu()
    {
        SwitchUI(UIType.menu);
        AudioController.PlaySound(App.Instance.audioDB.click);
    }

    public void ToggleSFX()
    {
        App.Instance.settings.sfx = !App.Instance.settings.sfx;
        settingsSFX.color = (App.Instance.settings.sfx) ? settingsEnabled : settingsDisabled;
        AudioController.PlaySound(App.Instance.audioDB.click);
    }

    public void ToggleMusic()
    {
        AudioController.PlaySound(App.Instance.audioDB.click);
        App.Instance.settings.music = !App.Instance.settings.music;
        settingsMusic.color = (App.Instance.settings.music) ? settingsEnabled : settingsDisabled;

        App.Instance.musicPlayer.switchClip(UIType.settings);
    }

    public void ToggleShake()
    {
        AudioController.PlaySound(App.Instance.audioDB.click);
        App.Instance.settings.shake = !App.Instance.settings.shake;
        settingsShake.color = (App.Instance.settings.shake) ? settingsEnabled : settingsDisabled;
    }

    public void ClearSave()
    {
        AudioController.PlaySound(App.Instance.audioDB.click);
        App.Instance.playerVar = new PlayerVar();
        App.Instance.upgrades = new Upgrades();
        App.Instance.Save();
    }


    public void Leaderboard()
    {

    }

    #endregion

    #region What the buttons actually do

    private void OnMenuUI()
    {
        menuStatistic.text = string.Format("Money earned: {0}\nMoney spend: {1}\nHighscore: {2}\nHighest streak: {3}\nEnemies killed: {4}\ntimes played: {5}", App.Instance.playerVar.moneyEarned, App.Instance.playerVar.moneySpend, App.Instance.playerVar.highscore, App.Instance.playerVar.highest_streak, App.Instance.playerVar.enemiesKilled, App.Instance.playerVar.timesPlayed);
    }

    private void OnGameUI()
    {
        App.Instance.ResetGameVars();
        UpdateGameScore(0, 0);
        UpdateGameAmmo(10);
        App.Instance.StartGame();
    }

    private void OnGameoverUI()
    {

    }

    private void OnUpgradeUI()
    {
        UpdateCoinInUpgrade();
    }

    private void OnSettingsUI()
    {
        settingsSFX.color = (App.Instance.settings.sfx) ? settingsEnabled : settingsDisabled;
        settingsMusic.color = (App.Instance.settings.music) ? settingsEnabled : settingsDisabled;
        settingsShake.color = (App.Instance.settings.shake) ? settingsEnabled : settingsDisabled;
    }

    private void OnTutorialUI()
    {

    }



    #endregion

    #region script event

    public void UpdateGameScore(int from,int to)
    {
        effects.TextValue(gameScore,"Score: {0}", from, to, 1f); 
        // gameScore.text = formatScore(App.Instance.Score, false);
    }

    public void UpdateGameAmmo(int ammo)
    {
        gameAmmo.text = string.Format("Ammo: {0}/{1}",ammo,App.Instance.upgrades.maxAmmo);
    }

    public void UpdateOptions()
    {

    }

    public void UpdateCoinInUpgrade()
    {
        upgradeMoney.text = "Money: " + App.Instance.playerVar.money;
    }

    #endregion

    public string formatScore(int score, bool highscore, bool includeText = true)
    {

        string extra = "";

        if (score < 10)
            extra += "0";
        if (score < 100)
            extra += "0";
        if (score < 1000)
            extra += "0";

        string t = (highscore) ? "New Highscore!\n" : "Score\n";

        if (!includeText)
            t = "";

        return string.Format("{0}{1}{2}", t, extra, score);
    }
}
