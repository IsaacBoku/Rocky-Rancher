using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTerrainTexture : MonoBehaviour
{
    public Transform playerTransform;
    public Terrain terrainObject;

    public int posX;
    public int posZ;
    public float[] textureValues;

    private void Start()
    {
        terrainObject = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
    }
    private void Update()
    {
        terrainObject = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
        GetterrainTexture();
    }
    public void GetterrainTexture()
    {
        UpdatePosition();
        CheckTexture();
    }
    void UpdatePosition()
    {
        Vector3 terrainPosition = playerTransform.position - terrainObject.GetPosition();
        Vector3 mapPosition = new Vector3 (terrainPosition.x/terrainObject.terrainData.size.x, 0,terrainPosition.z/terrainObject.terrainData.size.z);
        float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrainObject .terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    void CheckTexture()
    {
        float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(posX, posZ,1,1);
        textureValues[0] = splatMap[0,0,0];
        textureValues[1] = splatMap[0, 0, 1];
        textureValues[2] = splatMap[0, 0, 2];
        textureValues[3] = splatMap[0, 0, 3];
    }

}
