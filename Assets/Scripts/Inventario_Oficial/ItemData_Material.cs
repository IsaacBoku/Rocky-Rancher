using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MaterialType
{
    Material, Fruits, Gifts

}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Materials")]
public class ItemData_Material : ItemData_Oficial
{
    public MaterialType MaterialType;
    private int descriptionLength;
    public override string GetDescription()
    {
        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        return sb.ToString();
    }
}
