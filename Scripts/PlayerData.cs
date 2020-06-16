using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public float score;
    public float hScore;
    public float diamondData;
    public bool ShipOwned;
    public bool ShipOwned2;

    void Awake()
    {

    }

    public PlayerData(ScoreScript scores)
    {
        score = ScoreScript.scoreCounter;
    }

    public PlayerData(MenuScript menu)
    {
        hScore = menu.highScore;
        diamondData = menu.diamondsAmount;

        ShipOwned = menu.Ship1Owned;
        ShipOwned2 = menu.Ship2Owned;
    }


}
