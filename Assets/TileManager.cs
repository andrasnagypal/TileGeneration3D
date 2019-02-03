using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject Tile;
    public int SizeX,SizeY;
    public float TileSize = 3.25f;
    public float RoadSize = 1f;

    float PosX=0, PosY;

    private void Awake()
    {
        for (int i = 0; i < SizeX; i++)
        {
            if (i % 2 == 0)
            {
                PosY = 0;
            }
            else PosY = TileSize / 2+ RoadSize;
            for (int j = 0; j < SizeY; j++)
            {
                Vector3 tempPos = new Vector3(PosX, PosY, 0);
                Instantiate(Tile, tempPos, Quaternion.identity);
                PosY += TileSize+RoadSize*2;
            }
            PosX += TileSize +RoadSize;
        }
    }
}
