using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nagand
{
    enum VectorPositionNames
    {
        PosX,
        PosY,
        PosZ
    }
    enum RotationNames
    {
        RotX,
        RotY,
        RotZ
    }
    enum TypeOfTile
    {
        Field,
        Mountain
    }


    public struct TILE
    {
        public string IdForGameObject;
        public float[] PositionParameters;
        public float[] RotationParameters;
        public int TileType;
        public int IndexX, IndexY; // tömbből való megkereséshez
        public TILE[] NeighbourTiles;
        public ROAD[] SorroundingRoads;
    }

    public struct ROAD
    {
        public string IdForGameObject;
    }


    public class TileContainer : MonoBehaviour
    {
        public TILE AttributesOfTheTile;
       

        public void ShowInfo()
        {
            Debug.Log(AttributesOfTheTile.IdForGameObject + " " + AttributesOfTheTile.TileType);
            
            transform.Rotate(new Vector3(AttributesOfTheTile.RotationParameters[(int)RotationNames.RotX],
                AttributesOfTheTile.RotationParameters[(int)RotationNames.RotY],
                AttributesOfTheTile.RotationParameters[(int)RotationNames.RotZ]));
        }
    }
}
