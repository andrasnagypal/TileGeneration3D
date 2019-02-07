﻿using System;
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
        int TileCounterForSpawning = 0;
        public GameObject ParentObjectForTiles;
        TILE newTile;
        int rndx, rndy;
        List<int[]> TilesSpawned = new List<int[]>();

        void Start()
        {
            TilePositions = new Vector3[MaxX, MaxY];
            TilesOnBoard = new GameObject[MaxX, MaxY];
            CreatePositions();
            //CreateTiles();
            AddTiles();
            SetCamera();
        }

        private void SetCamera()
        {
            Camera.main.transform.position = TilePositions[TilesSpawned[0][0], TilesSpawned[0][1]]+new Vector3(0,1,-10);
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
                    StartPosY = LeapAmountY / 2;

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
            while (TileCounter < HowManyTilesToGenerate)
            {
                rndx = UnityEngine.Random.Range(0, MaxX);
                rndy = UnityEngine.Random.Range(0, MaxY);
                while (TilesOnBoard[rndx, rndy] != null)
                {
                    rndx = UnityEngine.Random.Range(0, MaxX - 3);
                    rndy = UnityEngine.Random.Range(0, MaxY - 3);
                }

                TileSetter(rndx, rndy);
                TileCounter++;
            }
        }

        void TileSetter(int x, int y)
        {
            newTile = new TILE();
            // tömbből való megkereséshez

            newTile.PositionParameters = new float[] { TilePositions[x,y].x, TilePositions[x, y].y, TilePositions[x, y].z };
            newTile.RotationParameters = new float[] { 0, 0, 90 };
            newTile.TileType = (int)TypeOfTile.Field;

            TilesOnBoard[x, y] = Instantiate(StartingTile, TilePositions[x, y], Quaternion.identity);
            TilesOnBoard[x, y].GetComponent<TileContainer>().AttributesOfTheTile = newTile;
            TilesOnBoard[x, y].GetComponent<TileContainer>().ShowInfo(x, y);
            TilesOnBoard[x, y].transform.parent = ParentObjectForTiles.transform;
            TilesSpawned.Add(new int[] { x, y });

        }

        public void AddTiles()
        {
            FirstTileToCreate();
            while (TileCounterForSpawning < HowManyTilesToGenerate)
            {
                bool IsTileCreated = false;
                while (!IsTileCreated)
                {
                    int rndForList = UnityEngine.Random.Range(0, TilesSpawned.Count);
                    TILE tempTile = TilesOnBoard[TilesSpawned[rndForList][0], TilesSpawned[rndForList][1]].GetComponent<TileContainer>().AttributesOfTheTile;
                    int rndForTileNeighbourGeneration = UnityEngine.Random.Range(0, 6);
                    //páros sorban levő tile kezelése
                    if (tempTile.IndexX % 2 == 0)
                        switch (rndForTileNeighbourGeneration)
                        {
                            case 0:
                                {
                                    //a West szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] - 2,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx,Boardy]==null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    //a NorthWest szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] - 1,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    //a NorthEast szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] + 1,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    //a East szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] + 2,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    //a SouthEast szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] + 1,
                                        Boardy = TilesSpawned[rndForList][1] - 1;
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    //a SouthWest szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] - 1,
                                        Boardy = TilesSpawned[rndForList][1] + 1;
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;

                        }
                    //páratlan sorban levő tile kezelése
                    else
                        switch (rndForTileNeighbourGeneration)
                        {
                            case 0:
                                {
                                    //a West szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] - 2,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    //a NorthWest szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] - 1,
                                        Boardy = TilesSpawned[rndForList][1] + 1;
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    //a NorthEast szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] + 1,
                                        Boardy = TilesSpawned[rndForList][1] + 1;
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    //a East szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] + 2,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    //a SouthEast szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] + 1,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    //a SouthWest szomszéd tile indexeinek beállítása
                                    int Boardx = TilesSpawned[rndForList][0] - 1,
                                        Boardy = TilesSpawned[rndForList][1];
                                    if (TilesOnBoard[Boardx, Boardy] == null)
                                    {
                                        TileSetter(Boardx, Boardy);
                                        IsTileCreated = true;
                                    }
                                }
                                break;
                        }
                }
                TileCounterForSpawning++;
            }
            
        }

        private void FirstTileToCreate()
        {
            if (TilesSpawned.Count == 0)
            {
                rndx = UnityEngine.Random.Range(5, MaxX - 5);
                rndy = UnityEngine.Random.Range(5, MaxY - 5);
                TileSetter(rndx,rndy);
                TilesSpawned.Add(new int[] { rndx, rndy });
                TileCounterForSpawning++;
            }
        }

    }

    
}

