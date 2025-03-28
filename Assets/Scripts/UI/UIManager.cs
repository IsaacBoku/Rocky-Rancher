using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour,ITimeTracker
{
    public static UIManager Instance {  get; private set; }
    [Header("StatusBar")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    [Header("Yes or No prompt")]
    public Yes_No_Prompt yesNoprompt;

    [Header("Player money")]
    public  TextMeshProUGUI moneyAmount;
    public TextMeshProUGUI moneyAmountUI;

    [Header("Screen Transitions")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("Shop")]
    public ShopListing_Manager shopListingManager;
    int moneyinicial = 500;
    [Header("Villagers")]
    public RelationshipListingManager relationshipListingManager;

    void Awake()
    {
        if(Instance != null &&Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
        Player_Stats_Buy.Earn(moneyinicial);
        RenderPlayerMoney();
    }
    #region FadeInorOut

    public void FadeOutScreen()
    {
        fadeOut.SetActive(true);
    }
    public void FadeInScreen()
    {
        fadeIn.SetActive(true);
    }
    public void OnFadeInComplete()
    {
        fadeIn.SetActive(false);
    }
    public void ResetDefault()
    {
        fadeOut.SetActive(false);
        fadeIn.SetActive(true);
    }

    #endregion
    public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
    {
        yesNoprompt.gameObject.SetActive(true);

        Time.timeScale = 0;

        yesNoprompt.CreatePrompt(message, onYesCallback);
    }
    public void ClockUpdate(GameTimeStap timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;
        if (minutes % 10 != 0) return;
        string prefix = "AM ";
        if(hours > 12)
        {
            prefix = "PM ";
            hours -= 12;
        }
        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + "  " + day + "(" + dayOfTheWeek + ")";
    }
    #region Shop
    public void RenderPlayerMoney()
    {

        moneyAmount.text = Player_Stats_Buy.Money + Player_Stats_Buy.CURRENCY;
        moneyAmountUI.text = Player_Stats_Buy.Money + Player_Stats_Buy.CURRENCY;
    }
    public void OpenShop(List<ItemData_Oficial> shopItems)
    {
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.Render(shopItems);
    }
    #endregion

    public void ToggleRelationshipPanel()
    {
        GameObject panel = relationshipListingManager.gameObject;
            relationshipListingManager.Render(RelationshipStats.relationships);
        

    }
}
