using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Juto.Standard;

public class UpgradeItemUI : MonoBehaviour
{
    public TextMeshProUGUI name, price, changeBy;
    public Button button;

    [HideInInspector]
    public string nameToLookFor = string.Empty;

    public void Purchase()
    {
        AudioController.PlaySound(App.Instance.audioDB.purchase);
        App.Instance.upgradeDatabase.Purchase(nameToLookFor);
    }
}
