using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public class GameStarter : MonoBehaviour
    {
        public GameObject StartingTile;
        public int MaxX, MaxY;
        public Vector3[,] TilePositions;
        public GameObject[,] TilesOnBoard;
        public float LeapAmountX = 1.015f;
        public float LeapAmountY = 2.3f;
        public int HowManyTilesToGenerate;
        TILE newTile;
        
        void Start()
        {
            TilePositions = new Vector3[MaxX, MaxY];
            TilesOnBoard = new GameObject[MaxX, MaxY];
            CreatePositions();
            CreateTiles();
        }

        void CreatePositions()
        {
            float StartPosX = 0;
            float StartPosY = 0;
            for (int i = 0; i < MaxX; i++)
            {
                if (i % 2 == 0)
                    StartPosY = 0;
                else
                    StartPosY = LeapAmountY/2;

                for (int j = 0; j < MaxY; j++)
                {
                    TilePositions[i, j] = new Vector3(StartPosX, StartPosY, 0);
                    StartPosY += LeapAmountY;
                }
                StartPosX += LeapAmountX;
            }
        }
        public void CreateTiles()
        {
            int TileCounter = 0;
            while (TileCounter<HowManyTilesToGenerate)
            {
                int rndx=UnityEngine.Random.Range(0,MaxX), rndy= UnityEngine.Random.Range(0, MaxY);
                while (TilesOnBoard[rndx,rndy]!=null)
                {
                    rndx = UnityEngine.Random.Range(0, MaxX-3);
                    rndy = UnityEngine.Random.Range(0, MaxY-3);
                }
                TilesOnBoard[rndx, rndy] = Instantiate(StartingTile, TilePositions[rndx, rndy], Quaternion.identity);
                TilesOnBoard[rndx, rndy].transform.Rotate(0, 0, 90);
                TileCounter++;
            }
        }

        void TileSetter()
        {
            newTile = new TILE();
            // tömbből való megkereséshez
            newTile.IdForGameObject = "valami";
            newTile.PositionParameters = new float[] { 0, 0, 0 };
            newTile.RotationParameters = new float[] { 0, 0, 90 };
            newTile.TileType = (int)TypeOfTile.Field;

            GameObject GO = Instantiate(StartingTile, new Vector3(newTile.PositionParameters[(int)VectorPositionNames.PosX],
                newTile.PositionParameters[(int)VectorPositionNames.PosY],
                 newTile.PositionParameters[(int)VectorPositionNames.PosZ]), Quaternion.identity);
            GO.GetComponent<TileContainer>().AttributesOfTheTile = newTile;
            GO.GetComponent<TileContainer>().ShowInfo();
        }

        //void CreateHexagons()
        //{
        //    lista = StartingTile.GetComponentsInChildren<HexagonController>();
        //    for (int i = 0; i < lista.Length; i++)
        //    {
        //        if (!lista[i].IsFullyNeighboured())
        //            NotAllNeighbourIsSet.Add(lista[i]);
        //    }

        //    int rnd = UnityEngine.Random.Range(0, NotAllNeighbourIsSet.Count);
        //    NotAllNeighbourIsSet[rnd].PopulateNeighbours();
        //    NotAllNeighbourIsSet.Clear();
        //}
    //     for (int i = 0; i<HowManyIteration; i++)
    //        {
    //            CreateHexagons();
    //}
    //        foreach (HexagonController con in StartingTile.GetComponentsInChildren<HexagonController>())
    //        {
    //            con.SetUpRoads();
    //        }
    //        foreach (HexagonController con in StartingTile.GetComponentsInChildren<HexagonController>())
    //        {
    //            con.ConnectAllTilesTogether();
    //        }
    }
}
