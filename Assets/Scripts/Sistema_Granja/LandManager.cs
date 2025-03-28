using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ObstacleGenerate))]
public class LandManager : MonoBehaviour
{
    public static LandManager Instance { get; private set; }

    public static Tuple<List<LandSaveState>,List<CropSaveState>> farmData = null;

    List<Land_Farm> landPlots = new List<Land_Farm>();

    List<LandSaveState> landData = new List<LandSaveState>();
    List<CropSaveState> cropData = new List<CropSaveState>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        RegisterPlots();
        StartCoroutine(LoadFarmData());
       
    }
    IEnumerator LoadFarmData()
    {
        yield return new WaitForEndOfFrame();
        if (farmData != null)
        {
            ImportLandData(farmData.Item1);
            ImportCropData(farmData.Item2);
        }
        else
        {
            GetComponent<ObstacleGenerate>().GenerateObstacles(landPlots);
        }
    }
    private void OnDestroy()
    {
        farmData =  new Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
    }
    void RegisterPlots()
    {
        foreach(Transform landTransform in transform)
        {
            Land_Farm land = landTransform.GetComponent<Land_Farm>();
            landPlots.Add(land);

            landData.Add(new LandSaveState());
            land.id = landPlots.Count - 1;
        }
    }
    public void RegisterCrop(int landID, SeedData seedToGrow, CropsBehaviour.CropStage cropState, int growth, int health)
    {
        cropData.Add(new CropSaveState(landID, seedToGrow.name, cropState, growth, health));
    }
    public void OnLandStateChange(int id,Land_Farm.LandStatus landStatus, GameTimeStap lastWatered,Land_Farm.farmObstacleStatus obstacleStatus)
    {
        landData[id] = new LandSaveState(landStatus, lastWatered,obstacleStatus);  
    }
    public void DeregisterCrop(int landID)
    {
        cropData.RemoveAll(x => x.landID == landID);
    }
    public void OnCropStateChange(int landID, CropsBehaviour.CropStage cropState, int growth,int health)
    {
        int cropIndex = cropData.FindIndex(x => x.landID == landID);
        string seedToGrow = cropData[cropIndex].seedToGrow;
        cropData[cropIndex] = new CropSaveState(landID, seedToGrow, cropState, growth, health);
    }
    public void ImportLandData(List<LandSaveState> landDatasetToLoad)
    {
        for(int i = 0; i < landDatasetToLoad.Count; i++)
        {
            LandSaveState landDataToLoad = landDatasetToLoad[i];
            landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered, landDataToLoad.obstacleStatus);
        }
        landData = landDatasetToLoad;
    }
    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            Land_Farm landToPlant = landPlots[cropSave.landID];
            CropsBehaviour cropToPlant = landToPlant.SpawnCrop();
            Debug.Log(cropToPlant.gameObject);
            SeedData seedToGrow = (SeedData)Inventory.instance.itemIndex.GetItemFromString(cropSave.seedToGrow);
            cropToPlant.LoadCrop(cropSave.landID,seedToGrow,cropSave.cropState,cropSave.growth,cropSave.health);
        }
    }
}
