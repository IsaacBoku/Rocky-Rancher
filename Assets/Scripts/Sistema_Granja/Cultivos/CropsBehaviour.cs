using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropsBehaviour : MonoBehaviour
{
    int landID;
    SeedData seedToGrow;

    [Header("Stages of Life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;

    private GameObject harvestable;
    int growth;
    int maxGrowth;
    int maxHealth = GameTimeStap.HourToMinute(48);
    int health;
    public enum CropStage
    {
        Seed, Seedling, Harvestable, Wilted

    }
    public CropStage cropStage;
    public void Plant(int landID,SeedData seedToGrow)
    {
        LoadCrop(landID,seedToGrow,CropStage.Seed,0,0);

        LandManager.Instance.RegisterCrop(landID,seedToGrow ,cropStage,growth,health);
    }
    public void LoadCrop(int landID, SeedData seedToGrow,CropStage cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;

        seedling = Instantiate(seedToGrow.seedling, transform);

        ItemData_Oficial cropToYield = seedToGrow.cropsToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        int hoursToGrow = GameTimeStap.DayToHour(seedToGrow.daysToGrow);

        maxGrowth = GameTimeStap.HourToMinute(hoursToGrow);

        this.growth = growth;
        this.health = health;
        if (seedToGrow.regrowable)
        {
            RegrowableHaverstBeha regrowableHaverst = harvestable.GetComponent<RegrowableHaverstBeha>();

            regrowableHaverst.SetParent(this);
        }
        SwitchState(cropState);
    }
    public void Grow()
    {
        growth++;
        if (health < maxHealth)
        {
            health++;
        }
        if (growth >= maxGrowth / 2 && cropStage == CropStage.Seed)
        {
            SwitchState(CropStage.Seedling);
        }
        if (growth >= maxGrowth && cropStage == CropStage.Seedling)
        {
            SwitchState(CropStage.Harvestable);
        }
        LandManager.Instance.OnCropStateChange(landID,cropStage,growth,health);
    }
    public void Wither()
    {
        health--;
        if (health <= 0 && cropStage != CropStage.Seed)
        {
            SwitchState(CropStage.Wilted);
        }
        LandManager.Instance.OnCropStateChange(landID, cropStage, growth, health);
    }
    void SwitchState(CropStage stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);

        harvestable.SetActive(false);
        wilted.SetActive(false);
        switch (stateToSwitch)
        {
            case CropStage.Seed:
                seed.SetActive(true);
                break;
            case CropStage.Seedling:
                seedling.SetActive(true);
                health = maxHealth;
                break;
            case CropStage.Harvestable:
                harvestable.SetActive(true);
                if (!seedToGrow.regrowable)
                {
                    harvestable.transform.parent = null;

                    harvestable.GetComponent<Interacion_Objetos>().onInteract.AddListener(RemoveCrop);

                }
                break;
            case CropStage.Wilted:
                wilted.SetActive(true);
                break;
        }
        cropStage = stateToSwitch;
    }
    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Destroy(gameObject);
    }
    public void ReGrow()
    {
        int hoursToReGrow = GameTimeStap.DayToHour(seedToGrow.dayToRegrow);
        growth = maxGrowth - GameTimeStap.HourToMinute(hoursToReGrow);
        SwitchState(CropStage.Seedling);
    }
}
