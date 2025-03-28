using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land_Farm : MonoBehaviour, ITimeTracker
{
    public int id;
   public enum LandStatus
   {
        Soil,FarmLand,Watered
   }
    public LandStatus landStatus;
    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;

    public InventoryItem item;

    [SerializeField] private ItemData_Oficial woodData;
    [SerializeField] private ItemData_Oficial rockData;


    public GameObject selectDirt;
    GameTimeStap timeWatered;
    public GameObject cropsPreFabs;
    

    CropsBehaviour cropPlanted = null;

    public enum farmObstacleStatus { None, Rock, Wood, Weeds}
    [Header("Obstacles")]
    public farmObstacleStatus obstacleStatus;
    public GameObject rockPrefab,woodPrefab, grassPrefab;

    GameObject obstacleObject;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        SwitchLandStatus(LandStatus.Soil);
        SelectDirt(false);
        TimeManager.Instance.RegisterTracker(this);
    }
    public void LoadLandData(LandStatus statusToSwitch, GameTimeStap lastWatered, farmObstacleStatus obstacleStatusToSwitch)
    {
        landStatus = statusToSwitch;
        timeWatered = lastWatered;

        Material matToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                matToSwitch = soilMat;
                break;
            case LandStatus.FarmLand:
                matToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                matToSwitch = wateredMat;
                break;
        }
        renderer.material = matToSwitch;
        switch (obstacleStatusToSwitch)
        {
            case farmObstacleStatus.None:
                if (obstacleObject != null)
                    Destroy(obstacleObject);
                break;
            case farmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case farmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case farmObstacleStatus.Weeds:
                obstacleObject = Instantiate(grassPrefab, transform);
                break;
        }
        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x,3f, transform.position.z);

        obstacleStatus = obstacleStatusToSwitch;
    }
    public void SelectDirt(bool toggle) => selectDirt.SetActive(toggle);
    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material matToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                matToSwitch = soilMat;
                break;
            case LandStatus.FarmLand:
                matToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                matToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimesStap();
                break; 
        }
        renderer.material = matToSwitch;

        LandManager.Instance.OnLandStateChange(id,landStatus,timeWatered,obstacleStatus);
    }
    public void SetObstaclesStatus(farmObstacleStatus statusToSwitch)
    {
        switch (statusToSwitch)
        {
            case farmObstacleStatus.None:
                if(obstacleObject != null)
                    Destroy(obstacleObject);
                break;
            case farmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case farmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case farmObstacleStatus.Weeds:
                obstacleObject = Instantiate(grassPrefab, transform);
                break;
        }
       if(obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, 3.15f, transform.position.z);

        obstacleStatus = statusToSwitch;

        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);
    }
    public void InteractDirt()
    {
        ItemData_Equipment equimentTool = Inventory.instance.GetEquipment(EquipmentType.Tool);

        if (equimentTool != null)
        {
            ItemData_Equipment.ToolType tooltype = equimentTool.toolType;

            switch (tooltype)
            {
                case ItemData_Equipment.ToolType.Hoe:

                    SwitchLandStatus(LandStatus.FarmLand);
                    break;
                case ItemData_Equipment.ToolType.WateringCan:
                    if(landStatus != LandStatus.Soil)
                    {
                         SwitchLandStatus(LandStatus.Watered);
                    }
                    break;
                case ItemData_Equipment.ToolType.Hoz:
                    
                    if (cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }
                    if (obstacleStatus == farmObstacleStatus.Weeds) 
                    {
                        SetObstaclesStatus(farmObstacleStatus.None);
                    }
                    break;
                case ItemData_Equipment.ToolType.Axe:
                    if (obstacleStatus == farmObstacleStatus.Wood)
                    {
                        Inventory.instance.AddItem(woodData);
                        SetObstaclesStatus(farmObstacleStatus.None);

                    }
                    break;
                case ItemData_Equipment.ToolType.Pickaxe:
                    if (obstacleStatus == farmObstacleStatus.Rock)
                    {
                        Inventory.instance.AddItem(rockData);
                        SetObstaclesStatus(farmObstacleStatus.None);
                    }
                    break;
            }
            return;
        }
    }
    public void SeedToolsCrop()
    {
        ItemData_Equipment seedTools = Inventory.instance.GetEquipment(EquipmentType.Seeds);

        SeedData seedTool = seedTools as SeedData;
        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null && obstacleStatus == farmObstacleStatus.None)
        {
            SpawnCrop();
            cropPlanted.Plant(id,seedTool);
            Inventory.instance.UnequipItem(seedTools);
           
            Inventory.instance.UpdateSlotsUI();
            
            
            
        }
    }
    public CropsBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropsPreFabs, transform);

        cropObject.transform.position = new Vector3(transform.position.x, 3f, transform.position.z);

        cropPlanted = cropObject.GetComponent<CropsBehaviour>();
        return cropPlanted;
    }
    public void ClockUpdate(GameTimeStap timestamp)
    {
        if (landStatus == LandStatus.Watered)
        {
            int hoursElapsed = GameTimeStap.CompareTimeStamps(timeWatered, timestamp);
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }
            if (hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.FarmLand);
            }
        }
        if (landStatus != LandStatus.Watered && cropPlanted != null)
        {
            if (cropPlanted.cropStage != CropsBehaviour.CropStage.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }
    private void OnDestroy()
    {
        TimeManager.Instance.UnRegisterTracker(this);
    }
}
