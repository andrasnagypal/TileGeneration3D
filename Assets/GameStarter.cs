using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public class GameStarter : MonoBehaviour
    {
        [Tooltip("Tile that will be spawned")]
        public GameObject StartingTile;
        public GameObject SettlementTriangle;
        public int MaxX, MaxY;
        public Vector3[,] TilePositions;
        public GameObject[,] TilesOnBoard;
        [Tooltip("Left or right leap for tile spawning")]
        public float LeapAmountX = 1.015f;
        [Tooltip("Up or down leap for tile spawning")]
        public float LeapAmountY = 2.3f;
        [Tooltip("The Amount of tiles that will be spawned")]
        public int HowManyTilesToGenerate;
        [Tooltip("Min 8 to avoid crash")]
        [Range(8, 20)]
        public int BorderValue = 6;
        int TileCounterForSpawning = 0;
        public GameObject ParentObjectForTiles;
        TILE newTile;
        int rndx, rndy;
        int IDCounterForTriangles = 1;
        List<int[]> TilesSpawned = new List<int[]>();
        List<ROAD> RoadsOfTheGame = new List<ROAD>();
        List<PLAINTRIANGLE> PlainTrianglesAtTheBeginning = new List<PLAINTRIANGLE>();
        List<GameObject> ListOfSettlementPlaces = new List<GameObject>();

        void Start()
        {
            TilePositions = new Vector3[MaxX, MaxY];
            TilesOnBoard = new GameObject[MaxX, MaxY];
            CreatePositions();
            AddTiles();
            SetCamera();
            for (int i = 0; i < 3; i++)
            {
                FillInTheGaps();
            }
            DestroyVector3s();
           //PutDownSettlementTriangles();
        }

        void PutDownSettlementTriangles()
        {
            for (int i = 0; i < TilesSpawned.Count; i++)
            {
                TILE currentTileAttributes = TilesOnBoard[TilesSpawned[i][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile;
                for (int j = 0; j < 6; j++)
                {
                    if (currentTileAttributes.IDNumberOfSorroundingSettlements[j] < 0)
                    {
                        PLAINTRIANGLE newTriangle;
                        newTriangle.IDNumberForTriangle = IDCounterForTriangles++;
                        //hogy később ne legyen vonzó simán visszaadni az currentattributes-ot a boardosba
                        TilesOnBoard[TilesSpawned[i][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[j] = newTriangle.IDNumberForTriangle;
                        newTriangle.TypeOfTilesForSettlement = new byte[3] { 255, 255, 255 };
                        newTriangle.TypeOfTilesForSettlement[0] = (byte)currentTileAttributes.TileType;
                        //ha páros sorú tile akkor ez az ág
                        if (currentTileAttributes.IndexX % 2 == 0)
                        {
                            //West "jelű" triangle kezelése
                            if (j == (int)TileDirections.West)
                            {
                                //West tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i - 2][0], TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i - 2][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i - 2][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast] = newTriangle.IDNumberForTriangle;
                                }
                                //NorthWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i - 1][0], TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i - 1][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i - 1][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast] = newTriangle.IDNumberForTriangle;
                                }
                                //ishabitable beállítása, h később kevesebb mint 3 tile-ú trianglera ne rakjon a game
                                newTriangle.IsHabitable = true;
                                for (int k = 0; k < 3; k++)
                                {
                                    if (newTriangle.TypeOfTilesForSettlement[k] == 255)
                                    {
                                        newTriangle.IsHabitable = false;
                                        break;
                                    }
                                }
                                float posx= currentTileAttributes.PositionParameters[0]-1.015f;
                                float posy = currentTileAttributes.PositionParameters[1] + 0.54f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                GameObject go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);
                                ListOfSettlementPlaces.Add(go);
                            }
                        }
                        //ha páratlan soró tile akkor ez az ág
                        else
                        {

                        }
                    }


                }
            }

        }


        private void DestroyVector3s()
        {
            TilePositions = null;

        }

        private void FillInTheGaps()
        {
            for (int i = 0; i < MaxX; i++)
            {
                for (int j = 0; j < MaxY; j++)
                {
                    if (TilesOnBoard[i, j] != null)
                    {
                        //ne fusson ki a tömbből
                        if (i >= BorderValue &&
                            j >= BorderValue &&
                            i <= MaxX - BorderValue &&
                            j <= MaxY - BorderValue)
                            //ha páros hexagonon áll
                            if (i % 2 == 0)
                            {
                                //West Tile generálás ha nincs + ha 2-vel odébb van tile
                                if (TilesOnBoard[i - 2, j] == null && (TilesOnBoard[i - 4, j] || TilesOnBoard[i - 6, j]))
                                {
                                    TileSetter(i - 2, j);
                                }

                                //East Tile generálás ha nincs + ha 2-vel odébb van tile
                                if (TilesOnBoard[i + 2, j] == null && (TilesOnBoard[i + 4, j] || TilesOnBoard[i + 6, j]))
                                {
                                    TileSetter(i + 2, j);
                                }
                            }
                            else
                            {
                                //West Tile generálás ha nincs + ha 2-vel odébb van tile
                                if (TilesOnBoard[i - 2, j] == null && (TilesOnBoard[i - 4, j] || TilesOnBoard[i - 6, j]))
                                {
                                    TileSetter(i - 2, j);
                                }

                                //East Tile generálás ha nincs + ha 2-vel odébb van tile
                                if (TilesOnBoard[i + 2, j] == null && (TilesOnBoard[i + 4, j] || TilesOnBoard[i + 6, j]))
                                {
                                    TileSetter(i + 2, j);
                                }
                            }

                    }
                }
            }
        }

        private void SetCamera()
        {
            Camera.main.transform.position = TilePositions[TilesSpawned[0][0], TilesSpawned[0][1]] + new Vector3(0, 1, -2);
        }

        void CreatePositions()
        {
            float StartPosX = 0;
            float StartPosY = 0;
            for (int i = 0; i < MaxX; i++)
            {
                if (i % 2 == 0)
                    StartPosY = .26f;
                else
                    StartPosY = LeapAmountY / 2 + .26f;

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

            newTile.PositionParameters = new float[] { TilePositions[x, y].x, TilePositions[x, y].y, TilePositions[x, y].z };
            newTile.RotationParameters = new float[] { -90, 0, 0 };
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

                    //ne fusson ki a tömbből
                    if (TilesSpawned[rndForList][0] >= BorderValue &&
                        TilesSpawned[rndForList][1] >= BorderValue &&
                        TilesSpawned[rndForList][0] <= MaxX - BorderValue &&
                        TilesSpawned[rndForList][1] <= MaxY - BorderValue)
                        //páros sorban levő tile kezelése
                        if (tempTile.IndexX % 2 == 0)
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
                                            Boardy = TilesSpawned[rndForList][1] - 1;
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
                TileSetter(rndx, rndy);
                TilesSpawned.Add(new int[] { rndx, rndy });
                TileCounterForSpawning++;
            }
        }
    }
    }

    


