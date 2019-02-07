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
    public enum TileDirections
    {
        West, NorthWest, NorthEast, East, SouthEast, SouthWest
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
        public UNINHABITEDSETTLEMENT[] SorroundingSettlements;
    }

    public struct ROAD
    {
        public string IdForGameObject;
    }
    public struct UNINHABITEDSETTLEMENT
    {
        public string IdForGameObject;
        public float[] PositionParameters;
        public float[] RotationParameters;
        public bool IsHabitable;
        public byte[] TilesOfSettlement;
    }


    public class TileContainer : MonoBehaviour
    {
        public TILE AttributesOfTheTile;
       

        public void ShowInfo(int x, int y)
        {
            AttributesOfTheTile.IdForGameObject = "TILE" + x + y;
            AttributesOfTheTile.IndexX = x;
            AttributesOfTheTile.IndexY = y;            
            AttributesOfTheTile.NeighbourTiles = new TILE[6];
            AttributesOfTheTile.SorroundingRoads=new ROAD[6];
            AttributesOfTheTile.SorroundingSettlements=new UNINHABITEDSETTLEMENT[6];            
        transform.Rotate(new Vector3(AttributesOfTheTile.RotationParameters[(int)RotationNames.RotX],
                AttributesOfTheTile.RotationParameters[(int)RotationNames.RotY],
                AttributesOfTheTile.RotationParameters[(int)RotationNames.RotZ]));
            gameObject.name= "TILE" + x + y;
        }
    }
}
