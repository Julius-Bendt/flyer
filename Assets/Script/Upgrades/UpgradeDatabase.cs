using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour
{
    public List<UpgradeItem> items;



    public GameObject prefab;
    public Transform parent;

    public Color canBuy = Color.white, cantBuy = Color.red;

    private void Start()
    {
        CreateUI();
    }

    public void Purchase(string name)
    {
        //Find the item first
        foreach (UpgradeItem item in items) //Primary updates enough coin text.
        {

            if(item.name == name) //Get value to change 
            {
                if(item.currentLevel < item.prices.Length-1)
                {
                    //Increase the price. (based on level-index)
                    App.Instance.playerVar.money -= item.GetPrice();
                    App.Instance.playerVar.moneySpend += item.GetPrice();
                    item.currentLevel++;

                    //actual change the value
                    int dir = (item.minMax.y > item.minMax.x) ? 1 : -1; //minus or plus it
                    ChangeValueBasedOnName(name, item.ChangeBy(), dir, item.currentLevel);
                }
            }

            UpdateUI(item);
        }

        App.Instance.uiManager.UpdateCoinInUpgrade();
    }

    public void ChangeValueBasedOnName(string name,float value,int dir,int index)
    {
        switch (name)
        {
            case "Max Ammo":
                App.Instance.upgrades.maxAmmo += (int)(value * dir);
                App.Instance.upgrades.maxAmmoIndex = index;
                break;
            case "Blast Radius":
                App.Instance.upgrades.blastRadius += (int)(value * dir);
                App.Instance.upgrades.blastRadiusIndex = index;
                break;
            case "Line":
                App.Instance.upgrades.line += (int)(value * dir);
                App.Instance.upgrades.lineIndex = index;
                break;
            case "Score to money":
                App.Instance.upgrades.scoreToMoney += (int)(value * dir);
                App.Instance.upgrades.scoreToMoneyIndex = index;
                break;
        }
    }

    public void UpdateUI()
    {
        foreach (UpgradeItem item in items)
        {
            UpdateUI(item);
        }
    }

    public void UpdateUI(UpgradeItem item)
    {
        item.ui.price.text = "Money: " + item.GetPrice();
        item.ui.price.color = (App.Instance.playerVar.money >= item.GetPrice()) ? canBuy : cantBuy;

        item.ui.changeBy.text = item.ChangeByFormat();
        item.ui.changeBy.color = (item.currentLevel <= item.prices.Length - 1) ? canBuy : cantBuy;

        item.ui.button.interactable = item.ButtonActive();
    }

    public void CreateUI()
    {
        foreach (UpgradeItem item in items)
        {
            UpgradeItemUI i = Instantiate(prefab,parent).GetComponent<UpgradeItemUI>();

            item.Init(i,canBuy,cantBuy);
        }
    }

    public void LoadUpgrades()
    {
        foreach (UpgradeItem item in items)
        {
            switch (item.name)
            {
                case "Max Ammo":
                    item.currentLevel = App.Instance.upgrades.maxAmmoIndex;
                    break;
                case "Blast Radius":
                    item.currentLevel = App.Instance.upgrades.blastRadiusIndex;
                    break;
                case "Line":
                    item.currentLevel = App.Instance.upgrades.lineIndex;
                    break;
                case "Score to money":
                    item.currentLevel = App.Instance.upgrades.scoreToMoneyIndex;
                    break;
                default:
                    Debug.Log("couldn't find " + item.name);
                    break;
            }
        }
    }
}

[System.Serializable]
public class UpgradeItem
{
    public string name; //name
    public int[] prices; //Controls how many levels there are.
    public Vector2 minMax; // minMax of value. min = x, max = y. 
    public int currentLevel; //controls current index.

    [HideInInspector]
    public string plusOrMinus;

    [HideInInspector]
    public UpgradeItemUI ui;

    public void Init(UpgradeItemUI _ui,Color canbuy,Color cantbuy)
    {
        ui = _ui;

        ui.nameToLookFor = name;
        ui.name.text = name;
        ui.price.text = "Money: " + prices[currentLevel].ToString();
        ui.price.color = (App.Instance.playerVar.money >= prices[currentLevel]) ? canbuy : cantbuy;

        plusOrMinus = (minMax.y > minMax.x) ? "+" : "-";

        ui.changeBy.text = ChangeByFormat();
        ui.changeBy.color = (currentLevel <= prices.Length - 1) ? canbuy : cantbuy;
        ui.button.interactable = ButtonActive();
    }

    public float ChangeBy() //How much should the upgrade value be changed by, pr purchase?
    {

        return minMax.y / prices.Length;
    }

    public string ChangeByFormat()
    {
        //current/max (+ or -) changeBy

        if (currentLevel >= prices.Length-1)
            return "Max";
        else
            return string.Format("{0}/{1} {2} {3}",FindValueFromString(),minMax.y,plusOrMinus,ChangeBy());
    }

    public string FindValueFromString()
    {

        switch(name)
        {
            case "Max Ammo":
                return App.Instance.upgrades.maxAmmo.ToString();
            case "Blast Radius":
                return App.Instance.upgrades.blastRadius.ToString();
            case "Line":
                return App.Instance.upgrades.line.ToString();
            case "Score to money":
                return App.Instance.upgrades.scoreToMoney.ToString();
        }

        return "invalid - " + name;
    }

    public int GetPrice()
    {
        return prices[currentLevel];
    }

    public bool ButtonActive()
    {
        if (currentLevel > prices.Length - 1 || App.Instance.playerVar.money < prices[currentLevel])
            return false;
        else
            return true;
}
}
