using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats_Buy 
{
    public static int Money {  get; private set; }

    public const string CURRENCY = "E";
    
    public static void Spend(int cost)
    {
        if(cost > Money)
        {

        }
        Money -= cost;
        UIManager.Instance.RenderPlayerMoney();
    }
    public static void Earn(int income)
    {
        
        Money += income;
        UIManager.Instance.RenderPlayerMoney();
    }
}
