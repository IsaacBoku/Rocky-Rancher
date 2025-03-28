using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;

    public InventoryItem[] toolSlots;
    public InventoryItem[] itemSlots;

    public InventoryItem equippedItemSlot;
    public InventoryItem equippedToolSlot;

    public GameTimeStap timestamp;

    public GameSaveState(List<LandSaveState> landData, List<CropSaveState> cropData, GameTimeStap timestamp)
    {
        this.landData = landData;
        this.cropData = cropData;

        this.timestamp = timestamp;
    }
}
