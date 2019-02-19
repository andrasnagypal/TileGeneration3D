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
        
        public float[] PositionParameters;
        public float[] RotationParameters;
        public int TileType;
        public int IndexX, IndexY; // tömbből való megkereséshez
        public TILE[] NeighbourTiles;
        public int[] IDForSorroundingRoads;
        public int[] IDNumberOfSorroundingSettlements;
    }

    public struct ROAD
    {
        public int IDNumberForRoad;
        public float[] PositionParameters;
        public float[] RotationParameters;
        public byte LevelOfTheRoad;
        public int[] IDNumberForTriangles;
    }
    public struct PLAINTRIANGLE
    {
        public int IDNumberForTriangle;
        public float[] PositionParameters;
        public float[] RotationParameters;
        public bool IsHabitable;
        public byte[] TypeOfTilesForSettlement;//enumja a field típusának
        public int[] RoadsToTheSettlement;
    }
    public struct AStarNode
    {
        public int IDNumberOfNode;
        //Cost of road
        public float GCost;
        //Cost of distance
        public float HCost;
        //szomszéd node-ok
        public int[] IDNumberForPossiblePaths;       
        
    }
    public struct AStarPath
    {
        //GCost+HCost
        public float FCost;
        //a path idje
        public int IDNumberOfNode;
    }

    public class TileContainer : MonoBehaviour
    {
        public TILE AttributesOfTheTile;
       

        public void ShowInfo(int x, int y)
        {            
            AttributesOfTheTile.IndexX = x;
            AttributesOfTheTile.IndexY = y;            
            AttributesOfTheTile.NeighbourTiles = new TILE[6];
            AttributesOfTheTile.IDForSorroundingRoads=new int[6] { -1,-1,-1,-1,-1,-1};
            AttributesOfTheTile.IDNumberOfSorroundingSettlements = new int[6] { -1,-1,-1,-1,-1,-1};
            //road generaláshoz
            for (int i = 0; i < 6; i++)
            {

            }
            transform.Rotate(new Vector3(AttributesOfTheTile.RotationParameters[(int)RotationNames.RotX],
                AttributesOfTheTile.RotationParameters[(int)RotationNames.RotY],
                AttributesOfTheTile.RotationParameters[(int)RotationNames.RotZ]));
            gameObject.name= "TILE" + x + y;
            //Debug.Log(AttributesOfTheTile.SorroundingRoads[0].IDNumberForTriangles  ); ez alapján roadokat
        }
    }
}
