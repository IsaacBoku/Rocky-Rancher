using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Land_Farm;

[System.Serializable]
public struct LandSaveState 
{
    public Land_Farm.LandStatus landStatus;
    public GameTimeStap lastWatered;
    public Land_Farm.farmObstacleStatus obstacleStatus;
    public LandSaveState(Land_Farm.LandStatus landStatus,GameTimeStap lastWatered, Land_Farm.farmObstacleStatus obstacleStatus)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
        this.obstacleStatus = obstacleStatus;
    }
    public void ClockUpdate(GameTimeStap timestamp)
    {
        if (landStatus == LandStatus.Watered)
        {
            int hoursElapsed = GameTimeStap.CompareTimeStamps(lastWatered, timestamp);
            if (hoursElapsed > 24)
            {
                landStatus = LandStatus.FarmLand;
            }
        }
    }
}
