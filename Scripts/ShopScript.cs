using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {

    public GameObject ShopMenu;
    public GameObject MainMenu;
    public GameObject SkinsContainer;
    public GameObject UpgradesContainer;
    public GameObject DiamondsContainer;


    public static float ShipPrice1;
    public static float ShipPrice2;

    void Start()
    {
        ShipPrice1 = 10;
        ShipPrice2 = 20;
    }

    void Update()
    {

    }

    public void Back()
    {
        ShopMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void SkinsContainerButton()
    {
        UpgradesContainer.SetActive(false);
        DiamondsContainer.SetActive(false);
        SkinsContainer.SetActive(true);
    }

    public void UpgradesContainerButton()
    {
        DiamondsContainer.SetActive(false);
        SkinsContainer.SetActive(false);
        UpgradesContainer.SetActive(true);
    }

    public void DiamondsContainerButton()
    {
        UpgradesContainer.SetActive(false);
        SkinsContainer.SetActive(false);
        DiamondsContainer.SetActive(true);
    }

}
