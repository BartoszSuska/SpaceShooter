using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public Text diamondsInShop;
    public Text lastScoreText;
    public Text highScoreText;
    public float lastScore;
    public float highScore;
    public float diamondsAmount;
    public GameObject ShopMenu;
    public GameObject MainMenu;
    public Text Ship1PriceText;
    public Text Ship2PriceText;

    public bool Ship0Owned = true;
    public bool Ship1Owned = false;
    public bool Ship2Owned = false;

	// Use this for initialization
	void Awake () {

        PlayerData score = SaveSystem.LoadScore();
        PlayerData menu = SaveSystem.LoadDiamondsAndHighScore();
        lastScore = score.score;
        highScore = menu.hScore;
        Ship1Owned = menu.ShipOwned;
        Ship2Owned = menu.ShipOwned2;
        diamondsAmount = menu.diamondData + Diamond.lastGameDiamonds;
        diamondsInShop.text = "Diamonds: " + diamondsAmount.ToString();
        if (lastScore > highScore)
        {
            highScore = lastScore;
        }
        lastScoreText.text = "LAST SCORE: " + (int)lastScore;
        highScoreText.text = "HIGH SCORE: " + (int)highScore;
    }
	
	// Update is called once per frame
	void Update () {

        if (Ship1Owned)
        {
            Ship1PriceText.text = "Select";
        }
        else
        {
            Ship1PriceText.text = "Diamonds: " + ShopScript.ShipPrice1.ToString();
        }

        if (Ship2Owned)
        {
            Ship2PriceText.text = "Select";
        }
        else
        {
            Ship2PriceText.text = "Diamonds: " + ShopScript.ShipPrice2.ToString();
        }
    }

    public void PlayGame()
    {
        Diamond.lastGameDiamonds = 0;
        SaveSystem.SaveDiamondsAndHighScore(this);
        ScoreScript.scoreCounter = 0;
        ScoreScript.bossAlive = false;
        ScoreScript.bossTime = 0;
        SceneManager.LoadScene("Game");
    }

    public void Shop()
    {
        MainMenu.SetActive(false);
        ShopMenu.SetActive(true);
    }

    public void Ship0Select()
    {
        PlayerSpawn.modelNumber = 0;
    }

    public void Ship1Select()
    {
        if(Ship1Owned)
        {
            PlayerSpawn.modelNumber = 1;
        }
        else
        {
            if(diamondsAmount >= ShopScript.ShipPrice1)
            {
                diamondsAmount -= ShopScript.ShipPrice1;
                Ship1Owned = true;
                PlayerSpawn.modelNumber = 1;
            }
        }
    }

    public void Ship2Select()
    {
        if (Ship2Owned)
        {
            PlayerSpawn.modelNumber = 2;
        }
        else
        {
            if (diamondsAmount >= ShopScript.ShipPrice2)
            {
                diamondsAmount -= ShopScript.ShipPrice2;
                Ship2Owned = true;
                PlayerSpawn.modelNumber = 2;
            }
        }
    }

}
