using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }
    bool screenFadeOut;
    private int minutesElapsed = 0;
    public UnityEvent onIntervalUpdate;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
    }
    public void ClockUpdate(GameTimeStap timestamp)
    {
        UpdateFarmState(timestamp);
        UpdateShippingState(timestamp);

        if(timestamp.hour == 0 && timestamp.minute == 0)
            OnDayReset();

        if (minutesElapsed >= 15)
        {
            minutesElapsed = 0;
            onIntervalUpdate?.Invoke();
        }
        else minutesElapsed++;

    }
    void OnDayReset()
    {
        foreach(NPCRelationshipState npc in RelationshipStats.relationships)
        {
            npc.hasTalkedToday = false;
            npc.giftGivenToday = false;
        }
    }
    void UpdateShippingState(GameTimeStap timestamp)
    {
        if (timestamp.hour == ShippingBin.hourToShip && timestamp.minute == 0)
        {
            ShippingBin.ShipItems();
        }
    }
    public void UpdateFarmState(GameTimeStap timestamp)
    {
        if (timestamp.hour == 0 && timestamp.minute == 0)
        {
            OnDayReset();
        }
        if (Trasition_Scenes.instance.currentLocation != Trasition_Scenes.Locations.Farm)
        {
            if (LandManager.farmData == null) return;

            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            if (cropData.Count == 0)
                return;
            for (int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if (crop.cropState == CropsBehaviour.CropStage.Wilted) continue;

                land.ClockUpdate(timestamp);

                if (land.landStatus == Land_Farm.LandStatus.Watered)
                {
                    crop.Grow();
                }
                else if (crop.cropState != CropsBehaviour.CropStage.Seed)
                {
                    crop.Wither();
                }
                cropData[i] = crop;
                landData[crop.landID] = land;
            }
            LandManager.farmData.Item2.ForEach((CropSaveState crop) => { Debug.Log(crop.seedToGrow + "\n health: " + crop.health + "\n Growth: " + crop.growth + "\n State: " + crop.cropState.ToString()); });
        }

    }
    public void Sleep()
    {
        UIManager.Instance.FadeOutScreen();
        screenFadeOut = false;
        
        StartCoroutine(TransitionTime());
        
    }
    IEnumerator TransitionTime()
    {
        GameTimeStap timestampOfNextDay = TimeManager.Instance.GetGameTimesStap();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;
       
        while(!screenFadeOut)
        {
            yield return new WaitForSeconds(1);
        }
        TimeManager.Instance.SkipTime(timestampOfNextDay);
        //SaveManager.Save(ExportSaveState());
        screenFadeOut = false;
        UIManager.Instance.ResetDefault();

    }
    public void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }
    public static GameSaveState ExportSaveState()
    {
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

        GameTimeStap timestamp = TimeManager.Instance.GetGameTimesStap();
        return new GameSaveState(landData, cropData, timestamp);
    }

    public void LoadSave()
    {
       
        GameSaveState save = SaveManager.Load(); 

        TimeManager.Instance.LoadTime(save.timestamp);

        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData,save.cropData);
    }
}
