using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerate : MonoBehaviour
{
    [Range(1f, 100f)]
    public int percentageFilled;
    public void GenerateObstacles(List<Land_Farm> landPlots)
    {
        int plotsToFill = Mathf.RoundToInt((float) percentageFilled / 100 * landPlots .Count);

        List<int> shuffledlist = ShuffleLandIndexes(landPlots.Count);
        for(int i = 0; i  <plotsToFill; i++)
        {
            int index = shuffledlist[i];
            Land_Farm.farmObstacleStatus status = (Land_Farm.farmObstacleStatus) Random.Range(1, 4);
            landPlots[index].SetObstaclesStatus(status);
        }
    }
    List<int> ShuffleLandIndexes(int count)
    {
        List<int> listToReturn = new List<int>();
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, i + 1);
            listToReturn.Insert(index, i);
        }
        return listToReturn;
    }
}
