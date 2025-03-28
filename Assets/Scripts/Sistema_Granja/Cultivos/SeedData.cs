using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed Data", menuName = "Data/Seed")]
public class SeedData : ItemData_Equipment
{
    public int daysToGrow;
    public ItemData_Oficial cropsToYield;

    public GameObject seedling;

    public bool regrowable;
    public int dayToRegrow;
}
