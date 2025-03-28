using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CropsBehaviour;

[System.Serializable]
public struct CropSaveState 
{
    public int landID;

    public string seedToGrow;
    public CropsBehaviour.CropStage cropState;
    public int growth;
    public int health;
    public CropSaveState(int landID, string seedToGrow, CropsBehaviour.CropStage cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }
    public void Grow()
    {
        growth++;

        SeedData seedInfo = (SeedData) Inventory.instance.itemIndex.GetItemFromString(seedToGrow);

        int maxGrowth = GameTimeStap.HourToMinute(GameTimeStap.DayToHour(seedInfo.daysToGrow));
        int maxHealth = GameTimeStap.HourToMinute(48);
        if (health < maxHealth)
        {
            health++;
        }
        if (growth >= maxGrowth / 2 && cropState == CropStage.Seed)
        {
            cropState = CropStage.Seedling;
        }
        if (growth >= maxGrowth && cropState == CropStage.Seedling)
        {
            cropState = CropStage.Harvestable;
        }
    }
    public void Wither()
    {
        health--;
        if (health <= 0 && cropState != CropStage.Seed)
        {
            cropState = CropStage.Wilted;
        }
    }
}
