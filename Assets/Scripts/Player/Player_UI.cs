using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player_UI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] public GameObject GameOver_Panel;
    [SerializeField] public GameObject PausedMenu_Panel;

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI LevelText;
    [SerializeField] private Int_SO currentLevel;
    [SerializeField] public TextMeshProUGUI GoldText;
    [SerializeField] private GoldData currentGold;
    [SerializeField] private Int_SO currentHealth;

    [Header("Hunger")]
    [SerializeField] public HungerData hungerData;

    public static Player_UI instance;

    public int Level
    {
        get { return currentLevel.value; }
        set 
        {
            currentLevel.value = value;
            LevelText.text = "Level " + currentLevel.value.ToString();
        }
    }

    public int Gold
    {
        get { return currentGold.CurrentGold; }
        set 
        {
            currentGold.CurrentGold = value;
            GoldText.text = currentGold.CurrentGold.ToString();
        }
    }

    public bool IsDoubleGold
    {
        get { return currentGold.IsDoubleGoldActive; }
        set { currentGold.IsDoubleGoldActive = value; }
    }

    public float Hunger
    {
        get { return hungerData.Hunger; }
        set { hungerData.Hunger = value; }
    }

    private void Start()
    {
        /*
        if (SceneManager.GetActiveScene().name == "Start_Menu")
        {
            Level = 1;
            Gold = 200;
            Hunger = 100;

            currentHealth.value = 3;
        }
        */

        if (SceneManager.GetActiveScene().name == "Game_Level")
        {

            GameOver_Panel.SetActive(false);
            PausedMenu_Panel.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game_Level")
        {
            GoldText.text = currentGold.CurrentGold.ToString();
            LevelText.text = "Level " + Level;
        }
    }

    public void SetUp()
    {
        Level = 1;
        Gold = 200;
        Hunger = 100;
        currentHealth.value = 3;
    }
    
}
