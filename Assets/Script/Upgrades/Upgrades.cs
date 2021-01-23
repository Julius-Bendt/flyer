using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrades
{

    //in-game upgrades
    public int maxAmmo = 15;
    public float blastRadius;
    public int line;

    
    //Money upgrades
    public int scoreToMoney = 25;



    public int maxAmmoIndex, blastRadiusIndex, lineIndex, scoreToMoneyIndex;
	
}
