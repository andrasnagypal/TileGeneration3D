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
        public GameObject StartingRoad;
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
        public GameObject ParentObjectForTriangles;
        public GameObject ParentObjectForRoads;
        TILE newTile;
        int rndx, rndy;
        int IDCounterForGameObjects= 0;
        
        List<int[]> TilesSpawned = new List<int[]>();
        List<ROAD> RoadsOfTheGame = new List<ROAD>();
        List<PLAINTRIANGLE> PlainTrianglesAtTheBeginning = new List<PLAINTRIANGLE>();
        List<GameObject> ListOfSettlementPlaces = new List<GameObject>();
        List<GameObject> ListOfRoads = new List<GameObject>();
        GameObject go;

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
            PutDownSettlementTriangles();
            PutDownRoads();
            //FindObjectOfType<AStarCalculator>().AStarCalc();
            
        }

       

        private void PutDownRoads()
        {
            for (int i = 0; i < TilesSpawned.Count; i++)
            {
                TILE currentTileAttributes = TilesOnBoard[TilesSpawned[i][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile;
                for (int j = 0; j < 6; j++)
                {
                    if (currentTileAttributes.IDForSorroundingRoads[j]<0)
                    {
                        ROAD newRoad;
                        newRoad.LevelOfTheRoad = 6;
                        newRoad.IDNumberForTriangles = new int[2] { -1, -1 };
                        newRoad.IDNumberForRoad = IDCounterForGameObjects++;
                        //páros indexű tilek kezelése
                        if (currentTileAttributes.IndexX % 2 == 0)
                        {    //West road inicializálása
                            if (j == (byte)TileDirections.West)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] - 1.016f,
                             currentTileAttributes.PositionParameters[1],
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 90 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] - 1.016f,
                                    currentTileAttributes.PositionParameters[1],
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.West];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest];
                                                             
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //west tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.East] <0)
                                    TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.East] = newRoad.IDNumberForRoad;
                            }
                            //NorthWest road inicializálása
                            if (j == (byte)TileDirections.NorthWest)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] - 1.016f/2,
                             currentTileAttributes.PositionParameters[1]+.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 30 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] - 1.016f / 2,
                                    currentTileAttributes.PositionParameters[1],
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.West];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest];
                                                              
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //northwest tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthEast] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthEast] = newRoad.IDNumberForRoad;
                            }
                            //NorthEast road inicializálása
                            if (j == (byte)TileDirections.NorthEast)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] + 1.016f/2,
                             currentTileAttributes.PositionParameters[1]+.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, -30 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] + 1.016f / 2,
                                    currentTileAttributes.PositionParameters[1] + .88f,
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast];
                                
                                
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //northeast tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthWest] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthWest] = newRoad.IDNumberForRoad;
                            }
                            //East road inicializálása
                            if (j == (byte)TileDirections.East)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] + 1.016f,
                             currentTileAttributes.PositionParameters[1],
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 90 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] + 1.016f ,
                                    currentTileAttributes.PositionParameters[1],
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.East];
                               
                                
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //east tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.West] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.West] = newRoad.IDNumberForRoad;
                            }
                            //SouthEast road inicializálása
                            if (j == (byte)TileDirections.SouthEast)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] + 1.016f/2,
                             currentTileAttributes.PositionParameters[1]-.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 30 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] + 1.016f / 2,
                                    currentTileAttributes.PositionParameters[1] - .88f,
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.East];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast];
                                                             
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //southeast tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] - 1]&&TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]-1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthWest] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]-1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthWest] = newRoad.IDNumberForRoad;
                            }
                            //SouthWest road inicializálása
                            if (j == (byte)TileDirections.SouthWest)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] - 1.016f/2,
                             currentTileAttributes.PositionParameters[1]-.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, -30 };
                                Vector3 pos = new Vector3(newRoad.PositionParameters[0] ,
                                    newRoad.PositionParameters[1] ,
                                    newRoad.PositionParameters[2]);
                               go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest];
                               
                               
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //southwest tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1]&&TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthEast] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthEast] = newRoad.IDNumberForRoad;
                            }
                        }
                        //páratlan indexű tile kezelése
                        else
                        {
                            if (j == (byte)TileDirections.West)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] - 1.016f,
                             currentTileAttributes.PositionParameters[1],
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 90 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] - 1.016f,
                                    currentTileAttributes.PositionParameters[1],
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.West];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest];
                                                             
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //west tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.East] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.East] = newRoad.IDNumberForRoad;
                            }
                            //NorthWest road inicializálása
                            if (j == (byte)TileDirections.NorthWest)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] - 1.016f/2,
                             currentTileAttributes.PositionParameters[1]+.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 30 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] - 1.016f / 2,
                                    currentTileAttributes.PositionParameters[1],
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.West];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest];
                                                             
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //northwest tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] + 1]&&TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]+1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthEast] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] -1, TilesSpawned[i][1]+1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthEast] = newRoad.IDNumberForRoad;
                            }
                            //NorthEast road inicializálása
                            if (j == (byte)TileDirections.NorthEast)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] + 1.016f/2,
                             currentTileAttributes.PositionParameters[1]+.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, -30 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] + 1.016f / 2,
                                    currentTileAttributes.PositionParameters[1] + .88f,
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast];
                                                             
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //northeast tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1]&&TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]+1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthWest] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]+1].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.SouthWest] = newRoad.IDNumberForRoad;
                            }
                            //East road inicializálása
                            if (j == (byte)TileDirections.East)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] + 1.016f,
                             currentTileAttributes.PositionParameters[1],
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 90 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] + 1.016f,
                                    currentTileAttributes.PositionParameters[1],
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.East];
                                                          
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //east tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.West] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.West] = newRoad.IDNumberForRoad;
                            }
                            //SouthEast road inicializálása
                            if (j == (byte)TileDirections.SouthEast)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] + 1.016f/2,
                             currentTileAttributes.PositionParameters[1]-.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, 30 };
                                Vector3 pos = new Vector3(currentTileAttributes.PositionParameters[0] + 1.016f / 2,
                                    currentTileAttributes.PositionParameters[1] - .88f,
                                    currentTileAttributes.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.East];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast];
                                                             
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //southeast tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthWest] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthWest] = newRoad.IDNumberForRoad;
                            }
                            //SouthWest road inicializálása
                            if (j == (byte)TileDirections.SouthWest)
                            {
                                newRoad.PositionParameters = new float[3] { currentTileAttributes.PositionParameters[0] - 1.016f/2,
                             currentTileAttributes.PositionParameters[1]-.88f,
                            currentTileAttributes.PositionParameters[2]};
                                newRoad.RotationParameters = new float[3] { 0, 0, -30 };
                                Vector3 pos = new Vector3(newRoad.PositionParameters[0],
                                    newRoad.PositionParameters[1],
                                    newRoad.PositionParameters[2]);
                                go = Instantiate(StartingRoad, pos, Quaternion.identity);

                                newRoad.IDNumberForTriangles[0] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast];
                                newRoad.IDNumberForTriangles[1] = currentTileAttributes.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest];
                                                            
                                go.GetComponent<RoadController>().AttributesOfTheRoad = newRoad;
                                go.GetComponent<RoadController>().SetUp();
                                currentTileAttributes.IDForSorroundingRoads[j] = newRoad.IDNumberForRoad;
                                ListOfRoads.Add(go);
                                RoadsOfTheGame.Add(newRoad);
                                //southwest tile-ba belerakni roadot
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]]&&TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthEast] < 0)
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDForSorroundingRoads[(int)TileDirections.NorthEast] = newRoad.IDNumberForRoad;
                            }
                        }
                        go.transform.parent = ParentObjectForRoads.transform;
                        
                    }
                }
            }
            GiveRoadsToRoadsContainer();
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
                        newTriangle.IDNumberForTriangle = IDCounterForGameObjects++;
                        //hogy később ne legyen vonzó simán visszaadni az currentattributes-ot a boardosba
                        TilesOnBoard[TilesSpawned[i][0], TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[j] = newTriangle.IDNumberForTriangle;
                        newTriangle.TypeOfTilesForSettlement = new byte[3] { 255, 255, 255 };
                        newTriangle.TypeOfTilesForSettlement[0] = (byte)currentTileAttributes.TileType;
                        newTriangle.RoadsToTheSettlement = new int[3] { -1,-1,-1 };
                        //ha páros sorú tile akkor ez az ág
                        if (currentTileAttributes.IndexX % 2 == 0)
                        {
                            //West "jelű" triangle kezelése
                            if (j == (int)TileDirections.West)
                            {
                                //West tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i ][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i ][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast] = newTriangle.IDNumberForTriangle;
                                }
                                //NorthWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i ][0] - 1, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i ][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i ][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast] = newTriangle.IDNumberForTriangle;
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
                                float posx= currentTileAttributes.PositionParameters[0]-1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] + .59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);           
                               go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //NorthWest "jelű" triangle kezelése
                            if (j == (int)TileDirections.NorthWest)
                            {
                                //NortWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.East] = newTriangle.IDNumberForTriangle;
                                }
                                //NorthEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0] ;
                                float posy = currentTileAttributes.PositionParameters[1] + 1.175f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 180 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                              
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //NorthEast "jelű" triangle kezelése
                            if (j == (int)TileDirections.NorthEast)
                            {
                                //NortEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0]+ 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast] = newTriangle.IDNumberForTriangle;
                                }
                                //East tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.West] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0]+1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] + .59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //East "jelű" triangle kezelése
                            if (j == (int)TileDirections.East)
                            {
                                //East tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest] = newTriangle.IDNumberForTriangle;
                                }
                                //SouthEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1]-1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0]+1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] -.59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 180 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //SouthEast "jelű" triangle kezelése
                            if (j == (int)TileDirections.SouthEast)
                            {
                                //SouthEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] - 1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.West] = newTriangle.IDNumberForTriangle;
                                }
                                //SoutWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] -1, TilesSpawned[i][1] - 1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0] ;
                                float posy = currentTileAttributes.PositionParameters[1] -1.175f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                              
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //SoutWest "jelű" triangle kezelése
                            if (j == (int)TileDirections.SouthWest)
                            {
                                //SouthWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] - 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest] = newTriangle.IDNumberForTriangle;
                                }
                                //West tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1] ] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.East] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0] - 1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] - .59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 180 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                        }
                        //ha páratlan soró tile akkor ez az ág
                        else
                        {
                            //West "jelű" triangle kezelése
                            if (j == (int)TileDirections.West)
                            {
                                //West tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast] = newTriangle.IDNumberForTriangle;
                                }
                                //NorthWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]+1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]+1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]+1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0] - 1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] + .59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //NorthWest "jelű" triangle kezelése
                            if (j == (int)TileDirections.NorthWest)
                            {
                                //NorthWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]+1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0]- 1, TilesSpawned[i][1] + 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] + 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.East] = newTriangle.IDNumberForTriangle;
                                }
                                //NorthEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0];
                                float posy = currentTileAttributes.PositionParameters[1] + 1.175f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 180 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //NorthEast "jelű" triangle kezelése
                            if (j == (int)TileDirections.NorthEast)
                            {
                                //NortEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] + 1].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthEast] = newTriangle.IDNumberForTriangle;
                                }
                                //East tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.West] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0] + 1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] + .59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                               
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //East "jelű" triangle kezelése
                            if (j == (int)TileDirections.East)
                            {
                                //East tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 2, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.SouthWest] = newTriangle.IDNumberForTriangle;
                                }
                                //SouthEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0]+1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] -.59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 180 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //SouthEast "jelű" triangle kezelése
                            if (j == (int)TileDirections.SouthEast)
                            {
                                //SouthEast tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] + 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.West] = newTriangle.IDNumberForTriangle;
                                }
                                //SoutWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] ] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthEast] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0];
                                float posy = currentTileAttributes.PositionParameters[1] - 1.175f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 0 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                            //SoutWest "jelű" triangle kezelése
                            if (j == (int)TileDirections.SouthWest)
                            {
                                //SouthWest tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[1] = (byte)TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 1, TilesSpawned[i][1]].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.NorthWest] = newTriangle.IDNumberForTriangle;
                                }
                                //West tileba bepakolni idjét
                                if (TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1] ] != null)
                                {
                                    newTriangle.TypeOfTilesForSettlement[2] = (byte)TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.TileType;
                                    TilesOnBoard[TilesSpawned[i][0] - 2, TilesSpawned[i][1] ].GetComponent<TileContainer>().AttributesOfTheTile.IDNumberOfSorroundingSettlements[(int)TileDirections.East] = newTriangle.IDNumberForTriangle;
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
                                float posx = currentTileAttributes.PositionParameters[0] - 1.016f;
                                float posy = currentTileAttributes.PositionParameters[1] - .59f;
                                //3 koordináta beállítása
                                newTriangle.PositionParameters = new float[3] {
                                    posx,
                                    posy,
                                    currentTileAttributes.PositionParameters[2]
                                };
                                //forgás beállítása
                                newTriangle.RotationParameters = new float[3] { 0, 0, 180 };
                                PlainTrianglesAtTheBeginning.Add(newTriangle);
                                go = Instantiate(SettlementTriangle, new Vector3(
                                    newTriangle.PositionParameters[0],
                                    newTriangle.PositionParameters[1],
                                    newTriangle.PositionParameters[2]), Quaternion.identity);                                
                                go.GetComponent<TriangleController>().AttributesOfTheTriangle = newTriangle;
                                go.GetComponent<TriangleController>().SetUp();
                            }
                        }
                        ListOfSettlementPlaces.Add(go);
                    }


                }
            }
            GiveSettlementsToTrianglesContainer();
        }

        private void GiveSettlementsToTrianglesContainer()
        {
            gameObject.GetComponent<TrianglesContainer>().PlainTriangles = new PLAINTRIANGLE[PlainTrianglesAtTheBeginning.Count];
            gameObject.GetComponent<TrianglesContainer>().Settlements = new GameObject[ListOfSettlementPlaces.Count];
            for (int i = 0; i < PlainTrianglesAtTheBeginning.Count;i++ )
            {
                ListOfSettlementPlaces[i].transform.parent = ParentObjectForTriangles.transform;
                gameObject.GetComponent<TrianglesContainer>().PlainTriangles[i] = PlainTrianglesAtTheBeginning[i];
                gameObject.GetComponent<TrianglesContainer>().Settlements[i] = ListOfSettlementPlaces[i];
            }
        }
        private void GiveRoadsToRoadsContainer()
        {
            gameObject.GetComponent<RoadsContainer>().RoadsOfTheGame = new ROAD[RoadsOfTheGame.Count];
            gameObject.GetComponent<RoadsContainer>().RoadGameObjects = new GameObject[ListOfRoads.Count];
            for (int i = 0; i < RoadsOfTheGame.Count; i++)
            {               
                gameObject.GetComponent<RoadsContainer>().RoadsOfTheGame[i] = RoadsOfTheGame[i];
                gameObject.GetComponent<RoadsContainer>().RoadGameObjects[i] = ListOfRoads[i];
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
            Camera.main.transform.position = TilePositions[TilesSpawned[0][0], TilesSpawned[0][1]] + new Vector3(0, 1, -.8f);
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
       

        void TileSetter(int x, int y)
        {
            newTile = new TILE();
            // tömbből való megkereséshez

            newTile.PositionParameters = new float[] { TilePositions[x, y].x, TilePositions[x, y].y, TilePositions[x, y].z };
            newTile.RotationParameters = new float[] { 0, 0, 0 };
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
                rndx = UnityEngine.Random.Range(BorderValue, MaxX - BorderValue);
                rndy = UnityEngine.Random.Range(BorderValue, MaxY - BorderValue);
                TileSetter(rndx, rndy);
                TilesSpawned.Add(new int[] { rndx, rndy });
                TileCounterForSpawning++;
            }
        }
    }
    }

    


